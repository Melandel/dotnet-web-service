```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-05-12
```

## 📋 Context and Problem Statement
> **How do we write access modifiers for types and type members?**

By taking advantage of default access modifiers:
* Types (`class`, `struct`, `record`) are encapsulated at the assembly level by default (`internal`)
* Types members (`methods`, `properties`, `fields`) are encapsulated at the type level by default (`private`)
* Only a few `public` and `internal` keywords should be written in the files - and occasionally some `private`

## 💡 Considered Options
* _Option 1: by expliciting every access modifiers (private, internal, public)_
  * ✅ It is explicit
  * 🚫 Not taking advantage of default access modifiers
* **_Option 2: by taking advantage of default access modifiers_**
  * 🚫 It requires knowledge of the default access modifiers
  * ✅ Source code is less verbose
  * ✅✅ Creates a mindset where normality is encapsulation and only transparent access is specified
