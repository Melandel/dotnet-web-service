```text
Statut courant: Proposition - Par: Minh-Tâm - Depuis: 2023-04-07
```

## 📋 Contexte et énoncé du problème
> **Comment devrions-nous gérer les modifications cassant le contrat d'API ?**

En versionnant nos routes (`host/api/v1/route`, `host/api/v2/route`, etc.). Nos consommateurs ne devraient _jamais_ avoir d'appels OK qui deviennent KO un jour plus tard.

## 💡 Options envisagées
* _Option 1 : Nous ne le faisons pas._
  * ✅ Aucun effort
  * 🚫🚫🚫 Les consommateurs d'API n'ont aucune confiance dans leurs appels à notre API
* **_Option 2 : Versionnez nos routes d'API._**
  * ✅ Les consommateurs d'API ont confiance dans leurs appels à notre API
