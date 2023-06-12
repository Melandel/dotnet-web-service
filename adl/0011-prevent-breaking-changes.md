```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-04-07
```

## 📋 Context and Problem Statement
> **How should we handle API contract breaking changes?**

By versioning our routes (`host/api/v1/route`, `host/api/v2/route`, etc). Our consumers should _never_ have OK calls that become KO a day later.

## 💡 Considered Options
* _Option 1: We don't._
    * ✅ Zero effort
    * 🚫🚫🚫 API consumers have no confidence in their calls to our API
* **_Option 2: Version our API routes._**
    * ✅ API consumers have confidence in their calls to our API
