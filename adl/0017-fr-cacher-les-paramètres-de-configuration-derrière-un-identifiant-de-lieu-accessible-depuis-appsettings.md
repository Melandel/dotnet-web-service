```text
Current Status: Proposal - By: Minh-Tâm - Since: 2025-08-16
```

## 📋 Background and problem statement
> **Comment les paramètres de configuration devraient-ils être récupérés?**

Dans un emplacement dont l'accès est paramétré dans l'appsettings.json

## 💡 Options considered
* _Option 1: Dans les fichiers appsettings.json_
  * ✅ Solution minimaliste
  * ✅ Solution clé en main
  * 🚫 Les valeurs peuvent être écrasées par les variables d'environnement sur la machine déployée
* _Option 2: Dans les variables d'environnement de la machine déployée_
  * ✅ Solution clé en main
  * 🚫 Oblige les mainteneurs à examiner deux sources de configuration : appsettings.json _et_ variables d'environnement
* _Option 3: Dans un système externe tel qu'une base de données ou bien une API_
  * ✅ Permet des stratégies de mutualisation d'éléments de configuration
  * ✅ Permet une politique d'accès à la configuration plus sophistiquée
* _Option 4: Dans un mix de fichiers appsettings.json, variables d'environnement, et systèmes externes_
  * ✅ Permet une stratégie de mutualisation d'éléments de configuration
  * 🚫 Compliqué
* **_Option 5: Dans un emplacement dont l'accès est paramétré dans l'appsettings.json_**
  * ✅ Simplifie l'appsettings.json _et_ les variables d'environnement
  * ✅ Permet une politique d'accès à la configuration plus sophistiquée et plus contrôlée
  * ✅ Permet de personnaliser facilement la stratégie de récupération de configuration, applicable à l'entièreté d'une organisation
  * 🚫 Complexe
