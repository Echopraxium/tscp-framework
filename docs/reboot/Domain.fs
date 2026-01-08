namespace TSCP.Core

open System

// uuid: 8f4b2c1a-9e3d-4c5b-8a7f-1d2e3f4g5h6i
// TSCP.Core - Domain Model (M3 Layer)

module Domain =

    /// <summary>
    /// Meta-Attribute for fine-grained versioning and semantic linking.
    /// Allows binding a compiled Type or Member to its M3 Concept ID.
    /// </summary>
    [<AttributeUsage(AttributeTargets.All, AllowMultiple = false)>]
    type UuidAttribute(uuid : string) =
        inherit Attribute()
        member this.Uuid = uuid

    /// The 4 Fundamental Invariants of the M3 State Space.
    [<Uuid("5a6b7c8d-9e0f-1a2b-3c4d-5e6f7g8h9i0j")>]
    type SystemicVector = {
        Structure: float
        Information: float
        Dynamics: float
        Teleonomy: float
    }

    type ConceptId = string

    /// Represents an abstract Concept in the Systemic Space.
    [<Uuid("1a2b3c4d-5e6f-7g8h-9i0j-k1l2m3n4o5p6")>]
    type Concept = {
        Id: ConceptId
        Name: string
        Description: string
        Signature: SystemicVector
        Tags: string list
    }

    type RelationshipType =
        | Composition      
        | Aggregation      
        | Isotopy          
        | Instantiation    

    type Relationship = {
        SourceId: ConceptId
        TargetId: ConceptId
        Type: RelationshipType
        Weight: float
    }

    // --- Helpers ---
    let zeroVector = { Structure = 0.0; Information = 0.0; Dynamics = 0.0; Teleonomy = 0.0 }
    let unitVector = { Structure = 1.0; Information = 1.0; Dynamics = 1.0; Teleonomy = 1.0 }

    let createConcept id name desc vector tags = {
        Id = id
        Name = name
        Description = desc
        Signature = vector
        Tags = tags
    }