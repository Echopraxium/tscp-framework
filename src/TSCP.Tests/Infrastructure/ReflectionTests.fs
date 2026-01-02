// uuid: a1b2c3d4-e5f6-4789-a1b2-c3d4e5f6a1b2
namespace TSCP.Tests.Infrastructure

open System
open System.Reflection
open Xunit
open TSCP.Core

/// <summary>
/// Infrastructure tests ensuring that all core domain types maintain 
/// mandatory traceability metadata through UuidAttributes.
/// </summary>
module ReflectionTests =

    /// <summary>
    /// Safely retrieves the UUID from a type decorated with UuidAttribute.
    /// Handles missing attributes to prevent NullReferenceExceptions during scan.
    /// </summary>
    let private getUuid (t: Type) =
        let attr = t.GetCustomAttribute<UuidAttribute>()
        if box attr = null then 
            None 
        else 
            Some attr.Uuid

    /// <summary>
    /// Validates that all critical domain types are decorated with a non-empty UUID.
    /// </summary>
    [<Fact>]
    let ``Domain module types must have valid traceability metadata`` () =
        let domainTypes = [ 
            typeof<SystemicMetrics>
            typeof<Concept> 
        ]
        
        domainTypes |> List.iter (fun t ->
            match getUuid t with
            | Some uuid -> 
                Assert.False(String.IsNullOrWhiteSpace(uuid), sprintf "Type %s has an empty UUID string." t.Name)
            | None -> 
                Assert.Fail(sprintf "Type %s is missing the mandatory UuidAttribute for systemic traceability." t.Name))

    /// <summary>
    /// Ensures that specific core types match their hardcoded traceability contract.
    /// </summary>
    [<Fact>]
    let ``Domain module metadata must match the expected traceability UUID`` () =
        let expectedMetricsUuid = "a1b2c3d4-e5f6-4789-a1b2-c3d4e5f6a1b2"
        
        match getUuid typeof<SystemicMetrics> with
        | Some uuid -> 
            Assert.Equal(expectedMetricsUuid, uuid)
        | None -> 
            Assert.Fail("SystemicMetrics type is missing its UuidAttribute contract.")