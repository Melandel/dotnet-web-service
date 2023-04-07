```text
Current Status: Approved - By: Minh-Tâm - Since: 2023-04-07
```

## 📋 Context and Problem Statement
> **How do we allow ADRs to evolve over time?**

By adding the following header:
```text
Current Status: <Proposal | Approved | Superseded | Rendered Obsolete> - By: <Someone(s) | ADR_id> - Since: <Some date>
```

&nbsp;

Examples:
```text
Current Status: Proposal - By: Alice, John - Since: 2023-04-07
```

```text
Current Status: Approved - By: Alice - Since: 2023-04-08
```

```text
Current Status: Superseded - By: 0042 - Since: 2023-04-09
```

```text
Current Status: Rendered Obsolete - By: 0111 - Since: 2023-04-09
```

## 💡 Considered Options
* **_Option 1: Use a header with status information (date and identity)_**
  * ✅ it does the job
  * ✅ it is simple
  * ✅ it benefits from git versioning if anyone wants to know the evolution over time of the status
