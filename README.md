# 🐝 JustBee - Gestion Géographique des Utilisateurs

![.NET 10](https://img.shields.io/badge/.NET-10-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET-Core-blue)
![Razor Pages](https://img.shields.io/badge/Razor-Pages-green)
![Leaflet](https://img.shields.io/badge/Leaflet-Maps-red)

## 📋 Vue d'ensemble

JustBee est une application web moderne développée avec ASP.NET Core 10 et Razor Pages, permettant de gérer et visualiser géographiquement des utilisateurs répartis dans les départements français. L'application offre une interface interactive avec cartographie en temps réel utilisant Leaflet.

## ✨ Fonctionnalités Principales

### 🗺️ **Carte Interactive (MapBee)**
- Visualisation cartographique de tous les départements français
- Marqueurs colorés selon la densité d'utilisateurs (vert: 1, orange: 2, rouge: 3+)
- Popups informatifs avec détails des départements et listes des personnes
- Panneau latéral avec statistiques en temps réel
- Zoom automatique sur les zones d'activité

### 👥 **Gestion des Personnes**
- Interface complète pour ajouter, supprimer et gérer les utilisateurs
- Assignation automatique aux départements avec géolocalisation
- Génération de données de test aléatoires
- Recherche et filtrage en temps réel
- Statistiques par département et région

### 🔧 **API REST Complète**
- **GET** `/api/departements` - Tous les départements
- **GET** `/api/departements/{code}` - Département spécifique
- **GET** `/api/departements/with-persons` - Départements avec utilisateurs
- **POST** `/api/departements/{code}/persons` - Ajouter une personne
- **DELETE** `/api/departements/{code}/persons/{id}` - Supprimer une personne
- **GET** `/api/departements/persons` - Toutes les personnes

### 🧪 **Interface de Test API**
- Page de démonstration interactive des APIs
- Formulaires de test en temps réel
- Visualisation JSON des réponses
- Gestion des erreurs et validation

## 🏗️ Architecture

### Modèles de Données

```csharp
// Personne avec géolocalisation automatique
public class Person
{
    public int Id { get; set; }
    public string Pseudo { get; set; }
    public string? DepartementCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

// Département français avec coordonnées GPS
public class Departement
{
    public string Code { get; set; }        // Ex: "75", "69", "13"
    public string Nom { get; set; }         // Ex: "Paris", "Rhône"
    public string Region { get; set; }      // Ex: "Île-de-France"
    public double Latitude { get; set; }    // Coordonnée GPS
    public double Longitude { get; set; }   // Coordonnée GPS
    public List<Person> Persons { get; set; }
}
```

### Services

- **`DepartementService`** : Service principal avec tous les 101 départements français
- Gestion en mémoire pour des performances optimales
- Auto-génération des IDs et assignation géographique

## 🚀 Démarrage Rapide

### Prérequis
- .NET 10 SDK
- Navigateur web moderne

### Installation

```bash
# Cloner le repository
git clone https://github.com/Mafyou/JustBee.git
cd JustBee

# Restaurer les dépendances
dotnet restore src/

# Lancer l'application
dotnet run --project src/
```

### Accès aux Fonctionnalités

| Page | URL | Description |
|------|-----|-------------|
| 🗺️ **Carte Interactive** | `/MapBee` | Visualisation cartographique des utilisateurs |
| 👥 **Gestion Personnes** | `/PersonManagement` | Interface complète de gestion |
| 🔧 **Test API** | `/ApiDemo` | Démonstration et test des APIs |
| 🏠 **Accueil** | `/` | Page d'accueil du projet |

## 💡 Exemples d'Utilisation

### Ajouter des Personnes via Code

```csharp
var service = new DepartementService();

// Ajouter une personne à Paris (géolocalisation automatique)
service.AddPersonToDepartement("75", new Person 
{ 
    Pseudo = "Alice_Paris" 
});

// Ajouter plusieurs personnes à Lyon
service.AddPersonToDepartement("69", new Person { Pseudo = "Bob_Lyon" });
service.AddPersonToDepartement("69", new Person { Pseudo = "Charlie_Lyon" });

// Récupérer les statistiques
var departementsActifs = service.GetAllDepartements()
    .Where(d => d.Persons.Any())
    .Count();
```

### Utilisation de l'API REST

```javascript
// Ajouter une personne via API
const response = await fetch('/api/departements/75/persons', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ pseudo: 'NouvelUtilisateur' })
});

// Récupérer tous les départements avec utilisateurs
const departementsAvecPersonnes = await fetch('/api/departements/with-persons')
    .then(response => response.json());
```

## 🎨 Interface Utilisateur

### Caractéristiques de l'Interface
- **Design Responsive** : Compatible mobile et desktop
- **Bootstrap 5** : Interface moderne et intuitive
- **Font Awesome** : Icônes professionnelles
- **Animations** : Transitions fluides et feedback visuel
- **Recherche Temps Réel** : Filtrage instantané des données

### Carte Interactive
- **Leaflet.js** : Cartographie professionnelle
- **OpenStreetMap** : Cartes détaillées et gratuites
- **Marqueurs Personnalisés** : Couleurs selon la densité
- **Popups Informatifs** : Détails complets des départements
- **Légende Dynamique** : Explication des codes couleur

## 📊 Données Intégrées

### Départements Français Complets
- **101 Départements** : France métropolitaine + DOM-TOM
- **Coordonnées GPS Précises** : Centroïdes géographiques
- **Informations Complètes** : Codes, noms, régions
- **Données Officielles** : Conformes aux codes INSEE

### Exemples de Départements
| Code | Nom | Région | Coordonnées |
|------|-----|--------|-------------|
| 75 | Paris | Île-de-France | 48.9, 2.3 |
| 69 | Rhône | Auvergne-Rhône-Alpes | 45.7, 4.8 |
| 13 | Bouches-du-Rhône | Provence-Alpes-Côte d'Azur | 43.5, 5.4 |

## 🛠️ Technologies Utilisées

### Backend
- **ASP.NET Core 10** : Framework web moderne
- **Razor Pages** : Architecture MVC simplifiée
- **API REST** : Services web RESTful
- **C# 13** : Langage de programmation moderne

### Frontend
- **HTML5 / CSS3** : Structure et style modernes
- **JavaScript ES6+** : Interactions côté client
- **Bootstrap 5** : Framework CSS responsive
- **Leaflet.js** : Bibliothèque de cartographie
- **Font Awesome** : Icônes vectorielles

### Données
- **In-Memory Storage** : Stockage en mémoire pour les démonstrations
- **Auto-Generated IDs** : Génération automatique d'identifiants
- **JSON Serialization** : Sérialisation pour les APIs

## 🎯 Cas d'Usage

### Applications Professionnelles
- **Gestion de Clients** : Répartition géographique des clients
- **Logistique** : Optimisation des tournées de livraison
- **Marketing** : Analyse de la présence territoriale
- **RH** : Localisation des employés et équipes

### Fonctionnalités de Démonstration
- **Proof of Concept** : Démonstration de faisabilité
- **Prototypage Rapide** : Base pour développements futurs
- **Formation** : Exemple pédagogique complet
- **Tests d'Interface** : Validation UX/UI

## 🔧 Extensibilité

### Développements Futurs Possibles
- **Base de Données** : Migration vers SQL Server / PostgreSQL
- **Authentification** : Système de connexion utilisateur
- **Temps Réel** : Notifications WebSocket
- **Export/Import** : Fonctionnalités de sauvegarde
- **Filtres Avancés** : Recherche multicritères
- **Analytics** : Tableaux de bord et statistiques

### Architecture Modulaire
- **Services Découplés** : Facilité d'extension
- **API-First** : Réutilisabilité des services
- **Responsive Design** : Adaptabilité mobile
- **Clean Code** : Maintenabilité optimale

## 📈 Performance

### Optimisations Intégrées
- **Données en Mémoire** : Accès ultra-rapide
- **Lazy Loading** : Chargement différé des cartes
- **Caching Client** : Réduction des requêtes réseau
- **Compression** : Optimisation des transferts

### Métriques Estimées
- **Temps de Réponse** : < 100ms pour les APIs
- **Chargement Initial** : < 2s pour la carte complète
- **Capacité** : Support de milliers d'utilisateurs
- **Mémoire** : Empreinte minimale optimisée

## 🤝 Contribution

### Environnement de Développement
```bash
# Configuration recommandée
- Visual Studio 2024 ou VS Code
- .NET 10 SDK
- Extension C# pour VS Code
- Git pour le versioning
```

### Structure du Projet
```
src/
├── 📁 Models/              # Modèles de données
├── 📁 Services/            # Logique métier
├── 📁 Controllers/         # APIs REST
├── 📁 Pages/              # Pages Razor
├── 📁 Examples/           # Exemples d'utilisation
└── 📄 Program.cs          # Point d'entrée
```

## 📝 Licence

Ce projet est développé comme démonstration technique et éducative.

## 🔗 Liens Utiles

- **Repository GitHub** : [JustBee](https://github.com/Mafyou/JustBee)
- **Documentation .NET** : [Microsoft Learn](https://learn.microsoft.com/aspnet/core)
- **Leaflet.js** : [Documentation officielle](https://leafletjs.com/)
- **Bootstrap** : [Documentation CSS](https://getbootstrap.com/)

---

**Développé avec ❤️ et .NET 10** | **Cartographie par 🗺️ Leaflet** | **Interface par 🎨 Bootstrap**