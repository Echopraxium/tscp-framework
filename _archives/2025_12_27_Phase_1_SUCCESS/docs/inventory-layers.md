# TSCP Framework - Upper Layers Inventory (M3-M1)

Ce document répertorie les objets fondamentaux nécessaires au fonctionnement du moteur tensoriel et de la fonction `grow`.

---

## 🏛️ Layer M3 : Invariants (Universal Laws)
*Les règles physiques et logiques qui s'appliquent à tous les systèmes.*

| Object ID | Name | Description | Default Orthogonality |
| :--- | :--- | :--- | :--- |
| `tscp:M3/Entropy` | Loi d'Entropie | Tendance naturelle à la dégradation de l'information. | 1.0 |
| `tscp:M3/Symmetry` | Symétrie de Phase | Conservation des propriétés lors d'une rotation d'état. | 0.95 |
| `tscp:M3/Antifragility` | Antifragilité | Capacité d'un système à croître sous le stress. | 0.98 |
| `tscp:M3/Orthogonality` | Orthogonalité | Mesure de l'indépendance sémantique entre deux objets. | 1.0 |
| `tscp:M3/FeedbackLoop` | Boucle de Rétroaction | Mécanisme de régulation (Positive ou Négative). | 0.90 |

---

## 👁️ Layer M2 : Statespace & Observer (Context)
*Les cadres de résolution définis par l'expertise humaine.*

| Object ID | Name | Role | Precision Level |
| :--- | :--- | :--- | :--- |
| `tscp:M2/Resolution` | Résolution | Définit la finesse du grain (G0, G1, G2). | Dynamic |
| `tscp:M2/Intention` | Vecteur d'Intention | La "Question Structurante" (Requête SPARQL). | High |
| `tscp:M2/PhaseShift` | Déphasage | Écart entre l'observation et l'état réel. | 0.0 - 1.0 |
| `tscp:M2/Boundary` | Frontière | Limite du "Sandbox" ou du système analysé. | Rigid |

---

## 🚌 Layer M1 : Standards & Interfaces (The Bus)
*Composants réutilisables et protocoles de transport.*

| Object ID | Name | Type | Application Example |
| :--- | :--- | :--- | :--- |
| `tscp:M1/Standard_API` | Interface Standard | Interface | Communication inter-systèmes. |
| `tscp:M1/Semantic_Bus` | Bus Sémantique | Protocol | Transport de la connaissance (ex: Git). |
| `tscp:M1/Isotope` | Isotope | Interface | Pont entre deux réalités (ex: PLEX, ISK). |
| `tscp:M1/Rule_Engine` | Moteur de Règles | Component | Grammaire de MTG ou Unix. |
| `tscp:M1/Consensus` | Protocole Consensus | Protocol | Gouvernance DAO ou Mycélium. |

---

## 📏 Seuils de Tolérance d'Orthogonalité
Pour garantir la propreté du système, toute "Promotion" d'un objet vers une couche supérieure doit respecter ces scores de similarité cosinus :

1. **Promotion M0 -> M1** : Doit être orthogonal à **> 0.85** par rapport aux autres standards.
2. **Promotion M1 -> M3** : Doit être orthogonal à **> 0.98** (unicité quasi-totale de la loi).