```text
Current Status: Proposal - By: Minh-Tâm - Since: 2023-04-10
```

## 📋 Context and Problem Statement
> **What naming convention in URLs?**

Decision made:
`kebab-case`

* This is the best separator to tokenize/search text in URIs (example: for logs)
* Recommended by [Google](https://support.google.com/webmasters/answer/76329?hl=en)
* Recommended by `REST API Design Rulebook` (Oreilly) by Mark Masse
* [RFC 3986](https://www.rfc-editor.org/rfc/rfc3986) defines URLs as case-sensitive in several places. Since URLs are case sensitive, keeping all letters in lowercase is always safe and considered a good practice

## 💡 Considered options
* _Option 1: `PascalCase`_
* ✅ This is the default convention proposed by `dotnet`

* _Option 2: `camelCase`_
* ✅ This is widely used in query string keys (`https://myconfluence.atlassian.net/wiki/search?lastModified=today`)

* **_Option 3: `kebab-case`_**
* ✅ This is widely used in the `path` part of URIs (`https://stackoverflow.com/questions/10302179/hyphen-underscore-or-camelcase-as-word-delimiter-in-uris`)

* _Option 4: `snake_case`_
* ✅ It is widely used in the `path` part of URIs (`https://en.wikipedia.org/wiki/Cyrus_the_Great`)

