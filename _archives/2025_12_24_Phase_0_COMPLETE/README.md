# TSCP-Framework

This repository contains the software implementation and research papers derived from the **TSCP (Transdisciplinary Systems Construction Principles)** framework. 

The core of this project is to provide a method for constructing and analyzing complex systems, capable of mathematically quantifying the semantic incommensurability between different observers.

## 📁 Project Structure

* **`/TSCP.CLI`**: Source code of the Proof of Concept (PoC) tool.
    * `Domain.fs`: Core Model.
	* `Compute.fs`: Calculation engine (Semantic Tensors, Jaccard Index, Phase Distance $D_{\phi}$).
    * `Persistence.fs`: Persistence Utility module.
	* `Export.fs`: Import/Export management (JSON-LD, N-Triples).
    * `Program.fs`: Command Line Interface (CLI).
* **`/Docs`**: Research papers, methodological notes, and technical fact sheets.
* **`/Models`**: Ontology models in `model.jsonld` format.

## 🚀 The TSCP-CLI Tool

**TSCP-CLI** (*Transdisciplinary Systems Construction Principles CLI*) is an inference engine developed in **F#**. It was designed to operationalize TSCP principles using **Category Theory** to ensure the consistency of information transfer between system strata.

### Key Features:
* **Collision Analysis**: Calculates the Phase Distance ($D_{\phi}$) between two entities.
* **Pivot Synchronization**: Measures the ability of a heterogeneous swarm to coordinate on critical facets.
* **Semantic Web Interoperability**: Full export to RDF (N-Triples) for auditing via SPARQL queries.

## 🧠 TSCP Methodology

The framework organizes knowledge into four layers:
1.  **M3 (Principles)**: Ontological invariants.
2.  **M2 (Archetypes)**: Observation filters.
3.  **M1 (Structures)**: Operational rules and flows.
4.  **M0 (Instances)**: Agents and field data.

The framework's power lies in its ability to mathematically identify **phase ruptures** (e.g., $D_{\phi} = 1.0$) when two systems use the same data but mutually exclusive M2 filters (Case study: *GameStop - Swarm vs. Classic Hedge Fund*).

## 🤝 Human-Machine Collaboration

This project is the result of a symbiotic research collaboration between:
* **The Human Researcher**: Bearer of 20 years of research on systemic construction principles, guarantor of theoretical intuition and epistemological validation.
* **The Artificial Intelligence (Gemini)**: Thought Partner responsible for software prototyping, semantic modeling, and tensorial data analysis.

### Current Iteration Metrics (v1.7):
* **Context Volume Processed**: ~28,000 tokens.
* **Architecture**: Validated by tensorial calculation and SPARQL audit.

## ⚖️ License and Critique

This framework is a **proposal submitted for critique**. Tools are provided to enable scientific counter-expertise, modification of the M3-M1 layers, and transdisciplinary experimentation (AI, Geopolitics, Finance).