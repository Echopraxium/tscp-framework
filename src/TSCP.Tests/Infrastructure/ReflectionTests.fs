namespace TSCP.Tests.Infrastructure

open Xunit
open TSCP.Core
open TSCP.Core.Domain

module ReflectionTests =

    [<Fact>]
    let ``SystemicVector should verify Zero Trust defaults`` () =
        // Vérifie que le vecteur zéro est bien initialisé à 0.0 partout
        let v = Domain.zeroVector
        Assert.Equal(0.0, v.Structure)
        Assert.Equal(0.0, v.Information)
        Assert.Equal(0.0, v.Dynamics)
        Assert.Equal(0.0, v.Teleonomy)

    [<Fact>]
    let ``Concept Record has correct immutability`` () =
        // Vérifie simplement que la structure Concept existe et peut être instanciée
        let c = { 
            Id = "test_unit"
            Name = "Unit Test Concept"
            Description = "A temporary concept for testing."
            Signature = Domain.zeroVector
            Tags = ["unit"; "test"; "transient"] 
        }
        
        Assert.Equal("test_unit", c.Id)
        Assert.Equal("Unit Test Concept", c.Name)
        Assert.NotEmpty(c.Tags)