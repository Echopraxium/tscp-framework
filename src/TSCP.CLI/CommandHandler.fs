// uuid: 40995f5c-9c9d-473d-9860-e7f8e8787878
namespace TSCP.CLI

open System
open System.IO
open TSCP.Core
open TSCP.Session

module CommandHandler =

    let mutable private lastScanBuffer : (M0Model * string) list = []

    let executeCommand (command: string) (state: SessionState) : SessionState =
        let parts = command.Trim().Split([| ' ' |], StringSplitOptions.RemoveEmptyEntries) |> Array.toList
        
        match parts with
        | "catalog" :: _ ->
            printfn "\n--- TSCP Systemic Catalog Scan ---"
            let path = Catalog.getCatalogPath()
            printfn "Location: %s" path
            
            lastScanBuffer <- Catalog.loadFromDisk()
            
            if lastScanBuffer.IsEmpty then
                printfn "Warning: No 'seed.jsonld' found in 'catalog/M0*/' folders."
            else
                lastScanBuffer |> List.iteri (fun i (m, _) -> 
                    printfn "%d. [%s] %s" (i + 1) m.Id m.Description)
            state

        | "load" :: idOrIdx :: _ ->
            let items = if lastScanBuffer.IsEmpty then Catalog.loadFromDisk() else lastScanBuffer
            let target = 
                match Int32.TryParse(idOrIdx) with
                | (true, idx) when idx > 0 && idx <= items.Length -> Some (items.[idx - 1])
                | _ -> items |> List.tryFind (fun (m, _) -> m.Id = idOrIdx)
            
            match target with
            | Some (m, _) ->
                let c = Concept.CreateEmpty m.Id m.Description state.ActiveLayer
                printfn "Seed '%s' loaded." m.Id
                { state with History = ActiveConcept c :: Log (sprintf "Loaded %s" m.Id) :: state.History }
            | None -> 
                printfn "Error: '%s' not found. Check 'catalog' output." idOrIdx
                state

        | "help" :: _ ->
            printfn "\n=== COMMANDS ===\n catalog, load <idx/id>, list, analyse, graph, export, layer++, pwd, reset, exit"
            state

        | "exit" :: _ ->
            SessionManager.saveSession state
            Environment.Exit(0)
            state

        | cmd :: _ ->
            let newState, output = Engine.executeCommand cmd state
            output |> Option.iter (printfn "%s")
            newState

        | [] -> state