```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Context and Problem Statement
> **Quelle convention de nommage dans les URLs?**

Décision prise:
`kebab-case`

* C'est le meilleur séparateur pour tokeniser/rechercher du texte dans les URI (exemple: pour les logs)
* Recommandé par [Google](https://support.google.com/webmasters/answer/76329?hl=en)
* Recommandé par `REST API Design Rulebook` (Oreilly) de Mark Masse
* La [RFC 3986](https://www.rfc-editor.org/rfc/rfc3986) définit les URLs comme sensibles à la casse à plusieurs endroits. Puisque les URLs sont sensibles à la casse, garder tout en lettres minuscules is toujours sûr et considéré comme une bonne pratique

## 💡 Options envisagées
* _Option 1: `PascalCase`_
  * ✅ C'est la convention par défaut proposée par `dotnet`

* _Option 2: `camelCase`_
  * ✅ C'est largement utilisé dans les clés des query strings (`https://cyrusproj.atlassian.net/wiki/search?lastModified=today`)

* **_Option 3: `kebab-case`_**
  * ✅ C'est largement utilisé dans la partie `chemin` des URIs (`https://stackoverflow.com/questions/10302179/hyphen-underscore-or-camelcase-as-word-delimiter-in-uris`)

* _Option 4: `snake_case`_
  * ✅ C'est largement utilisé dans la partie `chemin` des URIs (`https://en.wikipedia.org/wiki/Cyrus_the_Great`)
