// uuid: 550e8400-e29b-41d4-a716-446655440000
namespace TSCP.Session

open System
open System.IO
open System.Text.Json
open TSCP.Core

[<Uuid("550e8400-e29b-41d4-a716-446655440000")>]
type SessionKey = { Id: string; Tag: string }

[<Uuid("a4f10712-9c3a-4e8b-b1e5-8d2673f592a1")>]
type SessionEntry =
    | Log of string
    | ActiveConcept of Concept

[<Uuid("9d2c4b1a-8e7f-4c3d-9b5a-2e1f0d8c7b6a")>]
type SessionState = {
    History: SessionEntry list
    ActiveLayer: float // Continuum M2 <-> M3
    CurrentKey: SessionKey option 
}

module SessionManager =
    let private savePath = "session.json"

    let initialStore : SessionState = {
        History = [ Log "TSCP Continuum initialized" ]
        ActiveLayer = 0.0
        CurrentKey = Some { Id = "KEY_001"; Tag = "Initial" } 
    }

    let saveSession (state: SessionState) =
        try
            let options = JsonSerializerOptions(WriteIndented = true)
            let json = JsonSerializer.Serialize(state, options)
            File.WriteAllText(savePath, json)
        with _ -> ()

    let loadSession () : SessionState =
        if File.Exists(savePath) then
            try
                let json = File.ReadAllText(savePath)
                JsonSerializer.Deserialize<SessionState>(json)
            with _ -> initialStore
        else initialStore

    let incrementLayer (state: SessionState) (step: float) =
        let nextLayer = state.ActiveLayer + step
        let updated = { 
            state with 
                ActiveLayer = nextLayer
                History = Log (sprintf "Layer transitioned to %.2f" nextLayer) :: state.History 
        }
        saveSession updated
        updated

    let resetSession () =
        if File.Exists(savePath) then File.Delete(savePath)
        initialStore