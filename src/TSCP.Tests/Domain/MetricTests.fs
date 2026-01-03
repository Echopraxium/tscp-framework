module TSCP.Tests.Domain.MetricTests

open System
open Xunit
open TSCP.Core
open TSCP.Core.Domain  // Important : Ouvre le module où est défini SystemicVector
open TSCP.Core.Engine  // Pour accéder au module Metrics

// CORRECTION ICI : On ajoute ": SystemicVector" pour aider le compilateur
let vec s i d t : SystemicVector = 
    { Structure = s; Information = i; Dynamics = d; Teleonomy = t }

[<Fact>]
let ``M3 Orthogonality : Structure and Dynamics should be orthogonal`` () =
    // L'hypothèse fondamentale : L'espace est statique (S) vs Le temps est mouvement (D).
    let s = vec 1.0 0.0 0.0 0.0
    let d = vec 0.0 0.0 1.0 0.0
    
    // On vérifie que le moteur calcule bien 0.0
    let similarity = Metrics.cosineSimilarity s d
    
    Assert.Equal(0.0, similarity)

[<Fact>]
let ``Isotope Validation : Active Learning is a Fractal of Phototropism`` () =
    // 1. Arrange : On récupère les concepts définis dans la mémoire (SeedData)
    let allConcepts = SeedData.getAllConcepts()
    
    // On cherche votre concept (Cognitif)
    let learning = 
        allConcepts 
        |> List.find (fun c -> c.Name.Contains("Active Learning"))
        
    // On cherche le concept biologique (Algue)
    let phototropism = 
        allConcepts 
        |> List.find (fun c -> c.Name.Contains("Phototropism"))

    // 2. Act : On demande au moteur de comparer leurs signatures vectorielles
    let similarity = Metrics.cosineSimilarity learning.Signature phototropism.Signature

    // 3. Assert : La similarité doit être très forte (> 95%)
    Assert.True(similarity > 0.95, sprintf "Expected similarity > 0.95, but was %f" similarity)
    
    // Vérification secondaire : Ils ne sont PAS identiques en magnitude
    let magDiff = abs(Metrics.magnitude learning.Signature - Metrics.magnitude phototropism.Signature)
    Assert.True(magDiff > 0.0, "Magnitudes should differ slightly (Reality gap)")

[<Fact>]
let ``Teleonomic Coherence : Homeostasis should be coherent`` () =
    // Teste si le calcul de cohérence (Alignement D -> T) fonctionne pour l'Homéostasie.
    let homeostasis = 
        SeedData.getAllConcepts() 
        |> List.find (fun c -> c.Id = "M2_HOMEOSTASIS")

    // Formule : Coherence = 1.0 - |Teleonomy - Dynamics|
    let expectedCoherence = 1.0 - abs(homeostasis.Signature.Teleonomy - homeostasis.Signature.Dynamics)

    Assert.Equal(0.9, expectedCoherence, 1)