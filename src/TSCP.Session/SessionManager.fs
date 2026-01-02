// uuid: a1b2c3d4-e5f6-4789-9876-543210abcdef
namespace TSCP.Session

open System.IO
open System.Text.Json
open TSCP.Core

module SessionManager =
    let private path = "session.json"
    let private opt = JsonSerializerOptions(WriteIndented = true)
    let initialStore = { ActiveLayer = 0.0; History = [ Log "TSCP Continuum initialized at L:0.0" ] }

    let loadSession () : SessionState =
        if File.Exists(path) then try JsonSerializer.Deserialize<SessionState>(File.ReadAllText(path), opt) with _ -> initialStore
        else initialStore

    let saveSession (s: SessionState) = File.WriteAllText(path, JsonSerializer.Serialize(s, opt))
    let resetSession () = initialStore
    let incrementLayer (state: SessionState) (step: float) =
        let newLayer = state.ActiveLayer + step
        { state with ActiveLayer = newLayer; History = Log (sprintf "Layer incremented to %.2f" newLayer) :: state.History }