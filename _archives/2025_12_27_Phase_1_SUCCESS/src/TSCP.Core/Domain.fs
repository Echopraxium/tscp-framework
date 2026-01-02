namespace TSCP.Core

open System

/// Represents the Ontological Layers of the TSCP Framework
type Layer = 
    | M3 // Invariants: Universal Laws (Antifragility, Entropy)
    | M2 // Observer: Perspective, Resolution, and Intent
    | M1 // Standards: Reusable Protocols, APIs, and Patterns
    | M0 // Instances: Specific Semantic Seeds and Real-world systems

/// Represents a Systemic Concept (Seed) within the State-Space
type Concept = {
    Id: string                  // Unique URI (e.g., tscp:M0/MTG_Massive)
    Layer: Layer                // Ontological stratification
    Facets: Map<string, float>  // Tensor dimensions (0.0 to 1.0)
    Tags: Set<string>           // Semantic labels for Jaccard calculation
    Axioms: Map<string, obj>    // Rules and structural constants
    Metadata: Map<string, string> // Descriptive info (Labels, Comments)
}

/// Represents an interaction or link between two concepts in the Triple Store
type Interaction = {
    SourceId: string
    TargetId: string
    RelationType: string
    Weight: float               // Semantic strength/proximity
}

/// Evaluation metrics for Systemic Health and Phase
type SystemicMetrics = {
    Isotropy: float             // Calculated via Jaccard Index
    PhaseDistance: float        // Calculated via Cosine Similarity
    AntifragilityScore: float   // Nonlinearity of response to stress
}

/// Defines the granularity level of a seed (G0, G1, G2)
type Granularity = 
    | G0 // Raw data
    | G1 // Semantic Seed (Standard TSCP start)
    | G2 // Fully deployed state-space