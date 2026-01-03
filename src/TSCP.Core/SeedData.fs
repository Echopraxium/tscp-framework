namespace TSCP.Core

// uuid: 7a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p
// TSCP.Core - Seed Data
// This module populates the initial Graph with M3 Axioms and M2 Patterns.
module SeedData =
    open Domain

    let vec s i d t = { Structure = s; Information = i; Dynamics = d; Teleonomy = t }

    let getM3Invariants () = 
        [
            createConcept "M3_S" "Structure (|S>)" "Topology, Membrane, and Connectivity."
                (vec 1.0 0.0 0.0 0.0) ["M3"; "Invariant"]
            
            createConcept "M3_I" "Information (|I>)" "Semantics, Ontology, and Code."
                (vec 0.0 1.0 0.0 0.0) ["M3"; "Invariant"]
            
            createConcept "M3_D" "Dynamics (|D>)" "Transformation, Energy, and Flow."
                (vec 0.0 0.0 1.0 0.0) ["M3"; "Invariant"]
            
            createConcept "M3_T" "Teleonomy (|T>)" "Attractors, Tropisms, and Goal-seeking."
                (vec 0.0 0.0 0.0 1.0) ["M3"; "Invariant"]
        ]

    let getM2Patterns () =
        [
            // M2: Homeostasis
            createConcept "M2_HOMEOSTASIS" "Homeostasis" "Dynamic stability maintaining a setpoint."
                (vec 0.5 0.8 0.9 1.0) ["M2"; "Pattern"; "Cybernetic"]

            // M2: Feedback Loop
            createConcept "M2_FEEDBACK" "Feedback Loop" "Circular causality."
                (vec 0.9 0.5 0.9 0.2) ["M2"; "Pattern"; "Topology"]

            // M2: Hierarchy
            createConcept "M2_HIERARCHY" "Hierarchy" "Vertical organization of components."
                (vec 1.0 0.7 0.1 0.3) ["M2"; "Pattern"; "Architecture"]

            // --- NOUVEAUX PATTERNS POUR VALIDATION ISOTOPIQUE ---

            // M2: Active Learning (Le Chercheur / Vous)
            createConcept "M2_ACTIVE_LEARNING" "Active Learning Loop" 
                "Cognitive strategy using Dynamics to acquire Information and satisfy Teleonomy (Clarity)."
                // S:0.3 (Souple), I:0.9 (Signal), D:0.8 (Questionnement), T:0.9 (Compréhension)
                (vec 0.3 0.9 0.8 0.9) 
                ["M2"; "Pattern"; "Cognition"; "Negentropy"]

            // M2: Phototropism (L'Algue)
            createConcept "M2_PHOTOTROPISM" "Positive Phototropism" 
                "Biological movement towards a light source for survival."
                // S:0.4 (Cellule), I:0.8 (Lumière), D:0.7 (Flagelle), T:0.9 (Survie)
                (vec 0.4 0.8 0.7 0.9) 
                ["M2"; "Pattern"; "Biology"; "Tropism"]
        ]

    let getAllConcepts () =
        List.concat [ getM3Invariants(); getM2Patterns() ]

// End of SeedData module