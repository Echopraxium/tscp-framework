namespace TSCP.Session

open System.IO
open TSCP.Core

module Engine =
    
    let load (state: SessionState) (target: string) =
        let newState = { state with History = (sprintf "Loaded: %s" target) :: state.History }
        SessionManager.saveSession newState
        newState

    let analyse (state: SessionState) =
        let result = Logic.evaluateLayerComplexity state.ActiveLayer state.History
        let newState = { state with History = (sprintf "Analysis: %s" result) :: state.History }
        SessionManager.saveSession newState
        newState

    let grow (state: SessionState) =
        let step = Logic.calculateGrowthStep state.ActiveLayer
        let nextLayer = state.ActiveLayer + step
        let newState = { state with 
                            ActiveLayer = nextLayer
                            History = (sprintf "Growth: +%d (Total: %d)" step nextLayer) :: state.History }
        SessionManager.saveSession newState
        newState

    /// APPEL À LOGIC.COMPARE CORRIGÉ
    let compare (state: SessionState) (oldLayer: int) =
        let diff = Logic.compare state.ActiveLayer oldLayer
        let newState = { state with History = (sprintf "Comparison: %s" diff) :: state.History }
        SessionManager.saveSession newState
        newState

    let exportRDF (state: SessionState) =
        let rdf = Logic.generateRDF state.ActiveLayer state.History
        File.WriteAllText("export.ttl", rdf)
        { state with History = "RDF export done" :: state.History }

    let generateGraph (state: SessionState) =
        let dot = Logic.generateDot state.ActiveLayer state.History
        File.WriteAllText("session_graph.dot", dot)
        { state with History = "GraphViz export done" :: state.History }