```text
Current Status: Waiting for approval - By: Minh-Tâm - Since: 2023-04-16
```

## 📋 Context and Problem Statement
> **Which structure should the fearless programming project follow?**

Decision taken:
```text
. (project folder)
|-- TestData/          # make test data reusable
|-- TestEnvironments/  # mutualizes `system-under-test` instantiation
`-- TestSuites/        # actual tests
```

## 💡 Considered Options
* _Option 1: only put tests_
  * ✅ most simple option
  * 🚫🚫 test data is duplicated a massive amount of times
  * 🚫🚫 adding a parameter to the constructor of the `system-under-test` implies an impact on _every_ test instantiating it
* _Option 2: put tests, and mutualize test data_
  * ✅ test data is reusable
  * 🚫🚫 adding a parameter to the constructor of the `system-under-test` implies an impact on _every_ test instantiating it
* **_Option 3: put tests, mutualize test data and mutualize `system-under-test` instantiation_**
  * ✅ test data is reusable
  * ✅ `system-under-test` instantiation is cheap to modify

