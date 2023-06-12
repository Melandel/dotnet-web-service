```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-04-07
```

## 📋 Context and Problem Statement
> **What should our api behave when queried on a commonly used default route such as `/`, `/index.html` or `/home.html`?**

These routes should redirect to SwaggerUI.

## 💡 Considered Options
* _Option 1: Return a `404` http status_
    * ✅ Default behavior (hence standardized)
* _Option 2: Return a `404` http status with a body containing a 404 error description_
    * ✅ More helpful feedback than Option1
* _Option 3: Return a `404` http status with a link to the documentation_
    * ✅ More helpful feedback than Option2
* **_Option 4: Redirect to the documentation_**
    * 🚫 No real "endpoint not found" feedback - can be misleading
    * ✅ Fewer steps to get the same outcome as Option3
    * **✅ Tools that ping these default urls will understand the `api` is up and running**
