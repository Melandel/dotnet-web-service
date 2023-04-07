## 📋 Contexte et énoncé du problème
> **Quelle forme pour le premier niveau d'arborescence de fichiers ce repository doit-il suivre ?**

Décision prise :
```text
. (repo)
|-- adl/
|-- res/
`-- src/
```

## 💡 Options envisagées
* _Option 1 : mettre le code source directement à la racine_
  * ✅ option la plus simple
  * 🚫🚫 les dossiers tels que `doc/`, `contributing/`, `cicd/` ou `scripts/` qui pourraient venir plus tard feront partie du code source
* _Option 2 : dossiers racine : `src/`, `doc/` et `res/`_
  * ✅ ouvert aux changements futurs
  * ✅ `doc/` est un nom générique et ouvre la possibilité d'ajouter `doc/how-to-guides`, `doc/key-topics` et d'autres couches de documentation structurantes
  * 🚫 mais nous savons déjà que nous voulons ces types de documents dans un [Confluence dédié](https://cyrusproj.atlassian.net/wiki/spaces/Informatique/overview), en dehors du repository, car certains d'entre eux pourraient être pertinent pour les référentiels `N` à la fois
* **_Option 3 : dossiers racines : `src/`, `adl/` et `res/`_**
  * ✅ ouvert aux modifications futures
  * ✅ option la plus simple étant donné que nous souhaitons uniquement documenter les décisions prises pour l'instant
  * ✅ Les fichiers `README` dans chaque dossier devraient suffire à documenter des acronymes tels que src, res, [adl](https://github.com/joelparkerhenderson/architecture-decision-record#what-is-an-architecture-decision-record) ou [cicd](https://en.wikipedia.org/wiki/CI/CD)
