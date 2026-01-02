namespace TSCP.Core

open System.Text

// --- MOTEUR DE CALCUL ET EXPORTATION ---
module ExportUtils =
    // 2. Génération du Graphe pour Edotor (Format DOT)
    let generateDot (model: FrameworkModel) =
        let sb = StringBuilder()
        sb.AppendLine("digraph TSCP {") |> ignore
        sb.AppendLine("  rankdir=LR; node [shape=box, style=rounded, fontname=Arial];") |> ignore
        for obj in model.Objects do
            let color = match obj.Layer.ToUpper() with "M3" -> "gold" | "M2" -> "lightblue" | _ -> "lightgrey"
            // Nettoyage des IDs pour Graphviz
            let cleanId = obj.Id.Replace(":", "_").Replace("@", "").Replace("-", "_")
            let cleanLabel = obj.Label.Replace("è", "e").Replace("é", "e").Replace("à", "a")
            sb.AppendLine(sprintf "  %s [label=\"%s\\n(%s)\", fillcolor=%s, style=filled];" 
                cleanId cleanLabel obj.Layer color) |> ignore
            for rel in obj.Relations do
                let cleanTarget = rel.Key.Replace(":", "_").Replace("@", "").Replace("-", "_")
                sb.AppendLine(sprintf "  %s -> %s [label=\"%s\"];" cleanId cleanTarget rel.Value) |> ignore
        sb.AppendLine("}") |> ignore
        sb.ToString()

    // 3. Génération RDF Universelle (N-Triples pour SPARQL Playground)
    let generateNTriples (model: FrameworkModel) =
        let sb = StringBuilder()
        let ns = "https://tscp-framework.org/ontology/v1#"
        let rdfType = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type"
        let rdfsLabel = "http://www.w3.org/2000/01/rdf-schema#label"

        // Helper avec annotation de type explicite pour éviter l'erreur de surcharge
        let toUri (id: string) = 
            let cleanId = id.Replace("tscp:", "")
            sprintf "<%s%s>" ns cleanId

        for obj in model.Objects do
            let subject = toUri obj.Id
            
            // Type et Label
            sb.AppendLine(sprintf "%s <%s> %s ." subject rdfType (toUri obj.Type)) |> ignore
            sb.AppendLine(sprintf "%s <%s> \"%s\" ." subject rdfsLabel obj.Label) |> ignore
            
            // Layer et Facettes
            sb.AppendLine(sprintf "%s <%sLayer> \"%s\" ." subject ns obj.Layer) |> ignore
            for f in obj.Facettes do
                sb.AppendLine(sprintf "%s <%sFacette> \"%s\" ." subject ns f) |> ignore

            // Relations (Triplets de structure)
            for rel in obj.Relations do
                let predicate = sprintf "<%s%s>" ns rel.Value
                let objectUri = toUri rel.Key
                sb.AppendLine(sprintf "%s %s %s ." subject predicate objectUri) |> ignore
        
        sb.ToString()