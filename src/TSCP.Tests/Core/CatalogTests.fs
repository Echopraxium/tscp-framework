// uuid: e9a8b7c6-d5e4-4321-b1a2-c3d4e5f6a7b8
namespace TSCP.Tests.Core

open System
open System.IO
open Xunit
open TSCP.Core

/// <summary>
/// Unit tests for the Catalog discovery engine and M3 invariant validation logic.
/// Ensures deterministic path resolution from the solution root.
/// </summary>
module CatalogTests =

    /// <summary>
    /// Validates that the solution root resolution (via CallerFilePath) is operational.
    /// </summary>
    [<Fact>]
    let ``Catalog.getSolutionRoot should return a valid directory containing the solution file`` () =
        // Act
        let rootPath = Catalog.getSolutionRoot()
        
        // Assert
        Assert.True(Directory.Exists(rootPath), sprintf "Solution root path does not exist: %s" rootPath)
        
        // Check for the presence of the .sln file to confirm we are at the root
        let slnFiles = Directory.GetFiles(rootPath, "TSCP_Framework.sln")
        Assert.NotEmpty(slnFiles)

    /// <summary>
    /// Tests the coverage calculation for the 37 systemic invariants.
    /// </summary>
    [<Fact>]
    let ``Logic.validateInvariants should return correct coverage ratio`` () =
        // Arrange: Providing 2 valid invariants out of 37
        let observations = [
            { Invariant = "INV_01_Mass_Conservation"; Value = 1.0 }
            { Invariant = "INV_02_Energy_Balance"; Value = 0.9 }
            { Invariant = "INVALID_CODE"; Value = 0.0 } // Should be ignored
        ]
        
        // Act
        let coverage = Logic.validateInvariants observations
        
        // Assert: 2 / 37 approx 0.054
        let expected = 2.0 / 37.0
        Assert.Equal(expected, coverage, 3)

    /// <summary>
    /// Ensures that the catalog root path targets the physical 'catalog' directory.
    /// </summary>
    [<Fact>]
    let ``Catalog.getCatalogRootPath should target the correct absolute folder`` () =
        // Act
        let catalogPath = Catalog.getCatalogRootPath()
        
        // Assert
        Assert.EndsWith("catalog", catalogPath)
        // Note: We don't necessarily assert existence here as the user might 
        // have a clean repo, but the path format must be correct.

    /// <summary>
    /// Validates the Phase Distance logic on the continuum.
    /// </summary>
    [<Theory>]
    [<InlineData(2.0, 2.0, "1.00 (Perfect Phase Cohesion)")>]
    [<InlineData(2.5, 2.0, "0.50 (Interstitial Phase Transition)")>]
    [<InlineData(3.0, 1.0, "2.00 (Systemic Phase Collision)")>]
    let ``Logic.compare should return correct descriptive gradient`` (lA: float, lB: float, expected: string) =
        // Act
        let result = Logic.compare lA lB
        
        // Assert
        Assert.Equal(expected, result)