```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-04-07
```

## 📋 Context and Problem Statement
Dotnet's native `enum` types are known for being recurring sources of problems:
* an unassigned `enum` type takes the `0` value even when that value is not defined in the enum, generating unnecessary risks of exceptions during deserialization
* `enum` types in web request bodies or queryStrings can be passed both as integers (`1`) and string (`foo`), generating unnecessary risks of inconsistency

> **Which constraints do we enforce on dotnet's `enum` types?**

Conventions through data validation and unit testing

## 💡 Considered options
* _Option 1: None_
* ✅ No effort
* ✅ Problematic behaviors and therefore maintainers are familiar with them
* 🚫 Avoiding problems depends on human discipline
* _Option 2: Use a library such as `Ardalis.SmartEnum`_
* ✅ No effort
* 🚫 Dependency on an external library
* 🚫 Works via `static` values which makes us lose a certain number of syntaxes requiring known code at compile time (pattern matching to name one)
* _Option 3: Use a home-made type derived from `Enumeration`_
* ✅ Uses mechanisms and types native to dotnet
* 🚫 Works via `static` values which makes us lose a certain number of syntaxes requiring known code at compile time (pattern matching to name one)
* **_Option 4: Enforce conventions through data validation and unit testing_**
* ✅ Uses mechanisms and types native to dotnet
* ✅ Allows the use of a certain number of syntaxes requiring known code at compile time (pattern matching to name one)
* 🚫 Data validation to implement by hand
