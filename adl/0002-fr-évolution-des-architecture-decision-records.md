```text
Statut courant: Approuvé - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Comment permettre aux ADR d'évoluer au fil du temps ?**

En ajoutant l'en-tête suivant :
```texte
État actuel : <Proposition | Approuvé | Remplacé | Rendu obsolète> - Par : <Quelqu'un(e) | ADR_id> - Depuis : <Une date>
```

&nbsp;

Exemples :
```texte
État actuel : Proposition - Par : Gauthier, MFO - Depuis : 2025-01-24
```

```texte
État actuel : Approuvé - Par : Mathieu - Depuis : 2025-01-25
```

```texte
État actuel : Remplacé - Par : [0003](www.google.com) - Depuis : 2025-01-26
```

```texte
État actuel : Rendu obsolète - Par : [0004](www.google.com) - Depuis : 2025-01-27
```

## 💡 Options envisagées
* **_Option 1 : Utiliser un en-tête avec des informations de statut (date et identité)_**
* ✅ ça fait le travail
* ✅ c'est simple
* ✅ ça bénéficie du versioning git si quelqu'un veut connaître l'évolution au fil du temps du statut de la décision
