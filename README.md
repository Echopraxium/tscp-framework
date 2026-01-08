# 🟦 TSCP Framework (Core 4)

**Transdisciplinary System Construction Principles**
*Version 4.1.0-REBOOT | .NET 10 | F# Vector Engine*

> **"Comprendre, c'est calculer la trajectoire."**

TSCP est un **Moteur Systémique Vectoriel** conçu pour la modélisation haute-fidélité de systèmes complexes.

## 📐 Architecture : Le Continuum & Le Cube

Le framework opère une distinction stricte entre la réalité mathématique (le moteur) et sa représentation cognitive (l'interface).

### 1. Le Moteur (M3) : Continuum Vectoriel
Au niveau du noyau (`TSCP.Core`), il n'y a pas de limites discrètes. Tout système est un point dans un Espace de Hilbert à 4 dimensions, défini par une signature $\vec{V}$ normalisée $[0.0 - 1.0]$ :

* **$|S\rangle$ STRUCTURE** : Topologie, Conteneur, Interface.
* **$|I\rangle$ INFORMATION** : Sémantique, Code, Néguentropie.
* **$|D\rangle$ DYNAMIQUE** : Énergie, Flux, Transformation.
* **$|T\rangle$ TÉLÉONOMIE** : But, Attracteur, Fonction.

### 2. La Représentation (M2) : Le Cube Cognitif
Pour rendre cet espace 4D intelligible à l'opérateur humain, le framework projette le continuum sur une **Grille Heuristique de 64 Slots** (le "Cube 4x4x4").
* **Fonction** : C'est une "carte" simplifiée du territoire.
* **Usage** : Permet de classer intuitivement les patterns (ex: *Homéostasie*, *Résilience*) dans des cases familières, bien que leur signature réelle soit une valeur flottante précise (ex: $S=0.412, D=0.89$).

### 3. Stratification Fonctionnelle
* **M2 (Pattern Layer)** : Bibliothèque des archétypes systémiques (Le Tableau Périodique).
* **M1 (Narrative Layer)** : Kit de Navigation (Questions Structurantes & Épisodes).
* **M0 (Trajectory Layer)** : La réalité observée. Analyse des transitions d'états ($\Delta \vec{V}$).

---

## 🛠️ Stack Technique

Le projet est une solution hybride **.NET 10** :

* **`TSCP.Core` (F#)** : Algèbre tensorielle et types immutables.
* **`TSCP.CLI` (C#)** : Interface CLI (`analyse`, `load`, `sync`).
* **`TSCP.Doc2B64z` (C#)** : Transport sécurisé de "Matière Grise" (Base64 GZip).
* **Data (JSON-LD)** : Sérialisation standardisée des Ontologies.

## 🚀 Getting Started

### Prérequis
* .NET 10.0 SDK

### Installation & Build
1.  Cloner le dépôt :
    ```bash
    git clone [https://github.com/Echopraxium/tscp-framework.git](https://github.com/Echopraxium/tscp-framework.git)
    ```
2.  Lancer le script de reconstruction sécurisé :
    ```cmd
    _01_Rebuild_Solution.bat
    ```

### Utilisation (CLI)
Lancer le moteur en mode interactif :
```bash
dotnet run --project src/TSCP.CLI