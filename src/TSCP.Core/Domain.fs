namespace TSCP.Core

open System

/// <summary>
/// Domain Types for TSCP Framework.
/// </summary>
module Domain =

    // --- 1. ATTRIBUTS CROSS-LANGAGE (M3 Meta-Data) ---

    [<AttributeUsage(AttributeTargets.All, AllowMultiple = false)>]
    type UuidAttribute(uuid : string) =
        inherit Attribute()
        member this.Uuid = uuid

    // --- 2. TYPES FONDAMENTAUX ---

    /// The 4 Fundamental Invariants of the M3 State Space.
    [<Uuid("5a6b7c8d-9e0f-1a2b-3c4d-5e6f7g8h9i0j")>]
    type Axis = 
        | Structure 
        | Information
        | Dynamics 
        | Teleonomy

    [<CLIMutable>]
    type SystemicVector = {
        Structure : float
        Information : float
        Dynamics : float
        Teleonomy : float
    }

    [<CLIMutable>]
    type Concept = {
        Id : string
        Name : string
        Description : string
        Signature : SystemicVector
        Tags : string list
    }

    // --- 3. SESSION & HISTORIQUE ---

    type SessionEntry =
        | Log of string
        | ActiveConcept of Concept
        with
            // Propriétés pour le pattern matching C#
            member this.IsLogEntry = match this with Log _ -> true | _ -> false
            member this.IsConceptEntry = match this with ActiveConcept _ -> true | _ -> false
            
            // Helpers d'accès sécurisé (Zero Trust)
            member this.GetLogContent() = match this with Log s -> s | _ -> ""
            member this.GetConcept() = match this with ActiveConcept c -> Some c | _ -> None

    /// <summary>
    /// Represents the current state of the user session.
    /// </summary>
    type SessionState = {
        History : SessionEntry list
        ActiveLayer : float
    }
    with
        // Point d'entrée statique pour C#
        static member Default = { History = []; ActiveLayer = 1.0 }
        
    // Valeur par défaut pour les vecteurs
    let zeroVector = { Structure=0.0; Information=0.0; Dynamics=0.0; Teleonomy=0.0 }