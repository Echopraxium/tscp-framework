namespace TSCP.Core

open System
open System.Text
open TSCP.Core.Domain

// TSCP.Core - Engine v5.2
// The Systemic Engine responsible for calculations, analysis, and graph operations.
// Includes Semantic Profiling (Archetype vs Peer detection).
[<Uuid("d8c7b6a5-94b3-4210-8019-e1d2c3b4a5f6")>]
module Engine =

    // --- 1. TENSORIAL ALGEBRA ---
    module Metrics =
        
        let magnitude (v: SystemicVector) =
            sqrt (v.Structure**2.0 + v.Information**2.0 + v.Dynamics**2.0 + v.Teleonomy**2.0)

        let dotProduct (a: SystemicVector) (b: SystemicVector) =
            (a.Structure * b.Structure) + (a.Information * b.Information) + 
            (a.Dynamics * b.Dynamics) + (a.Teleonomy * b.Teleonomy)

        let cosineSimilarity (a: SystemicVector) (b: SystemicVector) =
            let magA = magnitude a
            let magB = magnitude b
            if magA = 0.0 || magB = 0.0 then 0.0 
            else dotProduct a b / (magA * magB)

    // --- 2. SYSTEMIC PHYSICS ---
    module Physics =
        
        type InteractionState =
            | Stable | Strained | Critical | Incompatible

        type InteractionResult = {
            State: InteractionState
            LoadFactor: float
            Resonance: float
            IsLocked: bool
            Message: string
        }

        let computeInteraction (source: Concept) (target: Concept) : InteractionResult =
            let src = source.Signature
            let tgt = target.Signature

            // Load = Dynamics(Source) / Structure(Target)
            // Prevent division by zero with a minimal structural floor
            let structuralCapacity = if tgt.Structure <= 0.001 then 0.001 else tgt.Structure
            let loadFactor = src.Dynamics / structuralCapacity

            // Resonance (Cosine Similarity)
            let resonance = Metrics.cosineSimilarity src tgt

            // Decision Logic
            let state, message, locked =
                if resonance < 0.1 then
                    Incompatible, "Orthogonal or opposite vectors. Semantic rejection.", false
                elif loadFactor > 2.0 then
                    Critical, "COLLAPSE: Source dynamics crush target structure.", false
                elif loadFactor > 1.0 then
                    Strained, "OVERLOAD: Target structure under tension (D > S).", true
                else
                    // Stable means "Fluid", so IsLocked must be FALSE
                    Stable, "Nominal Connection. System balanced.", false 

            { State = state; LoadFactor = loadFactor; Resonance = resonance; IsLocked = locked; Message = message }

        // Finds the 'k' nearest M2 archetypes or M1 peers for a given concept.
        // This acts as the "Semantic Profiler" (The Investigator's Board / Red Strings).
        let findNearestNeighbors (target: Concept) (allConcepts: Concept list) (k: int) =
            allConcepts
            // 1. Filter out self and purely technical properties (keep M1 and M2)
            |> List.filter (fun c -> c.Id <> target.Id && not (c.Tags |> List.contains "owl:Property"))
            // 2. Compute similarity
            |> List.map (fun c -> 
                let sim = Metrics.cosineSimilarity target.Signature c.Signature
                c, sim)
            // 3. Sort by similarity descending (Closest first)
            |> List.sortByDescending snd
            // 4. Keep Top K
            |> List.truncate k

    // --- 3. HELPER FUNCTIONS ---

    let loadModel (model: Concept) (state: SessionState) =
        { state with History = ActiveConcept model :: state.History }

    [<Uuid("a1b2c3d4-e5f6-4789-a1b2-c3d4e5f6a1b2")>] 
    type SystemicMetrics = {
        Entropy: float
        Coherence: float
        DominantInvariant: string
        AverageVector: SystemicVector
    }
    
    let calculateMetrics (concepts: Concept list) =
        if concepts.IsEmpty then
            { Entropy = 0.0; Coherence = 0.0; DominantInvariant = "None"; AverageVector = Domain.zeroVector }
        else
            let count = float concepts.Length
            
            let sumVec = 
                concepts 
                |> List.fold (fun acc c -> 
                    { Structure = acc.Structure + c.Signature.Structure
                      Information = acc.Information + c.Signature.Information
                      Dynamics = acc.Dynamics + c.Signature.Dynamics
                      Teleonomy = acc.Teleonomy + c.Signature.Teleonomy }) Domain.zeroVector

            let avgVec = 
                { Structure = sumVec.Structure / count
                  Information = sumVec.Information / count
                  Dynamics = sumVec.Dynamics / count
                  Teleonomy = sumVec.Teleonomy / count }

            let values = [ 
                ("Structure", avgVec.Structure); 
                ("Information", avgVec.Information); 
                ("Dynamics", avgVec.Dynamics); 
                ("Teleonomy", avgVec.Teleonomy) 
            ]
            let dominant = values |> List.maxBy snd |> fst

            let entropy = 1.0 - ((avgVec.Information + avgVec.Structure) / 2.0)
            let coherence = 1.0 - abs(avgVec.Teleonomy - avgVec.Dynamics)

            { Entropy = entropy; Coherence = coherence; DominantInvariant = dominant; AverageVector = avgVec }

    // --- 4. COMMAND EXECUTION ---

    let executeCommand (cmd: string) (state: SessionState) : SessionState * string option =
        let parts = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        match parts with
        
        | [| "simulate"; targetId |] ->
            match state.History |> List.tryPick (function ActiveConcept c -> Some c | _ -> None) with
            | Some source ->
                let allConcepts = Catalog.loadAll()
                match allConcepts |> List.tryFind (fun c -> c.Id = targetId || c.Name.Equals(targetId, StringComparison.OrdinalIgnoreCase)) with
                | Some target ->
                    let result = Physics.computeInteraction source target
                    let report = 
                        sprintf "\n--- TENSORIAL SIMULATION ---\n" +
                        sprintf "Source : %s [D:%.1f]\n" source.Name source.Signature.Dynamics +
                        sprintf "Target : %s [S:%.1f]\n" target.Name target.Signature.Structure +
                        sprintf "--------------------------\n" +
                        sprintf "PHYSICS : Load %.0f%% | Resonance %.2f\n" (result.LoadFactor * 100.0) result.Resonance +
                        sprintf "STATE   : %A\n" result.State +
                        sprintf "VERDICT : %s\n" result.Message
                    { state with History = Log report :: state.History }, Some report
                
                | None -> state, Some (sprintf "Target '%s' not found." targetId)
            | None -> state, Some "No active source. Load a concept first (load <id>)."

        | [| "analyse" |] | [| "analyze" |] ->
            let concepts = state.History |> List.choose (function | ActiveConcept c -> Some c | _ -> None)
            let m = calculateMetrics concepts
            let msg = sprintf "Analysis: Dominant=%s, Entropy=%.4f, Coherence=%.4f" m.DominantInvariant m.Entropy m.Coherence
            { state with History = Log msg :: state.History }, Some msg

        | [| "profile"; targetId |] ->
             let allConcepts = Catalog.loadAll()
             // Find target (in history or catalog)
             let targetOpt = 
                 state.History |> List.tryPick (function ActiveConcept c when c.Id = targetId -> Some c | _ -> None)
                 |> Option.orElse (allConcepts |> List.tryFind (fun c -> c.Id = targetId))

             match targetOpt with
             | Some target ->
                 // 1. Get a broad set of neighbors
                 let allNeighbors = Physics.findNearestNeighbors target allConcepts 100 

                 // 2. Distinguish Archetypes (M2 - The Zones) vs Peers (M1 - The Objects)
                 // Assumption: M2/M3 concepts start with M or have Ontology tag
                 let isArchetype (c: Concept) = 
                    c.Id.StartsWith("M3_") || c.Id.StartsWith("M2_") || c.Tags |> List.contains "Ontology"
                 
                 let archetypes = 
                     allNeighbors 
                     |> List.filter (fun (c, _) -> isArchetype c)
                     |> List.truncate 3 // Top 3 Semantic Zones
                 
                 let peers = 
                     allNeighbors 
                     |> List.filter (fun (c, _) -> not (isArchetype c))
                     |> List.truncate 5 // Top 5 Similar Instances

                 // 3. Build Report
                 let sb = StringBuilder()
                 sb.AppendLine(sprintf "\n=== SEMANTIC PROFILE: %s ===" target.Name) |> ignore
                 sb.AppendLine(sprintf "Vector: [S:%.2f | I:%.2f | D:%.2f | T:%.2f]" 
                     target.Signature.Structure target.Signature.Information 
                     target.Signature.Dynamics target.Signature.Teleonomy) |> ignore
                 
                 sb.AppendLine("\n--- NATURE (M2/M3 Archetypes) ---") |> ignore
                 if archetypes.IsEmpty then sb.AppendLine(" (No archetypal match found)") |> ignore
                 else
                     archetypes |> List.iter (fun (c, sim) ->
                         sb.AppendLine(sprintf " > %-25s : %.1f%% similarity" c.Name (sim * 100.0)) |> ignore
                     )

                 sb.AppendLine("\n--- PEERS (M1 Similar Bricks) ---") |> ignore
                 if peers.IsEmpty then sb.AppendLine(" (No similar M1 bricks found)") |> ignore
                 else
                     peers |> List.iter (fun (c, sim) ->
                         sb.AppendLine(sprintf " > %-25s : %.1f%% similarity" c.Name (sim * 100.0)) |> ignore
                     )
                 
                 let report = sb.ToString()
                 { state with History = Log report :: state.History }, Some report
             | None -> state, Some "Target concept not found."

        | [| "load"; targetId |] ->
             let allConcepts = Catalog.loadAll()
             match allConcepts |> List.tryFind (fun c -> c.Id = targetId || c.Name.Equals(targetId, StringComparison.OrdinalIgnoreCase)) with
             | Some c -> 
                let msg = sprintf "Concept loaded: %s" c.Name
                { state with History = ActiveConcept c :: state.History }, Some msg
             | None -> state, Some "Concept not found."

        | _ -> state, None