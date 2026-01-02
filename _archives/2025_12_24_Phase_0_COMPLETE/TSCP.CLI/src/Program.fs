namespace TSCP.CLI

open TSCP.Core

open System
open System.IO

module Program =
    let findObj (model: FrameworkModel) id =
        model.Objects |> List.tryFind (fun o -> o.Id = id)

    let rec mainLoop (model: FrameworkModel) =
        printf "\nTSCP.CLI> "
        let inputLine = Console.ReadLine()
        
        if String.IsNullOrWhiteSpace(inputLine) then 
            mainLoop model 
        else
            let input = inputLine.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
            let cmd = input.[0].ToLower()

            match cmd with
            | "load" ->
                let path = if input.Length > 1 then input.[1] else "model.jsonld"
                match PersistenceUtils.load path with
                | Ok nm -> 
                    printfn "Succès : %d objets chargés." nm.Objects.Length
                    mainLoop nm
                | Error e -> 
                    printfn "Erreur : %s" e
                    mainLoop model

            | "list" -> 
                printfn "\n--- OBJETS DANS LE MODÈLE ---"
                model.Objects |> List.iter (fun o -> 
                    printfn "[%s] %-30s : %s" o.Layer o.Id o.Label)
                mainLoop model

            | "compare" ->
                if input.Length < 3 then 
                    printfn "Usage: compare idA idB"
                else
                    match (findObj model input.[1]), (findObj model input.[2]) with
                    | Some a, Some b -> 
                        let similarity = OperationUtils.calculateSimilarity a b                        // Mettez :
                        printfn "Distance de Phase (Dφ) : %.4f" (1.0 - similarity)
                    | _ -> printfn "Identifiant(s) introuvable(s)."
                mainLoop model

            | "analyze" ->
                if input.Length < 3 then 
                    printfn "Usage: analyze tscp:id1 tscp:id2"
                else
                    let result = OperationUtils.analyzeSystemicCollision model input.[1] input.[2]
                    printfn "\n--- ANALYSE DE COLLISION SYSTÉMIQUE ---"
                    printfn "%s" result
                mainLoop model

            | "pivot" ->
                if input.Length < 4 then 
                    printfn "Usage: pivot [seuil 0-1] [facette1,facette2,...] tscp:id1 tscp:id2"
                else
                    let seuil = float input.[1]
                    let pivots = input.[2].Split(',') |> Array.toList |> List.map (fun s -> s.ToLower().Trim())
                    let id1, id2 = input.[3], input.[4]
                    
                    match (findObj model id1), (findObj model id2) with
                    | Some a, Some b ->
                        let sim = OperationUtils.calculatePivotSimilarity a b pivots
                        let dist = 1.0 - sim
                        printfn "\n--- ANALYSE DE SYNCHRONISATION SUR PIVOTS ---"
                        printfn "Facettes analysées : %A" pivots
                        printfn "Indice de Synchro : %.2f (Seuil requis : %.2f)" sim seuil
                        
                        if sim >= seuil then
                            printfn "RÉSULTAT : ACTION COLLECTIVE ENGAGÉE (Divergence résiduelle : %.2f)" dist
                        else
                            printfn "RÉSULTAT : DÉSYNCHRONISATION (L'essaim se fragmente)"
                    | _ -> printfn "Identifiants introuvables."
                mainLoop model

            | "graph" ->
                let dot = ExportUtils.generateDot model
                File.WriteAllText("model.dot", dot)
                printfn "Fichier 'model.dot' généré."
                mainLoop model

            | "export" ->
                let nt = ExportUtils.generateNTriples model
                File.WriteAllText("model.nt", nt)
                printfn "Export RDF réussi : model.nt"
                mainLoop model

            | "exit" -> printfn "Fermeture."
            | _ -> 
                printfn "Commandes: load, list, compare, analyze, graph, export, exit"
                mainLoop model

    [<EntryPoint>]
    let main argv =
        Console.OutputEncoding <- System.Text.Encoding.UTF8
        printfn "=== TSCP CLI v0.1 ==="
        let initialModel = { Context = Map.empty; Type = "FrameworkModel"; Version = "1.6.8"; Objects = [] }
        mainLoop initialModel
        0