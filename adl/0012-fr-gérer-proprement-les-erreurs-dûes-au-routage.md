```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Comment gérer un routage qui échoue ?**

Avec :
* 405 MethodNotAllowed lorsque l'URL est gérée, mais que le verbe HTTP est incorrect
* 404 NotFound lorsque l'URL n'est pas gérée

## 💡 Options envisagées
* _Option 1 : Aucun effort._
  * ✅ Aucun effort
  * 🚫🚫🚫 Les consommateurs d'API n'ont aucune confiance dans l'utilité de notre API
* **_Option 2 : utilisez 405 lorsque seul le verbe est incorrect et 404 lorsque le routage n'a pas été résolu._**
  * ✅ Les consommateurs d'API ont confiance dans les commentaires de notre API
