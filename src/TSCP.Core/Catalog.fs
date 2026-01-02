// uuid: 40995f5c-9c9d-473d-9860-e7f8e8787878
namespace TSCP.Core

open System
open System.IO
open System.Text.Json

module Catalog =

    /// Resolves the absolute path to the solution root by searching for the .slnx file.
    let getSolutionRoot () = 
        let mutable dir = DirectoryInfo(Directory.GetCurrentDirectory())
        while dir <> null && dir.GetFiles("*.slnx").Length = 0 do
            dir <- dir.Parent
        if dir = null then Directory.GetCurrentDirectory() else dir.FullName

    /// Returns the path to the 'catalog' directory.
    let getCatalogPath () =
        Path.Combine(getSolutionRoot(), "catalog")

    /// Loads M0 models from the 'catalog/M0*/seed.jsonld' structure.
    let loadFromDisk () : (M0Model * string) list =
        let catalogDir = getCatalogPath()
        if not (Directory.Exists(catalogDir)) then
            []
        else
            Directory.GetDirectories(catalogDir, "M0*")
            |> Array.toList
            |> List.choose (fun subDir ->
                let seedPath = Path.Combine(subDir, "seed.jsonld")
                if File.Exists(seedPath) then
                    try
                        let json = File.ReadAllText(seedPath)
                        let model = JsonSerializer.Deserialize<M0Model>(json)
                        Some (model, seedPath)
                    with _ -> None
                else
                    None)