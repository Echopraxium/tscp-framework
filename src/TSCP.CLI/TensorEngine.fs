// Metadata UUID: f5e6-d7c8-core-tensor-v2
namespace TSCP.Core

module TensorEngine =

    let activate (concept: Concept) (signal: float) : Concept =
        let newScore = signal * (1.0 - concept.Metrics.Entropy)
        let updatedMetrics = { concept.Metrics with AntifragilityScore = newScore }
        { concept with Metrics = updatedMetrics }
    
    /// Calcule un score d'activation systémique
    /// Utilise les champs float (AntifragilityScore, Negentropy) définis dans le Domain
    let calculateActivation (metrics: SystemicMetrics) =
        // Calcul pondéré : 70% Antifragilité, 30% Negentropie
        (metrics.AntifragilityScore * 0.7) + (metrics.Negentropy * 0.3)

    /// Évalue l'écart (shift) entre deux états de métriques
    let evaluatePhaseShift (m1: SystemicMetrics) (m2: SystemicMetrics) =
        { m1 with 
            PhaseDistance = m1.PhaseDistance - m2.PhaseDistance
            Isotropy = (m1.Isotropy + m2.Isotropy) / 2.0 }

    /// Applique de nouvelles métriques à un Concept existant
    let updateConceptMetrics (concept: Concept) (newMetrics: SystemicMetrics) =
        { concept with Metrics = newMetrics }

    /// Génère un diagnostic rapide basé sur l'Entropie
    let getEntropyDiagnostic (metrics: SystemicMetrics) =
        if metrics.Entropy > 0.8 then "Instabilité critique détectée"
        elif metrics.Entropy < 0.2 then "Stabilité structurelle optimale"
        else "État homéostatique standard"