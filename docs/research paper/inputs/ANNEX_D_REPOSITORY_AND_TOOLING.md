# Annex C: Repository, Tooling, and Implementation Logic

## 1. Technological Stack & Functional Heritage

The TSCP framework's **initial prototype was developed in F#**, using a functional-first approach to manage systemic complexity. The architecture is designed for high modularity and mathematical precision:

* **F# & Avalonia:** The GUI prototype (`TSCP.GUI`) leverages **Avalonia UI**, allowing for a cross-platform, high-performance interface while maintaining a purely functional state management.
* **Logic Factorization:**
    * **Core Logic:** The business rules and systemic transformations are factored into the F# kernel to ensure category-theory consistency (Morphisms and Functors).
    * **Auth & Security:** Handled by a dedicated module (`Auth.fs`), ensuring secure access to modeling sessions.
    * **GUI Engine:** The `GuiMain.fs` orchestrates the visual representation of the double-pyramid, linking the F# backend logic to the user interface.

## 2. GitHub Repository Structure

The project is hosted at: `https://github.com/Echopraxium/tscp-framework`

```text
/ontology
  ├── TSCP_M3_Ontology_Core.jsonld       (Hilbert Space Axioms)
  ├── TSCP_M2_Ontology_Core.jsonld       (Periodic Table of Invariants)
  ├── TSCP_M1_Ontology_Core.jsonld       (Narrative & Connectors)
  └── /extensions
      └── TSCP_M1_Ext_Finance.jsonld     (Market Extensions)
/src
  ├── TSCP.GUI (F# / Avalonia)           (The Visualizer Prototype)
  │   ├── Auth.fs                        (Authentication Module)
  │   ├── GuiMain.fs                     (Main UI Logic)
  │   └── TSCP.GUI.fsproj                (Project Definition)
  ├── TSCP.Core (F#)                     (Mathematical Kernel)
  ├── TSCP.Server (C# / F#)              (SPARQL Storage)
  └── TSCP.Tests                         (Integrity & Cubic Limit Tests)
/web
  └── index.html                         (Layered D3.js Visualizer)