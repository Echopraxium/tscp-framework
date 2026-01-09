namespace TSCP.Core

open System
open System.IO
open System.Text.Json
open System.Text.RegularExpressions
open TSCP.Core.Domain

// TSCP.Core - Catalog (Ontology-First Architecture)
// Version: Semantic Filtering with Explicit M3 Configuration
module Catalog =

    // --- 1. PATH MANAGEMENT ---
    let getCatalogPath () = 
        let path = Path.Combine(Directory.GetCurrentDirectory(), "catalog")
        if not (Directory.Exists path) then Directory.CreateDirectory(path) |> ignore
        path

    let getOntologyPath () =
        let current = Directory.GetCurrentDirectory()
        // Recursive lookup to find the 'ontology' folder
        let candidates = 
            [ Path.Combine(current, "ontology")
              Path.Combine(current, "..", "ontology")
              Path.Combine(current, "..", "..", "ontology")
              Path.Combine(current, "..", "..", "..", "ontology") ]
        candidates |> List.tryFind Directory.Exists

    let private jsonOptions = 
        let options = JsonSerializerOptions()
        options.WriteIndented <- true
        options.PropertyNameCaseInsensitive <- true
        options

    // --- 2. KERNEL DATA (Empty) ---
    // We rely entirely on JSON-LD files now.
    let private kernelConcepts : Concept list = []

    // --- 3. JSON-LD PARSER & TENSORS ---

    let private parseTensorString (tensorStr: string) =
        let pattern = @"S:([\d\.-]+)\s*\|\s*I:([\d\.-]+)\s*\|\s*D:([\d\.-]+)\s*\|\s*T:([\d\.-]+)"
        let m = Regex.Match(tensorStr, pattern)
        if m.Success then 
            { Structure = float m.Groups.[1].Value
              Information = float m.Groups.[2].Value
              Dynamics = float m.Groups.[3].Value
              Teleonomy = float m.Groups.[4].Value }
        else Domain.zeroVector

    // CONFIGURATION LOGIC FOR M3 META-OBJECTS
    let private getM3Configuration (id: string) =
        match id with
        // A. FUNDAMENTAL DIMENSIONS (Basis Vectors)
        | "M3_STRUCTURE"   -> { Domain.zeroVector with Structure = 1.0 }
        | "M3_INFORMATION" -> { Domain.zeroVector with Information = 1.0 }
        | "M3_DYNAMICS"    -> { Domain.zeroVector with Dynamics = 1.0 }
        | "M3_TELEONOMY"   -> { Domain.zeroVector with Teleonomy = 1.0 }
        
        // B. TENSOR SPACES (Projectors / Masks)
        // 1.0 indicates the dimension is valid in this sub-space.
        | "M3_ANALYTICALSPACE"   -> { Structure = 1.0; Information = 1.0; Dynamics = 0.0; Teleonomy = 0.0 }
        | "M3_CONSTRUCTIVESPACE" -> { Structure = 0.0; Information = 0.0; Dynamics = 1.0; Teleonomy = 1.0 }

        // C. THE SUBSTRATE (Totality)
        // Represents the full 4D Hilbert Space.
        | "M3_HILBERTSUBSTRATE"  -> { Structure = 1.0; Information = 1.0; Dynamics = 1.0; Teleonomy = 1.0 }
        
        // Default (parsed later via JSON for M2/M1)
        | _ -> Domain.zeroVector

    let private parseJsonLd (content: string) : Concept list =
        try
            let doc = JsonDocument.Parse(content)
            let root = doc.RootElement
            let mutable graph = Unchecked.defaultof<JsonElement>
            
            if root.TryGetProperty("@graph", &graph) then
                [ for element in graph.EnumerateArray() do
                    
                    // A. TYPE ANALYSIS
                    let mutable typeProp = Unchecked.defaultof<JsonElement>
                    let typeStr = 
                        if element.TryGetProperty("@type", &typeProp) then typeProp.GetString()
                        else ""

                    // B. SEMANTIC FILTER
                    // Exclude syntactic tools (Properties, Ontologies)
                    // Keep systemic objects (Invariants, Spaces, Classes, Concepts)
                    let isMetaDefinition = 
                        typeStr.Contains("owl:Property") || 
                        typeStr.Contains("owl:Ontology") 

                    if not isMetaDefinition then
                        // C. EXTRACTION
                        // ID Normalization
                        let mutable idProp = Unchecked.defaultof<JsonElement>
                        let id = 
                             if element.TryGetProperty("@id", &idProp) then 
                                idProp.GetString()
                                      .Replace("m3:", "M3_")
                                      .Replace("m2:", "M2_")
                                      .Replace("m1:", "M1_")
                                      .ToUpper()
                             else "UNKNOWN"

                        // Name
                        let mutable nameProp = Unchecked.defaultof<JsonElement>
                        let name =
                            if element.TryGetProperty("rdfs:label", &nameProp) then nameProp.GetString()
                            else id

                        // Description
                        let mutable descProp = Unchecked.defaultof<JsonElement>
                        let description =
                            if element.TryGetProperty("rdfs:comment", &descProp) then descProp.GetString()
                            else "Imported from Ontology"

                        // D. TENSOR LOGIC
                        let mutable tensorProp = Unchecked.defaultof<JsonElement>
                        let signature = 
                            // 1. Explicit Tensor (M2 Concepts)
                            if element.TryGetProperty("m3:tensorConfiguration", &tensorProp) then 
                                parseTensorString (tensorProp.GetString())
                            elif element.TryGetProperty("tensor", &tensorProp) then 
                                parseTensorString (tensorProp.GetString())
                            else
                                // 2. Implicit Configuration (M3 Spaces & Dimensions)
                                getM3Configuration id

                        // E. FINAL VALIDATION
                        if id <> "UNKNOWN" then
                            { Id = id; Name = name; Description = description; Signature = signature; Tags = ["Ontology"; typeStr] }
                ]
            else []
        with ex -> 
            printfn "[ERROR] Parsing Ontology JSON-LD: %s" ex.Message
            []

    // --- 4. PUBLIC API ---

    let loadAll () =
        let kernel = kernelConcepts

        let ontologyConcepts =
            match getOntologyPath() with
            | Some path ->
                try
                    Directory.GetFiles(path, "*.jsonld")
                    |> Array.toList
                    |> List.collect (fun f -> File.ReadAllText f |> parseJsonLd)
                with _ -> []
            | None -> []

        let userConcepts =
            let p = getCatalogPath()
            try
                Directory.GetFiles(p, "*.json", SearchOption.AllDirectories)
                |> Array.toList
                |> List.choose (fun f ->
                    try 
                        let c = JsonSerializer.Deserialize<Concept>(File.ReadAllText f, jsonOptions)
                        if box c <> null then Some c else None
                    with _ -> None)
            with _ -> []

        (kernel @ ontologyConcepts @ userConcepts)
        |> List.filter (fun c -> not (String.IsNullOrWhiteSpace(c.Id)) && c.Id <> "UNKNOWN")
        |> List.groupBy (fun c -> c.Id)
        |> List.map (fun (_, instances) -> List.last instances)

    let saveToDisk (concept: Concept) =
        let p = getCatalogPath()
        let safeId = concept.Id.Replace(":", "_").Replace("/", "_").Replace("\\", "_")
        let filename = sprintf "%s.json" safeId
        let path = Path.Combine(p, filename)
        let json = JsonSerializer.Serialize(concept, jsonOptions)
        File.WriteAllText(path, json)