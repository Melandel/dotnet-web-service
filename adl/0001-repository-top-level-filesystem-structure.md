```text
Current Status: Approved - By: Minh-Tâm - Since: 2023-04-07
```

## 📋 Context and Problem Statement
> **Which top-level filesystem structure should the repository follow?**

Decision taken:
```text
. (repo)
|-- adl/
|-- res/
`-- src/
```

## 💡 Considered Options
* _Option 1: put the source code directly at the root_
  * ✅ most simple option
  * 🚫🚫 folders such as `doc/`, `contributing/`, `cicd/` or `scripts/` that may come later will be part of the source code
* _Option 2: root folders: `src/`, `doc/` and `res/`_
  * ✅ open to future changes
  * ✅ `doc/` is a generic name and opens the possibility for adding `doc/how-to-guides`, `doc/key-topics`, and other structuring documentation layers
  * 🚫 but we already know we want these types of documents in a dedicated wiki, outside of the repository, because some of them might be relevant for `N` repositories at once
* **_Option 3: root folders: `src/`, `adl/` and `res/`_**
  * ✅ open to future changes
  * ✅ most simple option given we only want to document decisions made for now
  * ✅ `README` files in each folder should be enough for documenting acronyms such as src, res, [adl](https://github.com/joelparkerhenderson/architecture-decision-record#what-is-an-architecture-decision-record) or [cicd](https://en.wikipedia.org/wiki/CI/CD)
