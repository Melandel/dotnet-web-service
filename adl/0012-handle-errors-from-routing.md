```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-04-07
```

## 📋 Context and Problem Statement
> **How should we handle failed routing?**

With:
* 405 MethodNotAllowed when the url is handled, but the HTTP verb is incorrect
* 404 NotFound when the url is not handled

## 💡 Considered Options
* _Option 1: Zero effort._
    * ✅ Zero effort
    * 🚫🚫🚫 API consumers have no confidence in the helpfulness of our API
* **_Option 2: Use 405 when only the verb is incorrect, and 404 when routing did not resolve._**
    * ✅ API consumers have confidence in our API's feedback
