```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Devrions-nous utiliser des tabulations ou des espaces comme caractères d'indentation ?**

Tabulations.

## 💡 Options envisagées
* _Option 1 : Utiliser des `espaces` pour l'indentation_
    * ✅ Autorise le style de code d'alignement vertical
    ```cs
    var foo = MyMethod(param1,
                       param2,
                       param3);
    ```
    * 🚫 Empêche la personnalisation de la longueur d'indentation
* **_Option 2 : utiliser `tabulations` pour l'indentation_**
    * ✅ Permet la personnalisation de la longueur d'indentation
    * ✅ Accessibilité
    * 🚫 N'autorise pas le style de code d'alignement vertical
    ```cs
    var foo = MyMethod(
    	param1,
    	param2,
    	param3);
    ```
