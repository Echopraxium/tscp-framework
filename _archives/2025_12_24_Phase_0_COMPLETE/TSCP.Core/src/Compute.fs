namespace TSCP.Core

// --- MOTEUR DE CALCUL ET EXPORTATION ---
module OperationUtils =
    
    // Version robuste du calcul de similarité
    let calculateSimilarity (obj1: TscpObject) (obj2: TscpObject) =
        // On transforme toutes les facettes en minuscules pour la comparaison
        let set1 = obj1.Facettes |> List.map (fun f -> f.ToLower().Trim()) |> Set.ofList
        let set2 = obj2.Facettes |> List.map (fun f -> f.ToLower().Trim()) |> Set.ofList
        
        if Set.isEmpty set1 && Set.isEmpty set2 then 1.0
        else
            let intersection = float (Set.intersect set1 set2).Count
            let union = float (Set.union set1 set2).Count
            if union = 0.0 then 0.0 else intersection / union

    // Calcul de similarité sur un sous-ensemble spécifique (Pivots)
    let calculatePivotSimilarity (obj1: TscpObject) (obj2: TscpObject) (pivots: string list) =
        let filterPivots (facettes: string list) =
            facettes 
            |> List.map (fun f -> f.ToLower().Trim())
            |> List.filter (fun f -> List.contains (f.ToLower()) pivots)
            |> Set.ofList

        let set1 = filterPivots obj1.Facettes
        let set2 = filterPivots obj2.Facettes
        
        if Set.isEmpty set1 && Set.isEmpty set2 then 0.0
        else
            let intersection = float (Set.intersect set1 set2).Count
            let union = float (Set.union set1 set2).Count
            intersection / union

        /// Calcule la Distance de Phase entre deux objets (0.0 = Identique, 1.0 = Rupture totale)
    let calculatePhaseDistance (obj1: TscpObject) (obj2: TscpObject) =
        let similarity = calculateSimilarity obj1 obj2
        1.0 - similarity

    /// Analyse la collision entre deux instances M0 via leurs ancêtres M2
    let analyzeSystemicCollision (model: FrameworkModel) (id1: string) (id2: string) =
        let obj1 = model.Objects |> List.tryFind (fun o -> o.Id = id1)
        let obj2 = model.Objects |> List.tryFind (fun o -> o.Id = id2)
        
        match obj1, obj2 with
        | Some a, Some b ->
            let dist = calculatePhaseDistance a b
            if dist > 0.7 then
                sprintf "RUPTURE CRITIQUE (Dφ=%.2f) : Les systemes sont en collision frontale." dist
            elif dist > 0.4 then
                sprintf "DIVERGENCE (Dφ=%.2f) : Risque de malentendu systemique." dist
            else
                sprintf "COHERENCE (Dφ=%.2f) : Les systemes partagent une base commune." dist
        | _ -> "Erreur : Identifiants introuvables."