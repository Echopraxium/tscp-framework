# TSCP Framework - Technical Reference Specification
**UUID:** 9a8b7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d
**Version:** 2.0.0-CORE4
**Status:** Active / Vector-Based
**Date:** 2026-01-03

## 1. Executive Summary
The TSCP (Traceable Systemic Continuum Platform) Framework is a **Vector-Based Systemic Engine** designed for high-fidelity modeling of complex systems. It treats systemic entities not as static objects, but as **Concepts** positioned within a 4-dimensional state space ($M3$). The framework distinguishes between the **Analytical Space** (The "Why"/Meaning) and the **Constructive Space** (The "How"/Mechanism), allowing for precise computation of systemic health, entropy, and coherence.

## 2. Core Architectural Pillars

### 2.1 The M3 Vector Space (Core 4)
The "37 Invariants" have been superseded by **4 Fundamental Orthogonal Vectors**. Every concept in the system possesses a `SystemicVector` signature $[S, I, D, T]$ normalized between $0.0$ and $1.0$.

| Symbol | Invariant | Analytic Space (Meaning) | Constructive Space (Mechanism) |
| :---: | :--- | :--- | :--- |
| **$|S\rangle$** | **STRUCTURE** | **Frontière Conceptuelle**<br>*(Identity / Scope)* | **Membrane / Interface**<br>*(Topology / Container)* |
| **$|I\rangle$** | **INFORMATION** | **Sémantique**<br>*(Ontologie / Sens)* | **Code / État**<br>*(Encodage / ADN)* |
| **$|D\rangle$** | **DYNAMIQUE** | **Transformation Anticipée**<br>*(Loi d'Évolution)* | **Flux / Énergie**<br>*(Transformation Observée)* |
| **$|T\rangle$** | **TÉLÉONOMIE** | **Attracteur**<br>*(Stabilité / Cible)* | **Tropisme**<br>*(Gradient / Adaptation)* |

### 2.2 Systemic Metrics & Algebra
The engine performs vector algebra on active concepts to derive systemic health indicators:
* **Average Vector ($\vec{V}_{avg}$):** The "Center of Gravity" of the active system.
* **System Entropy ($E$):** Measures the lack of order or definition.
    * $E \approx 1.0 - \text{mean}(I, S)$
* **Teleonomic Coherence ($C$):** Measures the alignment between the system's movement and its goal.
    * $C = 1.0 - |T - D|$ (High coherence means Dynamics serve the Teleonomy).

### 2.3 The Pattern Layer (M2)
The "Old Invariants" (e.g., Homeostasis, Feedback, Hierarchy) are no longer axioms but **Composite Patterns** stored in the M2 layer.
* They are defined in `SeedData.fs`.
* They are instantiated as Concepts with specific vector signatures (e.g., Homeostasis $\rightarrow$ High $T$ + High $D$).

## 3. Component Map

| Component | Responsibility | Key File |
| :--- | :--- | :--- |
| **TSCP.Core** | M3 Vector Engine & Domain Types | `Domain.fs`, `Engine.fs` |
| **TSCP.Core** | M2 Pattern Library (Seed) | `SeedData.fs` |
| **TSCP.Core** | Persistence & IO | `Catalog.fs` |
| **TSCP.Core** | Session State Management | `Session.fs` |
| **TSCP.CLI** | C# Command Line Interface | `Program.cs` |
| **TSCP.Tests** | Mathematical Validation & Traceability | `ReflectionTests.fs` |

## 4. Command Reference Table

| Command | Argument | Description |
| :--- | :--- | :--- |
| `catalog` / `ls` | - | Lists available concepts (M2/M1) with their **[S\|I\|D\|T]** signature. |
| `load` | `<id>` | Loads a concept into the active context (Session History). |
| `inspect` | `<id>` | Displays full details: Description, Vector, and Tags. |
| `analyse` | - | Computes **Entropy** and **Coherence** of the currently active concepts. |
| `history` | - | Shows the session log and active concept stack. |
| `sync` | - | Simulates a continuum shift (Layer + 0.5). |
| `reset` | - | Clears the session and reloads M3 defaults. |
| `pwd` | - | Displays the catalog root path. |

## 5. Traceability Contract
To ensure the link between the **Compiled Code** (Constructive) and the **Systemic Model** (Analytic), strict reflection rules apply:
* **Metadata:** All Domain types must be decorated with `[<Uuid("...")>]`.
* **Verification:** `TSCP.Tests` enforces that no core type exists without a UUID.
* **Null Safety:** Interop between C# CLI and F# Core must handle `null` via `Option.ofObj` / `Concept?`.