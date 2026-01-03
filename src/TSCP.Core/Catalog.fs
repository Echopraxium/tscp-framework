namespace TSCP.Core

open System
open System.IO
open System.Text.Json
open System.Text.Json.Serialization
open TSCP.Core.Domain

// uuid: 40995f5c-9c9d-473d-9860-e7f8e8787878
// TSCP.Core - Catalog
// Manages the persistence and loading of Systemic Concepts from disk.

module Catalog =

    /// Returns the standard path for the catalog storage.
    let getCatalogPath () = 
        Path.Combine(Directory.GetCurrentDirectory(), "catalog")

    /// Options for JSON serialization (Case insensitive, formatted).
    let private jsonOptions = 
        let options = JsonSerializerOptions()
        options.WriteIndented <- true
        options.PropertyNameCaseInsensitive <- true
        options

    /// Loads all M0/M1/M2/M3 concepts found in the catalog directory.
    let loadFromDisk () : Concept list =
        let p = getCatalogPath()
        if not (Directory.Exists p) then 
            []
        else 
            // --- CORRECTION CRITIQUE ICI ---
            // AVANT : Directory.GetDirectories(p, "M0*")  <- Aveugle aux M2/M3
            // APRES : Directory.GetDirectories(p, "*")    <- Voit tout
            Directory.GetDirectories(p, "*") 
            |> Array.toList 
            |> List.choose (fun d ->
                let f = Path.Combine(d, "seed.jsonld")
                if File.Exists f then 
                    try 
                        let content = File.ReadAllText f
                        Some(JsonSerializer.Deserialize<Concept>(content, jsonOptions))
                    with ex -> 
                        printfn "Error loading %s: %s" f ex.Message
                        None 
                else 
                    None
            )

    /// Saves a Concept to disk as a JSON-LD definition.
    let saveToDisk (concept: Concept) =
        let p = getCatalogPath()
        let dir = Path.Combine(p, concept.Id) // Folder named after ID
        if not (Directory.Exists dir) then Directory.CreateDirectory(dir) |> ignore
        
        let path = Path.Combine(dir, "seed.jsonld")
        let json = JsonSerializer.Serialize(concept, jsonOptions)
        File.WriteAllText(path, json)

// // End of TSCP.Core namespace (Catalog)