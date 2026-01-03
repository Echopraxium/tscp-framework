namespace TSCP.Core

open TSCP.Core.Domain

// uuid: c3d4e5f6-a7b8-4901-c2d3-e4f5a6b7c8d9
// TSCP.Core - Session Management
// Centralized management of user session and IDE state.

/// <summary>
/// Represents an entry in the session history (Log or Navigation).
/// </summary>
type SessionEntry =
    | Log of string
    | ActiveConcept of Concept

/// <summary>
/// The immutable state of the current session.
/// Contains history and the currently active context layer.
/// </summary>
type SessionState = {
    History: SessionEntry list
    /// The current "Zoom Level" or Abstraction Layer (M0..M3).
    ActiveLayer: float 
}

/// <summary>
/// Utility module to initialize or manipulate sessions.
/// </summary>
module SessionManager =
    
    /// <summary>
    /// Creates a default session for a cold start.
    /// </summary>
    let createDefault () =
        { 
            History = [ Log "TSCP System initialized. M3 Core loaded." ]
            ActiveLayer = 1.0 
        }

// // End of TSCP.Core namespace (Session)