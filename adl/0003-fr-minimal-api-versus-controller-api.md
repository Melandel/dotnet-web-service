```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Quel style d'API Web ASP.NET devrions-nous utiliser ? minimal API ou bien Controller API (alias API MVC) ?**

Nous utiliserons les Controller API pour des raisons de sécurité (ensemble de fonctionnalités testées et approuvées) :
```text
dotnet new webapi
```

## 💡 Options envisagées
* _Option 1 : utiliser l'API Web de style `Minimal` (`dotnet new`)_
  * ✅ Ca fonctionne
  * ✅ C'est simple
  * 🚫 L'ensemble des fonctionnalités disponibles est minuscule mais en croissance
* **_Option 2 : utiliser l'API Web de style `Controller` (alias style `MVC`)_**
  * ✅ Ca fonctionne
  * ✅ C'est simple
  * ✅ Elle possède de nombreuses fonctionnalités prêtes à l'emploi

## 📎 Informations supplémentaires
* [.NET 6 Minimal API vs. Web API 🚀 Complete CRUD with EF Core InMemory](reasons)
* [How to version minimal APIs in ASP.NET Core 6](https://www.infoworld.com/article/3671870/how-to-version-minimal-apis-in-aspnet-core-6.html)
