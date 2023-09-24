```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Quelle structure le projet FearlessProgramming doit-il suivre ?**

Décision prise :
```texte
. (dossier du projet)
|-- TestData/ # réutilisabilité des données de test
|-- TestEnvironments/ # mutualisation de l'instanciation des `system-under-test`
`-- TestSuites/ # les tests en eux-mêmes
```

## 💡 Options envisagées
* _Option 1 : mettre uniquement des tests_
  * ✅ option la plus simple
  * 🚫🚫 les données de test sont dupliquées un nombre massif de fois
  * 🚫🚫 ajouter un paramètre au constructeur de `system-under-test` implique un impact sur _chaque_ test l'instanciant
* _Option 2 : mettre des tests et mutualiser les données de test_
  * ✅ les données de test sont réutilisables
  * 🚫🚫 ajouter un paramètre au constructeur de `system-under-test` implique un impact sur _chaque_ test l'instanciant
* **_Option 3 : mettre des tests, mutualiser les données de test et mutualiser l'instanciation des `system-under-test`_**
  * ✅ les données de test sont réutilisables
  * ✅ l'instanciation du système sous test est peu coûteuse à modifier
