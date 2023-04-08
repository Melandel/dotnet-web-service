```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-04-07
```

## 📋 Context and Problem Statement
> **Which ASP.NET Web API style should we use? Minimal API or Controller API (aka MVC API)?**

We'll use `Controller APIs` for safety (tried-and-tested set of features):
```text
dotnet new webapi
```

## 💡 Considered Options
* _Option 1: Use `Minimal`-style Web API (`dotnet new `)_
    * ✅ It works
    * ✅ It is simple
    * 🚫 Set of available features is tiny but growing
* **_Option 2: Use `Controller`-style (aka `MVC`-style) Web API_**
    * ✅ It works
    * ✅ It is simple
    * ✅ It has a lot of out-of-the-box features

## 📎 Additional Information
* [.NET 6 Minimal API vs. Web API 🚀 Complete CRUD with EF Core InMemory](reasons)
* [How to version minimal APIs in ASP.NET Core 6](https://www.infoworld.com/article/3671870/how-to-version-minimal-apis-in-aspnet-core-6.html)
