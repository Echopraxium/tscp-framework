namespace TSCP.Engine

open System
open TSCP.Core

module Metrics =
    /// Jaccard Similarity for Isotopy
    let jaccard (s1: Set<string>) (s2: Set<string>) =
        let intersect = Set.intersect s1 s2 |> Set.count |> float
        let union = Set.union s1 s2 |> Set.count |> float
        if union = 0.0 then 1.0 else intersect / union

    /// Cosine Similarity for Phase Distance
    let cosineSimilarity (v1: Map<string, float>) (v2: Map<string, float>) =
        let allKeys = Set.union (v1 |> Map.keys |> Set.ofSeq) (v2 |> Map.keys |> Set.ofSeq)
        let dotProduct = 
            allKeys |> Seq.sumBy (fun k -> 
                (v1.TryFind k |> Option.defaultValue 0.0) * (v2.TryFind k |> Option.defaultValue 0.0))
        let mag1 = v1.Values |> Seq.sumBy (fun v -> v * v) |> sqrt
        let mag2 = v2.Values |> Seq.sumBy (fun v -> v * v) |> sqrt
        if mag1 * mag2 = 0.0 then 0.0 else dotProduct / (mag1 * mag2)

module Health =
    let checkViability (concept: Concept) =
        let score = (float concept.Facets.Count * 0.5) + (float concept.Tags.Count * 0.3)
        if score < 1.5 then 
            printfn "[WARN] Seed %s is below G=1 granularity thresholds." concept.Id
        else 
            printfn "[OK] Seed %s verified as viable G=1." concept.Id