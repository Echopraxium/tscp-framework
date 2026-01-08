namespace TSCP.Core

open System
open System.IO
open System.Text.Json
open System.Text.RegularExpressions
open TSCP.Core.Domain

// uuid: 40995f5c-9c9d-473d-9860-e7f8e8787878
// TSCP.Core - Catalog
// Manages the persistence and loading of Systemic Concepts from disk.
// Handles parsing of JSON-LD Ontologies (M1/M2) to extract vector signatures.
module Catalog =

    /// Returns the standard path for the catalog storage.
    let getCatalogPath () = 
        Path.Combine(Directory.GetCurrentDirectory(), "catalog")

    /// Options for JSON serialization.
    let private jsonOptions = 
        let options = JsonSerializerOptions()
        options.WriteIndented <- true
        options.PropertyNameCaseInsensitive <- true
        options

    // --- PARSING LOGIC FOR JSON-LD ---

    /// Parses a tensor string format: "[ S:0.4 | I:0.2 | D:0.2 | T:0.2 ]"
    /// Returns a SystemicVector.
    let private parseTensorString (tensorStr: string) =
        // Regex to extract the 4 values (handles integer and float)
        let pattern = @"S:([\d\.]+)\s*\|\s*I:([\d\.]+)\s*\|\s*D:([\d\.]+)\s*\|\s*T:([\d\.]+)"
        let m = Regex.Match(tensorStr, pattern)
        if m.Success then
            { 
                Structure   = float m.Groups.[1].Value
                Information = float m.Groups.[2].Value
                Dynamics    = float m.Groups.[3].Value
                Teleonomy   = float m.Groups.[4].Value
            }
        else
            // Fallback or Log warning (return zero vector if parsing fails)
            Domain.zeroVector

    /// Tries to extract a string property from a JsonElement (case insensitive fallback).
    /// FIXED: Uses a local mutable variable to satisfy 'byref' requirements.
    let private getProperty (elem: JsonElement) (propName: string) =
        let mutable value = Unchecked.defaultof<JsonElement>
        if elem.TryGetProperty(propName, &value) then
            if value.ValueKind = JsonValueKind.String then
                Some (value.GetString())
            else
                None
        else 
            None

    /// Parses a raw JSON-LD content to find Concepts.
    /// Handles the specific TSCP ontology format (nodes inside @graph).
    let private parseJsonLd (jsonContent: string) : Concept list =
        try
            let doc = JsonDocument.Parse(jsonContent)
            let root = doc.RootElement
            
            // Look for @graph array
            // FIXED: Uses 'graphNode' mutable to receive the property safely
            let mutable graphNode = Unchecked.defaultof<JsonElement>
            
            if root.TryGetProperty("@graph", &graphNode) && graphNode.ValueKind = JsonValueKind.Array then
                graphNode.EnumerateArray()
                |> Seq.choose (fun node ->
                    // We only want nodes that have an ID and a Tensor definition
                    let idOpt = getProperty node "@id"
                    
                    // Try multiple keys for Tensor (raw 'tensor' or full URI 'm3:tensorConfiguration')
                    let tensorStrOpt = 
                        match getProperty node "tensor" with
                        | Some t -> Some t
                        | None -> getProperty node "m3:tensorConfiguration"

                    match idOpt, tensorStrOpt with
                    | Some id, Some tStr ->
                        let name = defaultArg (getProperty node "rdfs:label") id
                        let desc = defaultArg (getProperty node "description") "Imported from Ontology"
                        let signature = parseTensorString tStr
                        
                        // Detect layer based on ID prefix
                        let tags = 
                            if id.StartsWith("m1:") then ["M1"; "Narrative"; "Imported"]
                            elif id.StartsWith("m2:") then ["M2"; "Pattern"; "Imported"]
                            else ["Imported"]

                        Some {
                            Id = id
                            Name = name
                            Description = desc
                            Signature = signature
                            Tags = tags
                        }
                    | _ -> None // Skip nodes without vector signature (like properties definitions)
                )
                |> Seq.toList
            else
                [] // No @graph found
        with ex ->
            printfn "Warning: Failed to parse JSON-LD content: %s" ex.Message
            []

    // --- DISK OPERATIONS ---

    /// Loads all concepts found in the catalog directory (looking for .jsonld files).
    /// Supports both "seed.jsonld" (per folder) and standalone ontology files.
    let loadFromDisk () : Concept list =
        let p = getCatalogPath()
        if not (Directory.Exists p) then 
            []
        else 
            // 1. Load from Folders (Standard Structure)
            let folderConcepts = 
                Directory.GetDirectories(p, "*") 
                |> Array.toList 
                |> List.choose (fun d ->
                    let f = Path.Combine(d, "seed.jsonld")
                    if File.Exists f then 
                        try 
                            let content = File.ReadAllText f
                            // Try standard deserialize first (for files created by saveToDisk)
                            try 
                                Some(JsonSerializer.Deserialize<Concept>(content, jsonOptions))
                            with _ -> 
                                // Fallback: try parsing as simple JSON-LD node
                                parseJsonLd content |> List.tryHead
                        with ex -> 
                            printfn "Error loading %s: %s" f ex.Message
                            None 
                    else None
                )

            // 2. Load from Standalone Ontology Files (e.g., TSCP_M1_Ontology_Core.jsonld placed in root)
            let ontologyConcepts =
                Directory.GetFiles(p, "*.jsonld")
                |> Array.toList
                |> List.collect (fun f ->
                    let content = File.ReadAllText f
                    parseJsonLd content
                )

            folderConcepts @ ontologyConcepts

    /// Saves a Concept to disk as a simple JSON (Standard Persistence).
    let saveToDisk (concept: Concept) =
        let p = getCatalogPath()
        let dir = Path.Combine(p, concept.Id.Replace(":","_")) // Sanitize ID for folder name
        if not (Directory.Exists dir) then Directory.CreateDirectory(dir) |> ignore
        
        let path = Path.Combine(dir, "seed.jsonld")
        let json = JsonSerializer.Serialize(concept, jsonOptions)
        File.WriteAllText(path, json)