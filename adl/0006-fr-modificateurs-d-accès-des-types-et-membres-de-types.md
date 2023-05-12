```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Comment écrire des modificateurs d'accès pour les types et les membres de type ?**

En tirant parti des modificateurs d'accès par défaut :
* Les types (`class`, `struct`, `record`) sont encapsulés au niveau de l'_assembly_ par défaut (`internal`)
* Les membres des types (`methods`, `properties`, `fields`) sont encapsulés au niveau du _type_ par défaut (`private`)
* Seuls quelques mots-clés `public` et `internal` doivent être écrits dans les fichiers - et occasionnellement quelques mots-clés `private`

## 💡 Options envisagées
* _Option 1 : en explicitant tous les modificateurs d'accès (private, internal, public)_
  * ✅ C'est explicite
  * 🚫 Ne prend pas avantage des modificateurs d'accès par défaut
* **_Option 2 : en tirant parti des modificateurs d'accès par défaut_**
  * 🚫 Ca nécessite la connaissance des modificateurs d'accès par défaut
  * ✅ Le code source est moins verbeux
  * ✅✅ Crée un état d'esprit où la normalité est l'encapsulation et seul l'accès publique devient nécessaire à spécifier
