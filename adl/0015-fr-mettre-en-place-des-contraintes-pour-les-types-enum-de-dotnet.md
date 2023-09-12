```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
Les `enum` natives de `dotnet` sont connues pour être sources de problèmes récurrents:
* une `enum` non assignée prend la valeur `0` même si cette valeur n'est pas définie, entrainant des problèmes de désérialisation
* une `enum` en paramètre de requête web peut toujours être passée ou bien en tant que chiffre (`1`), ou bien en tant que string (`Toto`), entrainant des risques d'inconsistence

> **Quelles contraintes sur les types `enum` de dotnet?**

En enforçant des conventions par de la validation de donnée et des tests unitaires

## 💡 Options envisagées
* _Option 1: Aucune_
  * ✅ Pas d'effort
  * ✅ Comportements problématiques récurrents et donc connus
  * 🚫 L'évitement des problèmes récurrents dépend de la discipline humaine:w
* _Option 2: Utiliser une bibliothèque telle que `Ardalis.SmartEnum`_
  * ✅ Pas d'effort
  * 🚫 Dépendance à une bibliothèque externe
  * 🚫 Fonctionne via des valeurs `static` et l'on perd un certain nombre de syntaxes nécessitant du code connu à la compilation (pattern matching pour en citer une)
* _Option 3: Utiliser un type maison dérivant d'`Enumeration`_
  * ✅ Utilise des mécanismes et des types natifs à dotnet
  * 🚫 Fonctionne via des valeurs `static` et l'on perd un certain nombre de syntaxes nécessitant du code connu à la compilation (pattern matching pour en citer une)
* **_Option 4: Enforcer des conventions par de la validation de donnée et des tests unitaires_**
  * ✅ Utilise des mécanismes et des types natifs à dotnet
  * ✅ Permet d'utiliser un certain nombre de syntaxes nécessitant du code connu à la compilation (pattern matching pour en citer une)
  * 🚫 Validation de donnée à implémenter
