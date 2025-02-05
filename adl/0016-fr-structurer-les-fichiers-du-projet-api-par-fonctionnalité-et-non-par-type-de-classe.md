```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Comment structurer les fichiers de classes du projet `api` en dossier?**

En accordant un dossier par fonctionnalité:
* `Concerns.ErrorHandling`
* `Durabilité`
* `ENGEL`
* etc.

## 💡 Options envisagées
* _Option 1: Un dossier par type de classe (`Controllers`, `DTOs`, `Middlewares`, etc.)_
  * ✅ Le dossier `Controllers` est la convention par défaut dans `ASP.NET`
  * ✅ Le premier niveau d'arborescence de fichiers exprime l'architecture technique
  * 🚫 Le premier niveau d'arborescence de fichiers n'exprime pas les fonctionnalités gérées par l'API
  * 🚫 Créer un nouvel endpoint nécessite des copiers/collers dans plusieurs dossiers
  * 🚫 Les dépendances entre classes sont éparpillées dans plusieurs dossiers (plus de `usings`)
  * ✅ L'inventaire des classes par type (exemple: `Middlewares`) est facilement lisible
* **_Option 2: Un dossier par fonctionnalité (`Concerns.ErrorHandling`, `Durabilité`, `ENGEL`, etc.)_**
  * 🚫 Convention différente de la convention par défaut `ASP.NET`
  * ✅ Le premier niveau d'arborescence de fichiers exprime les fonctionnalités gérées par l'API
  * ✅ Créer un nouvel endpoint nécessite le copier/coller unique d'un dossier
  * ✅ Les dépendances entre classes sont regroupées dans un seul dossier (moins de `usings`)
