```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-05-21
```

## 📋 Context and Problem Statement
> **How should we detect errors?**

Verify constraints when instantiating any object (using exceptions)

## 💡 Considered Options
* _Option 1: Zero effort._
    * ✅ Zero effort
    * 🚫 Errors may be detected very late/never
    * 🚫🚫 High investigation cost when the actual behavior is not expected
* _Option 2: Apply guard clauses at the beginning of each method._
    * ✅ Impact caused by errors is mitigated to some extent
    * 🚫 Some verifications may be implemented several times
* **_Option 3: Verify constraints when instantiating any object (using exceptions)._**
    * ✅ Impact caused by errors is mitigated to some extent
    * ✅✅ Verifications are mutualized
    * ✅✅ Impossible for an invalid object instance to exist
    * ✅✅ Reusing existing objects is highly encouraged
    * 🚫 Exceptions disrupt the execution path and are expensive in terms of performance
* _Option 4: Verify constraints when instantiating any object (using Result<Value, Error> pattern)._
    * ✅ Impact caused by errors is mitigated to some extent
    * ✅✅ Verifications are mutualized
    * ✅✅ Impossible for an invalid object instance to exist
    * ✅✅ Reusing existing objects is highly encouraged
    * ✅ Errors do not disrupt the execution path
    * 🚫🚫 There is a big risk of either inconsistency in the use of the `Result<Value, Error>` pattern, either it will be used everywhere (can be expensive for both the writer & the reader)
