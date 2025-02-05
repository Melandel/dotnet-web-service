```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-04-07
```

## 📋 Background and problem statement
> **Which folders should we use to structure the `api` project files?**

One folder per feature:
* `Concerns.ErrorHandling`
* `FeatureA`
* `FeatureB`
* etc.

## 💡 Options considered
* _Option 1: One folder per class type (`Controllers`, `DTOs`, `Middlewares`, etc.)_
  * ✅ The `Controllers` folder is the default convention in `ASP.NET`
  * ✅ The first level of file tree expresses the technical architecture
  * 🚫 The first level of the file tree does not express the features managed by the API
  * 🚫 Creating a new endpoint requires copy/pasting into multiple folders
  * 🚫 Dependencies between classes are scattered in several folders (lots of/long `usings` statements)
  * ✅ The inventory of classes by type (example: `Middlewares`) is easily readable
* **_Option 2: One folder per feature (`Concerns.ErrorHandling`, `Durability`, `ENGEL`, etc.)_**
  * 🚫 Convention different from the default `ASP.NET` convention
  * ✅ The first level of the file tree expresses the features managed by the API
  * ✅ Creating a new endpoint requires a simple copy/paste of a folder
  * ✅ Dependencies between classes are grouped in a single folder (fewer/shorter `usings` statements)
