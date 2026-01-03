// uuid: a1b2c3d4-e5f6-4789-a1b2-c3d4e5f6a1b2
namespace TSCP.Tests.Infrastructure

open System
open System.Reflection
open Xunit
open TSCP.Core
open TSCP.Core.Domain
open TSCP.Core.Engine

/// <summary>
/// Infrastructure tests ensuring traceability and architectural integrity (M3).
/// </summary>
module ReflectionTests =

    // --- HELPER CORRIGÉ (FS0043 FIX) ---
    // On utilise Option.ofObj pour gérer le 'null' venant de la réflexion .NET
    // sans fâcher le compilateur F#.
    let private getUuid (t: Type) =
        t.GetCustomAttribute<UuidAttribute>()
        |> Option.ofObj 
        |> Option.map (fun attr -> attr.Uuid)

    // --- TESTS DE TRAÇABILITÉ ---

    [<Fact>]
    let ``Domain types must have valid traceability metadata`` () =
        let typesToCheck = [ 
            typeof<SystemicMetrics>
            typeof<Concept> 
            typeof<SystemicVector>
        ]
        
        typesToCheck |> List.iter (fun t ->
            match getUuid t with
            | Some uuid -> Assert.False(String.IsNullOrWhiteSpace(uuid), sprintf "%s UUID is empty." t.Name)
            | None -> Assert.Fail(sprintf "Type %s is missing [Uuid] attribute." t.Name)
        )

    [<Fact>]
    let ``SystemicMetrics metadata must match contract`` () =
        // UUID défini dans Engine.fs
        let expected = "a1b2c3d4-e5f6-4789-a1b2-c3d4e5f6a1b2" 
        match getUuid typeof<SystemicMetrics> with
        | Some uuid -> Assert.Equal(expected, uuid)
        | None -> Assert.Fail("SystemicMetrics missing contract UUID.")

    // --- TESTS DE LOGIQUE VECTORIELLE (M3) ---

    [<Fact>]
    let ``SystemicVector must enforce 4-Dimensional Orthogonality`` () =
        let props = typeof<SystemicVector>.GetProperties() |> Array.map (fun p -> p.Name)
        Assert.Contains("Structure", props)
        Assert.Contains("Information", props)
        Assert.Contains("Dynamics", props)
        Assert.Contains("Teleonomy", props)

    [<Fact>]
    let ``Engine verifies Coherence correctly`` () =
        // Test: Un système aligné (T ~= D) doit avoir une cohérence proche de 1.0
        let aligned = { zeroVector with Dynamics = 0.8; Teleonomy = 0.8 }
        let c1 = createConcept "Aligned" "Test" "" aligned []
        
        let m = Engine.calculateMetrics [c1]
        Assert.Equal(1.0, m.Coherence, 2)

    [<Fact>]
    let ``SeedData is populated with valid M2 Patterns`` () =
        let concepts = SeedData.getAllConcepts()
        Assert.NotEmpty(concepts)
        
        let homeostasis = concepts |> List.tryFind (fun c -> c.Id = "M2_HOMEOSTASIS")
        Assert.True(homeostasis.IsSome, "Homeostasis pattern not found in SeedData")
        Assert.True(homeostasis.Value.Signature.Teleonomy > 0.0)