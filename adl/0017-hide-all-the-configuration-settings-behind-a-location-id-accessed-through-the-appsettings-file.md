```text
Current Status: Proposal - By: Minh-Tâm - Since: 2025-08-16
```

## 📋 Background and problem statement
> **How should the configuration settings be fetched?**

In a location whose access is parameterized in appsettings.json

## 💡 Options considered
* _Option 1: In the appsettings.json files_
  * ✅ Minimalist solution
  * ✅ Out-of-the-box solution
  * 🚫 Values can be overwritten by environment variables on the deployed machine
* _Option 2: In the environment variables of the deployed machine_
  * ✅ Out-of-the-box solution
  * 🚫 Forces maintainers to look at two sources of configuration: appsettings.json _and_ environment variables
* _Option 3: In an external system like a database or an api_
  * ✅ Allows for configuration settings mutualization strategy
  * ✅ Allows for a more sophisticated configuration access policy
  * 🚫 Creates a dependency and the risks that go with it
* _Option 4: In a mix of appsettings.json files, environment variables, and external systems_
  * ✅ Allows for configuration settings mutualization strategy
  * 🚫 Complicated
* **_Option 5: In a location whose access is parameterized in appsettings.json_**
  * ✅ Simplifies appsettings.json _and_ environment variables
  * ✅ Allows for a more sophisticated and more monitored configuration access policy
  * ✅ Allows for easily changeable, organization-wide custom configuration fetching strategy
  * 🚫 Complex
