namespace TSCP.Core

open System
open System.IO
open System.Text
open TSCP.Core.Domain

// uuid: d8c7b6a5-94b3-4210-8019-e1d2c3b4a5f6
// TSCP.Core - Engine
// The Systemic Engine responsible for calculations, analysis, and graph operations.
module Engine =

    // --- MODULE DE MÉTRIQUES AVANCÉES (Vector Algebra) ---
    module Metrics =
        
        /// Calcule la Magnitude (Norme L2) : La "Taille/Puissance" du vecteur.
        let magnitude (v: SystemicVector) =
            sqrt (v.Structure**2.0 + v.Information**2.0 + v.Dynamics**2.0 + v.Teleonomy**2.0)

        /// Calcule le Produit Scalaire.
        let dotProduct (a: SystemicVector) (b: SystemicVector) =
            (a.Structure * b.Structure) + (a.Information * b.Information) + 
            (a.Dynamics * b.Dynamics) + (a.Teleonomy * b.Teleonomy)

        /// Calcule la Similarité Cosinus : L'Angle (La "Signature Méta").
        /// Retourne 1.0 si identiques (Isotopes), 0.0 si orthogonaux.
        /// Répond à la question : "Ont-ils le même but/design ?"
        let cosineSimilarity (a: SystemicVector) (b: SystemicVector) =
            let magA = magnitude a
            let magB = magnitude b
            if magA = 0.0 || magB = 0.0 then 0.0 
            else dotProduct a b / (magA * magB)

    // --- FIN MODULE MÉTRIQUES ---

    [<Uuid("a1b2c3d4-e5f6-4789-a1b2-c3d4e5f6a1b2")>] 
    type SystemicMetrics = {
        Entropy: float
        Coherence: float
        DominantInvariant: string
        AverageVector: SystemicVector
    }
    
    // Computes systemic metrics based on active concepts using Vector Algebra.
    let calculateMetrics (concepts: Concept list) =
        if concepts.IsEmpty then
            { Entropy = 0.0; Coherence = 0.0; DominantInvariant = "None"; AverageVector = zeroVector }
        else
            let count = float concepts.Length
            
            // 1. Calculate Average Vector (The "Center of Gravity" of the system)
            let sumVec = 
                concepts 
                |> List.fold (fun acc c -> 
                    { Structure = acc.Structure + c.Signature.Structure
                      Information = acc.Information + c.Signature.Information
                      Dynamics = acc.Dynamics + c.Signature.Dynamics
                      Teleonomy = acc.Teleonomy + c.Signature.Teleonomy }) zeroVector

            let avgVec = 
                { Structure = sumVec.Structure / count
                  Information = sumVec.Information / count
                  Dynamics = sumVec.Dynamics / count
                  Teleonomy = sumVec.Teleonomy / count }

            // 2. Determine Dominant Invariant
            let values = [ 
                ("Structure", avgVec.Structure); 
                ("Information", avgVec.Information); 
                ("Dynamics", avgVec.Dynamics); 
                ("Teleonomy", avgVec.Teleonomy) 
            ]
            let dominant = values |> List.maxBy snd |> fst

            // 3. Calculate Entropy (Inverted Information density)
            // Simplified: High Information + High Structure = Low Entropy
            let entropy = 1.0 - ((avgVec.Information + avgVec.Structure) / 2.0)

            // 4. Calculate Coherence (Alignment of Teleonomy vs Dynamics)
            // Does the system move (D) towards its goal (T)?
            let coherence = 1.0 - abs(avgVec.Teleonomy - avgVec.Dynamics)

            { Entropy = entropy; Coherence = coherence; DominantInvariant = dominant; AverageVector = avgVec }

    /// Generates a DOT representation (Graphviz) of the systemic continuum.
    let generateDot (state: SessionState) =
        let concepts = state.History |> List.choose (function | ActiveConcept c -> Some c | _ -> None)
        
        let nodes = 
            concepts 
            |> List.map (fun c -> 
                let s = c.Signature
                let label = sprintf "%s\\n[S:%.1f I:%.1f D:%.1f T:%.1f]" c.Name s.Structure s.Information s.Dynamics s.Teleonomy
                sprintf "  \"%s\" [label=\"%s\", shape=box, style=rounded];" c.Id label) 
            |> String.concat "\n"

        sprintf "digraph TSCP {\n  rankdir=LR;\n  node [fontname=\"Arial\"];\n  label=\"TSCP Layer: %.2f\";\n%s\n}" state.ActiveLayer nodes

    /// Generates a Markdown report of the current session.
    let generateMarkdown (state: SessionState) =
        let sb = StringBuilder()
        sb.AppendLine("# TSCP Systemic Report") |> ignore
        sb.AppendLine(sprintf "**Generated on:** %s" (DateTime.Now.ToString())) |> ignore
        sb.AppendLine("## Active Concepts") |> ignore
        
        state.History 
        |> List.choose (function | ActiveConcept c -> Some c | _ -> None)
        |> List.iter (fun c ->
            sb.AppendLine(sprintf "### %s (%s)" c.Name c.Id) |> ignore
            sb.AppendLine(sprintf "> %s" c.Description) |> ignore
            sb.AppendLine("| Structure | Information | Dynamics | Teleonomy |") |> ignore
            sb.AppendLine("| :---: | :---: | :---: | :---: |") |> ignore
            sb.AppendLine(sprintf "| %.2f | %.2f | %.2f | %.2f |" 
                c.Signature.Structure c.Signature.Information c.Signature.Dynamics c.Signature.Teleonomy) |> ignore
            sb.AppendLine("") |> ignore
        )
        
        // Add Analysis Section
        let concepts = state.History |> List.choose (function | ActiveConcept c -> Some c | _ -> None)
        if not concepts.IsEmpty then
            let metrics = calculateMetrics concepts
            sb.AppendLine("## Systemic Analysis") |> ignore
            sb.AppendLine(sprintf "* **Dominant Invariant:** %s" metrics.DominantInvariant) |> ignore
            sb.AppendLine(sprintf "* **System Entropy:** %.4f" metrics.Entropy) |> ignore
            sb.AppendLine(sprintf "* **Teleonomic Coherence:** %.4f" metrics.Coherence) |> ignore

        sb.ToString()

    /// Processes high-level systemic commands.
    /// Handles: analyze, sync, compare, and init.
    let executeCommand (cmd: string) (state: SessionState) : SessionState * string option =
        let parts = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        match parts with
        | [| "analyse" |] | [| "analyze" |] ->
            let concepts = state.History |> List.choose (function | ActiveConcept c -> Some c | _ -> None)
            let m = calculateMetrics concepts
            let msg = sprintf "Analysis: Dominant=%s, Entropy=%.2f, Coherence=%.2f" m.DominantInvariant m.Entropy m.Coherence
            { state with History = Log msg :: state.History }, Some msg

        | [| "compare"; id1; id2 |] ->
            // Trouve les concepts dans l'historique (ou catalogue si on étendait la logique)
            let find cId = state.History |> List.tryPick (function | ActiveConcept c when c.Id = cId -> Some c | _ -> None)
            
            match find id1, find id2 with
            | Some c1, Some c2 ->
                let similarity = Metrics.cosineSimilarity c1.Signature c2.Signature
                let msg = sprintf "Isotope Analysis [%s vs %s]: Cosine Similarity = %.4f" c1.Name c2.Name similarity
                { state with History = Log msg :: state.History }, Some msg
            | _ -> state, Some "Error: Concepts not found in active session."

        | [| "init" |] ->
            // BOOTSTRAP : Matérialise la mémoire SeedData sur le disque dur
            let seeds = SeedData.getAllConcepts()
            seeds |> List.iter Catalog.saveToDisk
            
            let msg = sprintf "FileSystem Initialized: %d atomic concepts materialized to ./catalog" seeds.Length
            { state with History = Log msg :: state.History }, Some msg

        | [| "load"; id |] ->
             // Charge un concept depuis le disque vers la session active
             // Note: Cette logique doit appeler Catalog.loadFromDisk (simplifié ici pour l'exemple)
             // Pour l'instant, nous chargeons depuis SeedData pour assurer que ça marche même sans disque
             let found = SeedData.getAllConcepts() |> List.tryFind (fun c -> c.Id = id || c.Name = id)
             match found with
             | Some c -> 
                 let msg = sprintf "Concept '%s' loaded into active context." c.Name
                 { state with History = ActiveConcept c :: state.History }, Some msg
             | None -> state, Some (sprintf "Concept '%s' not found." id)

        | [| "catalog" |] | [| "ls" |] ->
             // Liste ce qu'il y a sur le disque
             let all = Catalog.loadFromDisk()
             let count = all.Length
             let names = all |> List.map (fun c -> c.Name) |> String.concat ", "
             let msg = sprintf "Catalog (%d): %s" count names
             { state with History = Log msg :: state.History }, Some msg

        | [| "sync" |] ->
            let newState = { state with ActiveLayer = state.ActiveLayer + 0.5 }
            newState, Some (sprintf "Continuum shifted to Layer %.2f" newState.ActiveLayer)
            
        | _ -> state, None

// // End of TSCP.Core namespace (Engine)