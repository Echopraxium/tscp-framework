// Metadata UUID: a1b2-c3d4-core-loader-v5
namespace TSCP.Core

open System
open System.IO

module Loader =

    let private mapMetrics (obsList: Observation list) =
        let mutable m = SystemicMetrics.Default
        for obs in obsList do
            match obs.Invariant.ToLower() with
            | "entropy" | "m3:entropy" -> m <- { m with Entropy = obs.Value }
            | "negentropy" | "m3:negentropy" -> m <- { m with Negentropy = obs.Value }
            | _ -> ()
        m

    let transformM0ToConcept (model: M0Model) : Concept =
        let c = Concept.CreateEmpty model.Id model.DisplayName 0
        { c with 
            Metrics = mapMetrics model.Observations
            Metadata = model.Facets }
  
    /// Charge un contenu texte brut (.NET 10.0)
    let loadRawContent (path: string) =
        try
            if File.Exists(path) then
                let content = File.ReadAllText(path)
                sprintf "Content loaded (%d characters) from %s" content.Length path
            else
                sprintf "!! : file '%s' not found" path
        with ex ->
            sprintf "!! : System Error while loading: %s" ex.Message

    /// Crée un Concept minimaliste aligné sur le Domain v12
    let createMinimalConcept id name layer =
        { Id = id
          Name = name
          Layer = layer
          Facets = Map.empty
          Tags = Set.empty
          Axioms = []
          Metrics = SystemicMetrics.Default
          Metadata = Map.empty }