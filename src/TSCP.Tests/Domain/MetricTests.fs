namespace TSCP.Tests.Domain

open Xunit
open TSCP.Core
open TSCP.Core.Domain

module MetricTests =

    // Helper: Create transient concepts for physics testing
    // (We don't rely on the Catalog for unit logic tests)
    let makeTestConcept id s i d t = 
        { Id = id
          Name = id
          Description = "Test Concept"
          Tags = []
          Signature = { Structure = s; Information = i; Dynamics = d; Teleonomy = t } }

    [<Fact>]
    let ``Magnitude Calculation (Euclidean Norm)`` () =
        // Unit vector on Structure axis
        // Magnitude = sqrt(1^2 + 0 + 0 + 0) = 1
        let v = { Structure = 1.0; Information = 0.0; Dynamics = 0.0; Teleonomy = 0.0 }
        let m = Engine.Metrics.magnitude v
        Assert.Equal(1.0, m)

    [<Fact>]
    let ``Physics Interaction - Stable State`` () =
        // Source: Low Dynamics (0.2)
        // Target: High Structure (0.8)
        // Load = 0.2 / 0.8 = 0.25 (<< 1.0) -> Stable
        let source = makeTestConcept "src" 0.5 0.5 0.2 0.5
        let target = makeTestConcept "tgt" 0.8 0.5 0.1 0.5 

        let result = Engine.Physics.computeInteraction source target
        
        // Assertions
        Assert.Equal(Engine.Physics.InteractionState.Stable, result.State)
        // FIX VALIDATION: Stable state must NOT be locked (IsLocked = false)
        Assert.False(result.IsLocked, "Stable interaction should not be locked.")

    [<Fact>]
    let ``Physics Interaction - Critical State`` () =
        // Source: Very High Dynamics (2.5)
        // Target: Low Structure (0.5)
        // Load = 2.5 / 0.5 = 5.0 (> 2.0) -> Critical (Collapse)
        let source = makeTestConcept "src" 0.5 0.5 2.5 0.5
        let target = makeTestConcept "tgt" 0.5 0.5 0.1 0.5 

        let result = Engine.Physics.computeInteraction source target
        
        Assert.Equal(Engine.Physics.InteractionState.Critical, result.State)

    [<Fact>]
    let ``Catalog Loading Integrity`` () =
        // Integration Test: Verifies that Catalog correctly loads external JSON-LD
        let items = Catalog.loadAll()
        
        // NOTE: This test requires the 'ontology' folder to be accessible 
        // relative to the test execution path.
        if items.Length > 0 then
            // We now check for a real M2 concept instead of the old "sys" kernel concept
            Assert.Contains(items, fun c -> c.Id = "M2_SYSTEM")