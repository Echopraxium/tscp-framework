namespace TSCP.Core

// uuid: 7a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p
// TSCP.Core - Seed Data
// This module populates the initial Graph with M3 Axioms and the full M2 Periodic Table.
// Source of Truth: TSCP_M2_Ontology_Core.jsonld (v1.5.0)

module SeedData =
    open Domain

    /// Helper to create a SystemicVector (S, I, D, T)
    let vec s i d t = { Structure = s; Information = i; Dynamics = d; Teleonomy = t }

    /// <summary>
    /// The 4 Fundamental Invariants (M3 Substrate).
    /// </summary>
    let getM3Invariants () = 
        [
            createConcept "M3_S" "Structure (|S>)" "Topology, Membrane, and Connectivity."
                (vec 1.0 0.0 0.0 0.0) ["M3"; "Invariant"; "Analytic"]
            
            createConcept "M3_I" "Information (|I>)" "Semantics, Ontology, and Code."
                (vec 0.0 1.0 0.0 0.0) ["M3"; "Invariant"; "Analytic"]
            
            createConcept "M3_D" "Dynamics (|D>)" "Transformation, Energy, and Flow."
                (vec 0.0 0.0 1.0 0.0) ["M3"; "Invariant"; "Constructive"]
            
            createConcept "M3_T" "Teleonomy (|T>)" "Attractors, Tropisms, and Goal-seeking."
                (vec 0.0 0.0 0.0 1.0) ["M3"; "Invariant"; "Constructive"]
        ]

    /// <summary>
    /// The 50 Pattern Invariants (M2 Periodic Table).
    /// </summary>
    let getM2Patterns () =
        [
            // --- FAMILY 1: ARCHITECTURE & TOPOLOGY ---
            createConcept "M2_SYSTEM" "System" "A unified complex of interacting components."
                (vec 0.4 0.2 0.2 0.2) ["M2"; "Architecture"; "Constructive"]

            createConcept "M2_COMPONENT" "Component" "A part of a larger system."
                (vec 0.9 0.0 0.1 0.0) ["M2"; "Architecture"; "Constructive"]

            createConcept "M2_BOUNDARY" "Boundary" "The limit distinguishing system from environment."
                (vec 1.0 0.0 0.0 0.0) ["M2"; "Architecture"; "Constructive"]

            createConcept "M2_INTERFACE" "Interface" "Surface forming a common boundary."
                (vec 0.7 0.0 0.3 0.0) ["M2"; "Architecture"; "Constructive"]

            createConcept "M2_NETWORK" "Network" "Interconnected group or system."
                (vec 0.6 0.0 0.4 0.0) ["M2"; "Architecture"; "Constructive"]

            createConcept "M2_ENVIRONMENT" "Environment" "The surroundings or conditions."
                (vec 0.5 0.1 0.3 0.1) ["M2"; "Architecture"; "Constructive"]

            createConcept "M2_SUBSTRATE" "Substrate" "The underlying substance or layer."
                (vec 0.8 0.0 0.2 0.0) ["M2"; "Architecture"; "Constructive"]


            // --- FAMILY 2: PHYSICS & RESOURCES ---
            createConcept "M2_MATTER" "Matter" "Physical substance."
                (vec 1.0 0.0 0.0 0.0) ["M2"; "Physics"; "Constructive"]

            createConcept "M2_ENERGY" "Energy" "The capacity to do work."
                (vec 0.0 0.0 1.0 0.0) ["M2"; "Physics"; "Constructive"]

            createConcept "M2_RESOURCE" "Resource" "Stock or supply of money, materials, staff."
                (vec 0.5 0.0 0.5 0.0) ["M2"; "Physics"; "Constructive"]

            createConcept "M2_STOCK" "Stock" "Accumulated quantity."
                (vec 0.7 0.0 0.3 0.0) ["M2"; "Physics"; "Constructive"]

            createConcept "M2_FLOW" "Flow" "Movement of matter/energy."
                (vec 0.0 0.0 1.0 0.0) ["M2"; "Physics"; "Constructive"]

            createConcept "M2_GRADIENT" "Gradient" "Increase or decrease in the magnitude of a property."
                (vec 0.2 0.0 0.8 0.0) ["M2"; "Physics"; "Constructive"]


            // --- FAMILY 3: DYNAMICS & TIME ---
            createConcept "M2_EVENT" "Event" "A thing that happens, especially of importance."
                (vec 0.0 0.0 1.0 0.0) ["M2"; "Dynamics"; "Constructive"]

            createConcept "M2_PROCESS" "Process" "A series of actions or steps taken in order."
                (vec 0.2 0.1 0.7 0.0) ["M2"; "Dynamics"; "Constructive"]

            createConcept "M2_TRAJECTORY" "Trajectory" "The path followed by an object."
                (vec 0.1 0.0 0.7 0.2) ["M2"; "Dynamics"; "Constructive"]

            createConcept "M2_CYCLE" "Cycle" "A series of events that are regularly repeated."
                (vec 0.3 0.0 0.7 0.0) ["M2"; "Dynamics"; "Constructive"]

            createConcept "M2_INTERACTION" "Interaction" "Reciprocal action or influence."
                (vec 0.0 0.0 1.0 0.0) ["M2"; "Dynamics"; "Constructive"]

            createConcept "M2_TRANSDUCTION" "Transduction" "Conversion of one form of energy to another."
                (vec 0.2 0.0 0.8 0.0) ["M2"; "Dynamics"; "Constructive"]

            createConcept "M2_REPLICATION" "Replication" "The action of copying or reproducing."
                (vec 0.5 0.5 0.0 0.0) ["M2"; "Dynamics"; "Constructive"]


            // --- FAMILY 4: AGENCY ---
            createConcept "M2_AGENT" "Agent" "A being capable of acting."
                (vec 0.3 0.2 0.3 0.2) ["M2"; "Agency"; "Constructive"]

            createConcept "M2_ROLE" "Role" "The function assumed or part played."
                (vec 0.1 0.5 0.0 0.4) ["M2"; "Agency"; "Analytic"]

            createConcept "M2_IDENTITY" "Identity" "The fact of being who or what a person or thing is."
                (vec 0.8 0.2 0.0 0.0) ["M2"; "Agency"; "Analytic"]


            // --- FAMILY 5: INFORMATION & SEMANTICS ---
            createConcept "M2_INFORMATION" "Information" "What is conveyed or represented."
                (vec 0.0 1.0 0.0 0.0) ["M2"; "Information"; "Analytic"]

            createConcept "M2_LANGUAGE" "Language" "Method of human communication."
                (vec 0.3 0.7 0.0 0.0) ["M2"; "Information"; "Analytic"]

            createConcept "M2_SIGNAL" "Signal" "A gesture, action, or sound conveying information."
                (vec 0.0 0.5 0.5 0.0) ["M2"; "Information"; "Analytic"]

            createConcept "M2_CODE" "Code" "System of words, letters, figures, or other symbols."
                (vec 0.2 0.8 0.0 0.0) ["M2"; "Information"; "Analytic"]

            createConcept "M2_REPRESENTATION" "Representation" "The description or portrayal of something."
                (vec 0.3 0.7 0.0 0.0) ["M2"; "Information"; "Analytic"]

            createConcept "M2_DIAGNOSTIC" "Diagnostic" "Concerned with the diagnosis of illness or other problems."
                (vec 0.0 0.8 0.0 0.2) ["M2"; "Information"; "Analytic"]


            // --- FAMILY 6: LOGIC & CONTROL ---
            createConcept "M2_RULE" "Rule" "One of a set of explicit or understood regulations."
                (vec 0.0 0.7 0.0 0.3) ["M2"; "Logic"; "Analytic"]

            createConcept "M2_CONSTRAINT" "Constraint" "A limitation or restriction."
                (vec 0.8 0.0 0.2 0.0) ["M2"; "Logic"; "Constructive"]

            createConcept "M2_REGULATION" "Regulation" "A rule or directive made and maintained by an authority."
                (vec 0.0 0.5 0.0 0.5) ["M2"; "Logic"; "Analytic"]

            createConcept "M2_PROTOCOL" "Protocol" "The official procedure or system of rules."
                (vec 0.3 0.7 0.0 0.0) ["M2"; "Logic"; "Analytic"]

            createConcept "M2_THRESHOLD" "Threshold" "The magnitude or intensity that must be exceeded."
                (vec 0.0 1.0 0.0 0.0) ["M2"; "Logic"; "Analytic"]

            createConcept "M2_VARIABLE" "Variable" "An element, feature, or factor that is liable to vary."
                (vec 0.1 0.9 0.0 0.0) ["M2"; "Logic"; "Analytic"]

            createConcept "M2_PARAMETER" "Parameter" "A numerical or other measurable factor."
                (vec 0.5 0.5 0.0 0.0) ["M2"; "Logic"; "Analytic"]

            createConcept "M2_RELATION" "Relation" "The way in which two or more concepts are connected."
                (vec 0.0 1.0 0.0 0.0) ["M2"; "Logic"; "Analytic"]


            // --- FAMILY 7: TELEONOMY & GOALS ---
            createConcept "M2_OBJECTIVE" "Objective" "A thing aimed at or sought; a goal."
                (vec 0.0 0.2 0.0 0.8) ["M2"; "Teleonomy"; "Analytic"]

            createConcept "M2_FUNCTION" "Function" "An activity or purpose natural to or intended for a person or thing."
                (vec 0.3 0.0 0.2 0.5) ["M2"; "Teleonomy"; "Analytic"]

            createConcept "M2_STRATEGY" "Strategy" "A plan of action or policy designed to achieve a major or overall aim."
                (vec 0.1 0.4 0.0 0.5) ["M2"; "Teleonomy"; "Analytic"]

            createConcept "M2_ATTRACTOR" "Attractor" "A set of numerical values toward which a system tends to evolve."
                (vec 0.0 0.0 0.0 1.0) ["M2"; "Teleonomy"; "Analytic"]


            // --- FAMILY 8: COMPLEX PROPERTIES ---
            createConcept "M2_STATE" "State" "The particular condition that someone or something is in."
                (vec 0.5 0.5 0.0 0.0) ["M2"; "Complex"; "Analytic"]

            createConcept "M2_EQUILIBRIUM" "Equilibrium" "A state in which opposing forces or influences are balanced."
                (vec 0.5 0.0 0.5 0.0) ["M2"; "Complex"; "Constructive"]

            createConcept "M2_STABILITY" "Stability" "The state of being stable."
                (vec 0.6 0.0 0.2 0.2) ["M2"; "Complex"; "Analytic"]

            createConcept "M2_RESILIENCE" "Resilience" "The capacity to recover quickly from difficulties."
                (vec 0.4 0.0 0.4 0.2) ["M2"; "Complex"; "Constructive"]

            createConcept "M2_SYNERGY_POS" "Synergy (+)" "Interaction producing a combined effect greater than the sum."
                (vec 0.2 0.0 0.8 0.0) ["M2"; "Complex"; "Constructive"]

            createConcept "M2_SYNERGY_NEG" "Synergy (-)" "Interaction producing a combined effect less than the sum (Interference)."
                (vec 0.2 0.0 0.8 0.0) ["M2"; "Complex"; "Constructive"]

            createConcept "M2_BIFURCATION" "Bifurcation" "The division of something into two branches or parts."
                (vec 0.0 0.5 0.5 0.0) ["M2"; "Complex"; "Analytic"]

            createConcept "M2_DYSFUNCTION" "Dysfunction" "Abnormality or impairment."
                (vec 0.5 0.0 0.5 0.0) ["M2"; "Complex"; "Analytic"]

            createConcept "M2_ADAPTATION" "Adaptation" "The action or process of adapting or being adapted."
                (vec 0.4 0.2 0.2 0.2) ["M2"; "Complex"; "Constructive"]
        ]

    /// <summary>
    /// Returns the complete set of Seed Concepts (M3 + M2).
    /// Used by 'init' command to populate the catalog.
    /// </summary>
    let getAllConcepts () =
        List.concat [ getM3Invariants(); getM2Patterns() ]