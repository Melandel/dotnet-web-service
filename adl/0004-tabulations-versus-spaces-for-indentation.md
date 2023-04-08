```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-04-07
```

## 📋 Context and Problem Statement
> **Should we use tabulations or spaces as indentation characters?**

Tabulations.
    
## 💡 Considered Options
* _Option 1: Use `spaces` for indentation_
    * ✅ Allows vertical alignment code style 
    ```cs
    var foo = MyMethod(param1,
                       param2,
                       param3);
    ```
    * 🚫 Prevents indentation length customization
* **_Option 2: Use `tabulations` for indentation_**
    * ✅ Allows indentation length customization
    * ✅ Accessibility
    * 🚫 Does not allow vertical alignment code style
    ```cs
    var foo = MyMethod(
    	param1,
    	param2,
    	param3);
    ```
