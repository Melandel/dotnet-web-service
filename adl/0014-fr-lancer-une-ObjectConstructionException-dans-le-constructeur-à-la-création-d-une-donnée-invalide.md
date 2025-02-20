```text
Statut courant : Proposition - Par : Minh-Tâm - Depuis : 2023-04-07
```

## 📋 Contexte et problématique
> **Comment exprimer qu'une donnée invalide est vide de sens ?**

Lancer une `ObjectConstructionException` dans le constructeur de toute donnée invalide

## 💡 Options envisagées
* _Option 1 : Ne pas chercher à exprimer cette idée._
  * ✅ Aucun effort
  * 🚫 Les inconsistences peuvent être détectées très tardivement et corrompre la donnée existante
  * 🚫🚫 Coût d'investigation élevé si le comportement réel est inattendu
* _Option 2 : Appliquer des clauses de garde au début de chaque méthode._
  * ✅ L'impact des erreurs est atténué
  * 🚫 Certaines vérifications peuvent être implémentées plusieurs fois
* **_Option 3 : Vérifier les contraintes lors de l'instanciation de tout objet (à l'aide d'exceptions)._**
  * ✅ L'impact des erreurs est atténué
  * ✅✅ Les vérifications sont mutualisées
  * ✅✅ Impossible qu'une instance d'objet invalide existe
  * ✅✅ La réutilisation des objets existants est fortement recommandée (par héritage ou bien composition)
  * 🚫 Les exceptions interrompent l'exécution et sont coûteuses en termes de performances
  * ✅ Ce qui est manifesté est le non-sens de la donnée vide, et non une stratégie pour empêcher ce scénario de s'exécuter, chose plus simple (pas d'enjeu de choix de retour applicatif en cas de donnée invalide à ce stade)
* _Option 4 : Vérifier les contraintes lors de l'instanciation de tout objet (à l'aide du pattern `Result<Value, Error>`)._
  * ✅ L'impact des erreurs est atténué
  * ✅✅ Les vérifications sont mutualisées
  * ✅✅ Impossible qu'une instance d'objet invalide existe
  * ✅✅ La réutilisation des objets existants est fortement recommandée
  * ✅ Les erreurs n'interrompent pas l'exécution
  * 🚫🚫 L'utilisation du modèle `Result<Value, Error>` présente un risque important d'incohérence, voire d'utilisation systématique (ce qui peut s'avérer coûteux pour l'écriture et la lecture).
  * 🚫 Ce qui est manifesté est une stratégie pour se protéger de la construction de donnée invalide, chose plus sophistiquée que d'exprimer que la donnée invalide est vide de sens (enjeux de granularité et d'utilité du retour applicatif en cas de donnée invalide)

