# TSCP Framework - Technical Reference Specification
**UUID:** 77a1e2f3-b5c6-4d7e-8f9a-0b1c2d3e4f5a
**Version:** 1.0.0-PROD
**Status:** Stable / Validated
**Date:** 2025-12-31

## 1. Executive Summary
The TSCP (Traceable Systemic Continuum Platform) Framework is a .NET 10 based system designed for high-fidelity systemic modeling. Unlike traditional discrete modeling approaches, TSCP treats systemic layers as a **continuum**, allowing for interstitial states between major abstraction levels (M0 to M3).

## 2. Core Architectural Pillars

### 2.1 The Systemic Continuum
The framework replaces integer-based layers with a **float-based indexing system**. This allows concepts to exist in transition phases between discrete systemic levels.
- **Discrete Reference Layers:** M0 (0.0), M1 (1.0), M2 (2.0), M3 (3.0).
- **Interstitial States:** Any value L in R (e.g., 2.5 represents a concept halfway between M2 and M3).

### 2.2 The 37 Systemic Invariants
The M3 layer transition is governed by **37 mandatory invariants**. These invariants represent the structural and functional requirements for a system to achieve M3 systemic consistency.
- **Validation Engine:** The `Logic.validateInvariants` function calculates the coverage ratio by checking observations against the internal `SystemicInvariants` registry.
- **Metric Logic:** Coverage = (Count of Satisfied Invariants) / 37.

### 2.3 Phase Distance (Dphi)
The alignment between two points on the continuum is measured through the Phase Distance calculation:
**Dphi = |Layer_A - Layer_B|**
- **Cohesion (Dphi = 0):** Perfect phase alignment.
- **Transition (0 < Dphi < 1):** Interstitial systemic evolution.
- **Collision (Dphi >= 1):** Radical phase disruption or layer mismatch.

## 3. Component Map

| Component | Responsibility | Key File |
| :--- | :--- | :--- |
| **TSCP.Core** | Domain Models & Systemic Logic | `Logic.fs`, `Domain.fs` |
| **TSCP.Session** | Persistence & State Continuum | `State.fs` |
| **TSCP.CLI** | Command Dispatching & Visualization | `CommandHandler.fs` |
| **TSCP.GUI** | Visual Simulation & Event Loop | `GuiMain.fs` |
| **TSCP.Tests** | Traceability & Non-regression | `ReflectionTests.fs` |

## 4. Command Reference Table

| Command | Argument | Description |
| :--- | :--- | :--- |
| `catalog` | - | Lists available M0 JSONLD models found in the catalog directory. |
| `load` | `<id>` | Loads a model and calculates its M3 Invariant coverage. |
| `list` | - | Displays session history with float layer precision. |
| `analyse` | - | Computes M3 metrics (Antifragility, Entropy, Isotropy). |
| `compare` | - | Calculates Dphi relative to the previous layer. |
| `layer++` | `[step]` | Advances the continuum (default step: 1.0). |
| `graph` | - | Outputs DOT source for visualization in the console. |
| `export` | `<file>` | Saves the systemic graph as a .dot file to the /exports folder. |
| `pwd` | - | Displays the absolute path of the exports directory. |
| `reset` | - | Deletes the session file and resets the continuum to 0.0. |

## 5. Traceability Contract
All major types and modules must be decorated with the `UuidAttribute` for automated reflection-based traceability. This contract is enforced by the `TSCP.Tests.Infrastructure` suite.