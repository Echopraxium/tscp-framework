// uuid: e8d7c6b5-a4b3-4210-9123-f1e2d3c4b5a6
namespace TSCP.Tests.Core

open Xunit
open TSCP.Core

module DomainTests =

    [<Fact>]
    let ``Metrics should have default values`` () =
        let m = SystemicMetrics.Default
        Assert.Equal(0.0, m.Entropy)
        Assert.Equal(0.0, m.AntifragilityScore)

    [<Fact>]
    let ``Engine should calculate metrics correctly`` () =
        // FIX FS0039: Changed Create to CreateEmpty to match Domain.fs
        let concepts = [ Concept.CreateEmpty "1" "Test Concept" 1.0 ]
        let metrics = Engine.calculateMetrics concepts
        // Based on Engine logic: count (1) * 0.15 = 0.15
        Assert.Equal(0.15, metrics.AntifragilityScore)

    [<Fact>]
    let ``Engine should generate valid DOT graph from concepts`` () =
        // FIX FS0039: Changed Create to CreateEmpty
        let concept = Concept.CreateEmpty "node1" "Node Name" 1.0
        let state = { ActiveLayer = 1.0; History = [ ActiveConcept concept ] }
        let dot = Engine.generateDot state
        Assert.Contains("digraph TSCP", dot)
        Assert.Contains("Node Name", dot)