# ?? JustBee - Gestion G�ographique des Utilisateurs

![.NET 10](https://img.shields.io/badge/.NET-10-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET-Core-blue)
![Razor Pages](https://img.shields.io/badge/Razor-Pages-green)
![Leaflet](https://img.shields.io/badge/Leaflet-Maps-red)

## ?? Vue d'ensemble

JustBee est une application web moderne d�velopp�e avec ASP.NET Core 10 et Razor Pages, permettant de g�rer et visualiser g�ographiquement des utilisateurs r�partis dans les d�partements fran�ais. L'application offre une interface interactive avec cartographie en temps r�el utilisant Leaflet.

## ? Fonctionnalit�s Principales

### ??? **Carte Interactive (MapBee)**
- Visualisation cartographique de tous les d�partements fran�ais
- Marqueurs color�s selon la densit� d'utilisateurs (vert: 1, orange: 2, rouge: 3+)
- Popups informatifs avec d�tails des d�partements et listes des personnes
- Panneau lat�ral avec statistiques en temps r�el
- Zoom automatique sur les zones d'activit�

### ?? **Gestion des Personnes**
- Interface compl�te pour ajouter, supprimer et g�rer les utilisateurs
- Assignation automatique aux d�partements avec g�olocalisation
- G�n�ration de donn�es de test al�atoires
- Recherche et filtrage en temps r�el
- Statistiques par d�partement et r�gion

### ?? **API REST Compl�te**
- **GET** `/api/departements` - Tous les d�partements
- **GET** `/api/departements/{code}` - D�partement sp�cifique
- **GET** `/api/departements/with-persons` - D�partements avec utilisateurs
- **POST** `/api/departements/{code}/persons` - Ajouter une personne
- **DELETE** `/api/departements/{code}/persons/{id}` - Supprimer une personne
- **GET** `/api/departements/persons` - Toutes les personnes

### ?? **Interface de Test API**
- Page de d�monstration interactive des APIs
- Formulaires de test en temps r�el
- Visualisation JSON des r�ponses
- Gestion des erreurs et validation

## ??? Architecture

### Mod�les de Donn�es

```csharp
// Personne avec g�olocalisation automatique
public class Person
{
    public int Id { get; set; }
    public string Pseudo { get; set; }
    public string? DepartementCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

// D�partement fran�ais avec coordonn�es GPS
public class Departement
{
    public string Code { get; set; }        // Ex: "75", "69", "13"
    public string Nom { get; set; }         // Ex: "Paris", "Rh�ne"
    public string Region { get; set; }      // Ex: "�le-de-France"
    public double Latitude { get; set; }    // Coordonn�e GPS
    public double Longitude { get; set; }   // Coordonn�e GPS
    public List<Person> Persons { get; set; }
}
```

### Services

- **`DepartementService`** : Service principal avec tous les 101 d�partements fran�ais
- Gestion en m�moire pour des performances optimales
- Auto-g�n�ration des IDs et assignation g�ographique

## ?? D�marrage Rapide

### Pr�requis
- .NET 10 SDK
- Navigateur web moderne

### Installation

```bash
# Cloner le repository
git clone https://github.com/Mafyou/JustBee.git
cd JustBee

# Restaurer les d�pendances
dotnet restore src/

# Lancer l'application
dotnet run --project src/
```

### Acc�s aux Fonctionnalit�s

| Page | URL | Description |
|------|-----|-------------|
| ??? **Carte Interactive** | `/MapBee` | Visualisation cartographique des utilisateurs |
| ?? **Gestion Personnes** | `/PersonManagement` | Interface compl�te de gestion |
| ?? **Test API** | `/ApiDemo` | D�monstration et test des APIs |
| ?? **Accueil** | `/` | Page d'accueil du projet |

## ?? Exemples d'Utilisation

### Ajouter des Personnes via Code

```csharp
var service = new DepartementService();

// Ajouter une personne � Paris (g�olocalisation automatique)
service.AddPersonToDepartement("75", new Person 
{ 
    Pseudo = "Alice_Paris" 
});

// Ajouter plusieurs personnes � Lyon
service.AddPersonToDepartement("69", new Person { Pseudo = "Bob_Lyon" });
service.AddPersonToDepartement("69", new Person { Pseudo = "Charlie_Lyon" });

// R�cup�rer les statistiques
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

// R�cup�rer tous les d�partements avec utilisateurs
const departementsAvecPersonnes = await fetch('/api/departements/with-persons')
    .then(response => response.json());
```

## ?? Interface Utilisateur

### Caract�ristiques de l'Interface
- **Design Responsive** : Compatible mobile et desktop
- **Bootstrap 5** : Interface moderne et intuitive
- **Font Awesome** : Ic�nes professionnelles
- **Animations** : Transitions fluides et feedback visuel
- **Recherche Temps R�el** : Filtrage instantan� des donn�es

### Carte Interactive
- **Leaflet.js** : Cartographie professionnelle
- **OpenStreetMap** : Cartes d�taill�es et gratuites
- **Marqueurs Personnalis�s** : Couleurs selon la densit�
- **Popups Informatifs** : D�tails complets des d�partements
- **L�gende Dynamique** : Explication des codes couleur

## ?? Donn�es Int�gr�es

### D�partements Fran�ais Complets
- **101 D�partements** : France m�tropolitaine + DOM-TOM
- **Coordonn�es GPS Pr�cises** : Centro�des g�ographiques
- **Informations Compl�tes** : Codes, noms, r�gions
- **Donn�es Officielles** : Conformes aux codes INSEE

### Exemples de D�partements
| Code | Nom | R�gion | Coordonn�es |
|------|-----|--------|-------------|
| 75 | Paris | �le-de-France | 48.9, 2.3 |
| 69 | Rh�ne | Auvergne-Rh�ne-Alpes | 45.7, 4.8 |
| 13 | Bouches-du-Rh�ne | Provence-Alpes-C�te d'Azur | 43.5, 5.4 |

## ??? Technologies Utilis�es

### Backend
- **ASP.NET Core 10** : Framework web moderne
- **Razor Pages** : Architecture MVC simplifi�e
- **API REST** : Services web RESTful
- **C# 13** : Langage de programmation moderne

### Frontend
- **HTML5 / CSS3** : Structure et style modernes
- **JavaScript ES6+** : Interactions c�t� client
- **Bootstrap 5** : Framework CSS responsive
- **Leaflet.js** : Biblioth�que de cartographie
- **Font Awesome** : Ic�nes vectorielles

### Donn�es
- **In-Memory Storage** : Stockage en m�moire pour les d�monstrations
- **Auto-Generated IDs** : G�n�ration automatique d'identifiants
- **JSON Serialization** : S�rialisation pour les APIs

## ?? Cas d'Usage

### Applications Professionnelles
- **Gestion de Clients** : R�partition g�ographique des clients
- **Logistique** : Optimisation des tourn�es de livraison
- **Marketing** : Analyse de la pr�sence territoriale
- **RH** : Localisation des employ�s et �quipes

### Fonctionnalit�s de D�monstration
- **Proof of Concept** : D�monstration de faisabilit�
- **Prototypage Rapide** : Base pour d�veloppements futurs
- **Formation** : Exemple p�dagogique complet
- **Tests d'Interface** : Validation UX/UI

## ?? Extensibilit�

### D�veloppements Futurs Possibles
- **Base de Donn�es** : Migration vers SQL Server / PostgreSQL
- **Authentification** : Syst�me de connexion utilisateur
- **Temps R�el** : Notifications WebSocket
- **Export/Import** : Fonctionnalit�s de sauvegarde
- **Filtres Avanc�s** : Recherche multicrit�res
- **Analytics** : Tableaux de bord et statistiques

### Architecture Modulaire
- **Services D�coupl�s** : Facilit� d'extension
- **API-First** : R�utilisabilit� des services
- **Responsive Design** : Adaptabilit� mobile
- **Clean Code** : Maintenabilit� optimale

## ?? Performance

### Optimisations Int�gr�es
- **Donn�es en M�moire** : Acc�s ultra-rapide
- **Lazy Loading** : Chargement diff�r� des cartes
- **Caching Client** : R�duction des requ�tes r�seau
- **Compression** : Optimisation des transferts

### M�triques Estim�es
- **Temps de R�ponse** : < 100ms pour les APIs
- **Chargement Initial** : < 2s pour la carte compl�te
- **Capacit�** : Support de milliers d'utilisateurs
- **M�moire** : Empreinte minimale optimis�e

## ?? Contribution

### Environnement de D�veloppement
```bash
# Configuration recommand�e
- Visual Studio 2024 ou VS Code
- .NET 10 SDK
- Extension C# pour VS Code
- Git pour le versioning
```

### Structure du Projet
```
src/
??? ?? Models/              # Mod�les de donn�es
??? ?? Services/            # Logique m�tier
??? ?? Controllers/         # APIs REST
??? ?? Pages/              # Pages Razor
??? ?? Examples/           # Exemples d'utilisation
??? ?? Program.cs          # Point d'entr�e
```

## ?? Licence

Ce projet est d�velopp� comme d�monstration technique et �ducative.

## ?? Liens Utiles

- **Repository GitHub** : [JustBee](https://github.com/Mafyou/JustBee)
- **Documentation .NET** : [Microsoft Learn](https://learn.microsoft.com/aspnet/core)
- **Leaflet.js** : [Documentation officielle](https://leafletjs.com/)
- **Bootstrap** : [Documentation CSS](https://getbootstrap.com/)

---

**D�velopp� avec ?? et .NET 10** | **Cartographie par ??? Leaflet** | **Interface par ?? Bootstrap**