// uuid: 956cee92-f17d-4b67-8178-0049ee0d4173
namespace TSCP.Core

open System
open System.Text.Json.Serialization

[<AttributeUsage(AttributeTargets.All, AllowMultiple = false)>]
type UuidAttribute(uuid : string) =
    inherit Attribute()
    member this.Uuid = uuid

[<Uuid("a1b2c3d4-e5f6-4789-a1b2-c3d4e5f6a1b2")>]
type SystemicMetrics = {
    AntifragilityScore: float
    Isotropy: float
    PhaseDistance: float
    Entropy: float        
    Negentropy: float     
} with
    static member Default = 
        { AntifragilityScore = 0.0; Isotropy = 0.0; PhaseDistance = 0.0; Entropy = 0.0; Negentropy = 0.0 }

[<Uuid("b2c3d4e5-f6a1-4234-b2c3-d4e5f6a1b2c3")>]
type Concept = {
    Id: string
    Name: string                  
    Layer: float
    Facets: Map<string, string>   
    Tags: Set<string>             
    Axioms: string list
    Metrics: SystemicMetrics
    Metadata: Map<string, string>
} with 
    static member CreateEmpty id name (layer: float) =
        { Id = id; Name = name; Layer = layer; Facets = Map.empty; Tags = Set.empty; 
          Axioms = []; Metrics = SystemicMetrics.Default; Metadata = Map.empty }

type SessionEntry = Log of string | ActiveConcept of Concept
type SessionState = { ActiveLayer: float; History: SessionEntry list }

type Observation = { [<JsonPropertyName("invariant")>] Invariant: string; [<JsonPropertyName("value")>] Value: float }
type M0Model = { [<JsonPropertyName("system_id")>] Id: string; [<JsonPropertyName("description")>] Description: string; Observations: Observation list }
type CatalogRoot = { [<JsonPropertyName("@graph")>] Graph: M0Model list }