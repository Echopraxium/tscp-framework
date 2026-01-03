# TSCP_Context_Recovery.md

## Contexte du Projet : Framework TSCP (The Systemic Construction Platform)

Je continue le développement du framework systémique **TSCP**.
Nous venons de valider une refonte majeure du noyau (**Core 4**) et l'architecture technique sous-jacente.

### 1. Architecture M3 (Les 4 Invariants Fondamentaux)
Nous avons remplacé les 37 invariants historiques par 4 vecteurs fondamentaux orthogonaux :

* **$|S\rangle$ STRUCTURE** (Métrique : 0.0 à 1.0)
    * *Analytique* : Frontière Conceptuelle / Identité.
    * *Constructif* : Membrane, Interface, Topologie.
* **$|I\rangle$ INFORMATION** (Métrique : 0.0 à 1.0)
    * *Analytique* : Sémantique, Ontologie.
    * *Constructif* : Code, ADN, État mémorisé.
* **$|D\rangle$ DYNAMIQUE** (Métrique : 0.0 à 1.0)
    * *Analytique* : Transformation Anticipée (Loi d'évolution).
    * *Constructif* : Flux, Énergie, Transformation Observée.
* **$|T\rangle$ TÉLÉONOMIE** (Métrique : 0.0 à 1.0)
    * *Analytique* : Attracteur, État Stable.
    * *Constructif* : Tropisme, Mécanisme d'adaptation (C'est ce qui remplace la Téléologie).

### 2. État Technique du Code (C# / F#)

Le projet est une solution .NET 9/10 hybride.

#### A. TSCP.Core (F#)
* **Domain.fs** : Définit `SystemicVector` (record de 4 floats) et `Concept`.
    * Utilise un attribut personnalisé `[<Uuid("...")>]` pour la traçabilité (Réflexion) au lieu de commentaires.
* **Engine.fs** : Calcule les métriques (Entropie, Cohérence) basées sur la moyenne vectorielle des concepts actifs.
* **SeedData.fs** : Contient les définitions initiales (M3 Invariants).
* **Tests** (`ReflectionTests.fs`) :
    * Valident l'orthogonalité des 4 vecteurs.
    * Utilisent `Option.ofObj` pour gérer proprement les retours `null` de la Réflexion .NET (pour éviter l'erreur FS0043).

#### B. TSCP.CLI (C#)
* **Program.cs** : Interface en ligne de commande.
    * Affiche les vecteurs sous la forme `[S:1.0 I:0.5 D:0.0 T:0.0]`.
    * Gère la nullabilité avec `Concept?` pour éviter les warnings CS8603.
    * Commandes actives : `catalog`, `load <id>`, `analyse`.

### 3. Dernière action réalisée
Nous avons réussi le **"Deep Rebuild"** (nettoyage des bin/obj) et tous les tests (Core + Reflection) passent au vert. Le moteur vectoriel est fonctionnel.

### 4. Objectif de cette session (Next Step)
Nous devons maintenant passer à l'étape du **"Rangement M2" (Pattern Layer)**.
L'objectif est de peupler le fichier `SeedData.fs` en traduisant les anciens concepts systémiques (ex: Homéostasie, Feedback, Hiérarchie) en **signatures vectorielles composées** (mix de S, I, D, T).

---
*Peux-tu m'aider à définir et coder les premiers patterns M2 dans `SeedData.fs` ?*