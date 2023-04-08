```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Comment organiser le système de documentation ?**

Décision prise :
1. Une documentation complète dans l'espace Confluence [Informatique](https://cyrusproj.atlassian.net/wiki/spaces/Informatique/overview)
1. Un `SwaggerUI` dans les environnements non-PROD qui redirige vers cet espace Confluence

## 💡 Options envisagées
* _Option 1 : Mettre toute la documentation dans le repository_
  * 🚫 Cela crée un besoin de dupliquer la documentation qui est valable pour plusieurs repositories
* **_Option 2 : Mettre toute la documentation dans [Confluence](https://cyrusproj.atlassian.net/wiki/spaces/Informatique/overview) et fournir une redirection depuis SwaggerUI _**
  * ✅ Un seul endroit pour tous les sujets de documentation
