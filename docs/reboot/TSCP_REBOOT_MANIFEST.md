# 🚀 TSCP REBOOT MANIFEST (v4.0)

**UUID:** `c0d3-r3b00t-v4-m4n1f3st`
**Date:** 2026-01-07
**Statut:** ✅ INITIALIZED
**Phase:** Core 4 (Vector Engine) + Narrative M1

---

## 1. VISION STRATÉGIQUE
Le projet redémarre sur une architecture stricte "Zero Trust" basée sur l'Algèbre Tensorielle.
* **Axiome :** Tout concept système est un vecteur $\vec{V}$ dans l'Espace de Hilbert $\mathcal{H}_{M3}$.
* **Signature :** $\vec{V} = [S, I, D, T]$ (Structure, Information, Dynamique, Téléonomie).
* **Objectif :** Construire un "Navigateur Systémique" capable de calculer la trajectoire narrative d'un système ($\Delta \vec{V}$).

---

## 2. CORPUS DOCUMENTAIRE VALIDÉ (Source de Vérité)

### A. Références Théoriques & Architecture
| Fichier | Rôle | Statut |
| :--- | :--- | :--- |
| **`TSCP_Architecture_Ref.md`** | **Spec Technique.** Définit les 4 vecteurs et l'abandon des 37 invariants. | 🔒 LOCKED |
| **`SYSTEMIC_ARCHITECT_COMPANION_GUIDE.md`** | **Guide Mathématique.** Manuel de référence pour l'Algèbre Tensorielle et la Théorie des Catégories. | 🔒 LOCKED |
| **`TSCP_Context_Recovery.md`** | **Roadmap.** Historique du pivot vers le Core 4 et état des lieux F#. | 📝 ACTIVE |

### B. Ontologies (Data)
Ces fichiers définissent les primitives chargées par le moteur au démarrage.

| Fichier | Contenu |
| :--- | :--- |
| **`TSCP_M3_Ontology_Core.jsonld`** | Les 4 Invariants de base ($|S\rangle, |I\rangle, |D\rangle, |T\rangle$). |
| **`TSCP_M2_Ontology_Core.jsonld`** | Le Tableau Périodique (Concepts vectoriels : System, Agent, Energy...). |
| **`TSCP_M1_Ontology_Core.jsonld`** | Le Kit Narratif (StructuringQuestion, NarrativeStep, SourceReference). |

---

## 3. OUTILLAGE (Toolchain)

### A. I/O & Sécurité
* **Outil :** `TSCP.Doc2B64z` (v4.6)
* **Fonction :** Compression/Décompression sécurisée (Base64 GZip).
* **Règle :** Liste blanche stricte (Docs & Data uniquement).
* **Commande :** `_00_run_Doc2B64z.bat` (Remplace l'ancien `DocAsTokens`).

### B. Moteur (Runtime F#)
* **Langage :** F# (.NET 10)
* **Modules Clés :**
    * `Domain.fs` : Types vectoriels (`SystemicVector`) et UUIDs.
    * `Engine.fs` : Calcul de l'Entropie ($E$) et de la Cohérence ($C$).
    * `SeedData.fs` : (À venir) Implémentation des Patterns M2 composites.

---

## 4. FEUILLE DE ROUTE IMMÉDIATE (Next Steps)

1.  **M2 Pattern Seeding :**
    * Coder les signatures vectorielles complexes dans `SeedData.fs` (ex: Homéostasie = Fort $T$ + Fort $D$).
    * Source : Dérivé de `TSCP_M2_Ontology_Core.jsonld`.

2.  **Narrative Engine (Delta Logic) :**
    * Implémenter le calcul de transition $\vec{V}_{t+1} = f(\vec{V}_t, \text{Event})$.
    * Permettre la comparaison entre la Trajectoire Observée (M0) et l'Attracteur Théorique (M2).

3.  **Validation :**
    * Exécuter `TSCP.Tests` pour vérifier l'orthogonalité des nouveaux vecteurs M3.