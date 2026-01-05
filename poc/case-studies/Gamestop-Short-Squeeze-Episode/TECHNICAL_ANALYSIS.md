# 🛠 TSCP Technical Analysis: GameStop Event

Ce document détaille l'extraction technique des "Épisodes Révélateurs" depuis le graphe M0. Nous utilisons SPARQL pour isoler les sous-structures logiques et GraphViz (DOT) pour les visualiser.

---

## 🟢 Étape 1 : Le Dysfonctionnement (The Dysfunction)
**Analyse TSCP :** Identification d'une `Contrainte` dépassant la capacité du système, créant une `Dysfonction`.

### 1.1 Requête SPARQL (`step1_dysfunction.sparql`)
```sparql
PREFIX m2: [https://github.com/Echopraxium/tscp-framework/ontology/m2#](https://github.com/Echopraxium/tscp-framework/ontology/m2#)
PREFIX inst: [https://github.com/Echopraxium/tscp-framework/instances/gamestop#](https://github.com/Echopraxium/tscp-framework/instances/gamestop#)
PREFIX rdfs: [http://www.w3.org/2000/01/rdf-schema#](http://www.w3.org/2000/01/rdf-schema#)

CONSTRUCT {
  ?agent m2:induces ?constraint .
  ?constraint m2:Dysfunction ?error .
  ?constraint rdfs:label ?cLabel .
  ?error rdfs:label ?eLabel .
}
WHERE {
  ?agent m2:induces ?constraint .
  ?constraint m2:Dysfunction ?error .
  ?constraint rdfs:label ?cLabel .
  ?error rdfs:label ?eLabel .
  FILTER(?agent = inst:MelvinCapital)
}