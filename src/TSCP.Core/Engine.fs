// uuid: d8c7b6a5-94b3-4210-8019-e1d2c3b4a5f6
namespace TSCP.Core

open System
open System.IO
open System.Text.Json

module Engine =
    /// Computes systemic metrics based on active concepts.
    let calculateMetrics (concepts: Concept list) =
        let count = float concepts.Length
        { AntifragilityScore = count * 0.15
          Isotropy = 0.8
          PhaseDistance = 0.1
          Entropy = 1.0 / (count + 1.0)
          Negentropy = 1.0 - (1.0 / (count + 1.0)) }

    /// Generates a DOT representation of the systemic continuum.
    let generateDot (state: SessionState) =
        let concepts = state.History |> List.choose (function | ActiveConcept c -> Some c | _ -> None)
        let nodes = concepts |> List.map (fun c -> sprintf "  \"%s\" [label=\"%s (L:%.1f)\"];" c.Id c.Name c.Layer) |> String.concat "\n"
        sprintf "digraph TSCP {\n  rankdir=LR;\n  node [shape=box, style=rounded];\n  label=\"TSCP Layer: %.2f\";\n%s\n}" state.ActiveLayer nodes

    /// Loads a specific M0 model into the session state.
    let loadModel (model: M0Model) (state: SessionState) : SessionState =
        let concept = Concept.CreateEmpty model.Id model.Description state.ActiveLayer
        { state with History = ActiveConcept concept :: Log (sprintf "Loaded %s" model.Id) :: state.History }

    /// Processes high-level systemic commands.
    let executeCommand (cmd: string) (state: SessionState) : SessionState * string option =
        match cmd.ToLower().Trim() with
        | "analyse" ->
            let concepts = state.History |> List.choose (function | ActiveConcept c -> Some c | _ -> None)
            let m = calculateMetrics concepts
            let msg = sprintf "Systemic Analysis: Antifragility=%.2f, Entropy=%.2f" m.AntifragilityScore m.Entropy
            { state with History = Log msg :: state.History }, Some msg
        | "compare" ->
            let msg = sprintf "Phase Distance (Dphi) optimized at Layer %.2f" state.ActiveLayer
            { state with History = Log msg :: state.History }, Some msg
        | "sync" ->
            let newState = { state with ActiveLayer = state.ActiveLayer + 0.5 }
            newState, Some (sprintf "Continuum shifted to Layer %.2f" newState.ActiveLayer)
        | _ -> state, None