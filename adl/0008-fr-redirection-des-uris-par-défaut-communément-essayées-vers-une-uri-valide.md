```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Quel doit être le comportement de notre API lorsqu'elle est interrogée sur une route par défaut couramment utilisée telle que `/`, `/index.html` ou `/home.html` ?**

Ces routes doivent rediriger vers SwaggerUI.

## 💡 Options envisagées
* _Option 1 : Renvoyer un statut http « 404 »_
  * ✅ Comportement par défaut (donc standardisé)
* _Option 2 : Renvoyer un statut http « 404 » avec un corps contenant une description d'erreur 404_
  * ✅ Commentaires plus utiles que l'option 1
* _Option 3 : Renvoyer un statut http « 404 » avec un lien vers la documentation_
  * ✅ Commentaires plus utiles que l'option 2
* **_Option 4 : Redirection vers la documentation_**
  * 🚫 Pas de véritable retour « point de terminaison non trouvé » - peut être trompeur
  * ✅ Moins d'étapes pour obtenir le même résultat que l'option 3
  * **✅ Les outils qui ping ces URL par défaut comprendront que l'API est opérationnelle**
