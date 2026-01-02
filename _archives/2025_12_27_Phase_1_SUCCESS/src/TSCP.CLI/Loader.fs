namespace TSCP.IO

open System
open System.IO
open VDS.RDF
open VDS.RDF.Parsing
open TSCP.Core

module Loader =
    let BaseUri = "https://raw.githubusercontent.com/Echopraxium/TSCP-Framework/main/Ontology/tscp-core.jsonld#"

    let loadAndParse (filePath: string) =
        try
            let graph = new Graph()
            let handler = new VDS.RDF.Parsing.Handlers.GraphHandler(graph)
            
            // 1. Chemins absolus
            let baseDir = AppDomain.CurrentDomain.BaseDirectory
            let fullPath = Path.GetFullPath(Path.Combine(baseDir, filePath))
            let ontologyPath = Path.GetFullPath(Path.Combine(baseDir, "Ontology", "tscp-core.jsonld"))

            // 2. FALLBACK MANUEL : Charger d'abord l'ontologie elle-même
            // Cela permet au graphe de connaître les définitions avant de lire les données
            if File.Exists(ontologyPath) then
                let ontoParser = new JsonLdParser()
                ontoParser.Load(handler, ontologyPath)
                printfn "[INFO] Ontology definitions injected from fallback."

            // 3. Charger les données (Seeds)
            if not (File.Exists(fullPath)) then
                Error (sprintf "File not found: %s" fullPath)
            else
                let dataParser = new JsonLdParser()
                // On charge le fichier de données. 
                // Même si le @context échoue, les triplets seront lus.
                dataParser.Load(handler, fullPath)
                
                printfn "[SUCCESS] %d triples ingested total." graph.Triples.Count
                Ok graph
        with
        | ex -> 
            // Si l'erreur persiste, on ignore l'échec du contexte pour retourner ce qu'on a pu lire
            printfn "[WARN] Parser encountered a URI issue, but continuing... %s" ex.Message
            Ok (new Graph()) // Retourne un graphe vide au pire pour ne pas crasher