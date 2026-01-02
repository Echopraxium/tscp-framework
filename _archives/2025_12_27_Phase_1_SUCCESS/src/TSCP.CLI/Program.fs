open System
open TSCP.Core
open TSCP.IO
open TSCP.Engine

[<EntryPoint>]
let main argv =
    Console.ForegroundColor <- ConsoleColor.Cyan
    printfn "==============================================="
    printfn "   TSCP FRAMEWORK CLI - Version 2.0 (.NET 10)  "
    printfn "   Sovereign Semantic Engine: Echopraxium      "
    printfn "==============================================="
    Console.ResetColor()

    // 1. Initialize the Triple Store with the GitHub-based Ontology
    let catalogPath = "catalog/seeds-catalog.jsonld"

    match Loader.loadAndParse catalogPath with
    | Ok graph ->
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "[SUCCESS] Global Seed Catalog loaded."
        // Ajoute ces deux lignes pour voir la "matière" sémantique
        printfn "[METRICS] Total Triples: %d" graph.Triples.Count
        printfn "[INFO] Active Base URI: %s" Loader.BaseUri
        Console.ResetColor()
        
        // Petite boucle pour lister les IDs des graines (Seeds)
        printfn "--- Seeds detected in State-Space ---"
        for triple in graph.Triples do
            // On cherche la relation 'rdf:type' pour identifier les entités
            if triple.Predicate.ToString().Contains("type") then
                printfn " > Found Concept: %O" triple.Subject
 
        // --- LA BOUCLE INTERACTIVE AMÉLIORÉE ---
        let rec loop () =
            printf "\ntscp-grow> "
            let input = Console.ReadLine().Trim().ToLower()
            
            match input with
            | "exit" | "quit" -> 
                printfn "[EXIT] Arrêt du moteur Echopraxium. À bientôt, Observateur."
                0
            
            | "stats" ->
                printfn "[METRICS] Le graphe contient actuellement %d triplets." graph.Triples.Count
                loop ()

            | "list" ->
                printfn "--- Entités détectées dans le catalogue ---"
                // On cherche tout ce qui a un label rdfs:label
                let labelPredicate = graph.CreateUriNode(Uri("http://www.w3.org/2000/01/rdf-schema#label"))
                let nodes = graph.GetTriplesWithPredicate(labelPredicate)
                for t in nodes do
                    printfn " > %O : %O" t.Subject t.Object
                loop ()

            | "debug" ->
                printfn "--- Inspection brute (10 premiers triplets) ---"
                graph.Triples |> Seq.truncate 10 |> Seq.iter (fun t -> 
                    printfn "  S: %O\n  P: %O\n  O: %O\n" t.Subject t.Predicate t.Object)
                loop ()

            | "grow" ->
                printfn "[ENGINE] Analyse de germination systémique en cours..."
                // Prochaine étape : Appel au TensorEngine
                loop ()

            | "" -> loop () // Gère l'appui sur Entrée sans texte

            | _ ->
                printfn "[HELP] Commandes disponibles : list, stats, debug, grow, exit"
                loop ()
        
        loop ()

    | Error msg ->
        Console.ForegroundColor <- ConsoleColor.Red
        printfn "[CRITICAL] Failed to initialize TSCP Engine: %s" msg
        Console.ResetColor()
        1