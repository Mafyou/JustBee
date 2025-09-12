# 🐝 JustBee - Plan B Démocratie Participative

![.NET 9](https://img.shields.io/badge/.NET-9-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET-Core-blue)
![Razor Pages](https://img.shields.io/badge/Razor-Pages-green)
![Leaflet](https://img.shields.io/badge/Leaflet-Maps-red)
![Democracy](https://img.shields.io/badge/Democracy-Participative-yellow)

## 📋 Vue d'ensemble

**JustBee - Plan B Démocratie Participative** est une application web développée avec ASP.NET Core 9 et Razor Pages, conçue pour mettre en œuvre une innovation démocratique expérimentale. Cette plateforme permet de gérer et visualiser géographiquement les **alvéoles citoyennes** réparties dans les territoires français, favorisant une **réappropriation citoyenne bottom-up** dans le cadre constitutionnel français.

L'application s'inspire de la structure des ruches avec ses **alvéoles modulaires** et intègre des **sentinelles écologiques** (grenouilles) comme indicateurs de la santé démocratique territoriale.

## 🎯 Contexte du Plan B

### Origine et Mission
Le Plan B représente une **réponse pragmatique à une crise de défiance institutionnelle**. Il propose un système d'**alvéoles citoyennes** organisées comme des conseils consultatifs représentatifs avec une approche écologique symbolique.

### Fondements Juridiques
- **Articles L2143-1 à L2143-4 du CGCT** : Conseils consultatifs
- **Article L1112-17 du CGCT** : Participation des habitants  
- **Chartes de démocratie participative** régionales
- **Loi 1901** : Organisation associative
- **Conformité totale** : Extension consultative légale des outils existants

## ✨ Fonctionnalités Principales

### 🏛️ **Page d'Accueil - Plan B Présentation**
- **Hero section** avec thématique alvéoles et présentation du plan
- **Section sentinelles écologiques** avec les 4 grenouilles symboliques :
  - 🐸 **Gardienne des Traditions** (`frog_big.png`) - Sagesse ancestrale
  - 🐸 **Médiatrice Citoyenne** (`frog_peace.png`) - Dialogue pacifique  
  - 🐸 **Visionnaire du Futur** (`frog_wings.png`) - Innovation démocratique
  - 🐸 **Guide du Changement** (`frog_wings_front.png`) - Leadership de transition
- **Présentation détaillée** : Contexte, finalités, fondements juridiques
- **Structure de la Ruche** : Organisation en alvéoles thématiques
- **Timeline de mise en place** : Du lancement immédiat à l'extension

### 🗺️ **MapBee - Cartographie des Alvéoles**
- **Carte interactive** des départements/villes français avec visualisation des citoyens
- **Statistiques en temps réel** avec indicateurs grenouilles
- **Marqueurs couleur** selon la densité citoyenne
- **Popups thématiques** avec informations des alvéoles territoriales
- **Panneau de bord** avec métriques de couverture territoriale
- **MapVille** : Vue détaillée au niveau communal

### 👥 **Gestion des Citoyens (PersonManagement)**
- **Interface complète** pour administrer les ambassadeurs démocratiques
- **Terminologie adaptée** : "Citoyens", "Alvéoles territoriales", "Ruche"
- **Statistiques visuelles** avec les 4 grenouilles indicatrices
- **Génération automatique** de citoyens pour tester le système
- **Recherche temps réel** et gestion par alvéoles
- **Design cohérent** avec hero section et thème alvéoles/grenouilles

### 🛠️ **Création d'Alvéoles (CreerAlveole)**
- **Formulaire de création** de nouvelles alvéoles territoriales
- **Validation par email** avec service Brevo
- **Vérification d'alvéoles** existantes
- **Interface utilisateur intuitive** avec thématique alvéoles

### 🔒 **Politique de Confidentialité**
- **Protection RGPD** complète avec thématique grenouilles
- **4 principes** représentés par les sentinelles écologiques
- **Droits citoyens** détaillés dans le contexte démocratique
- **Contact DPO** : protection des données personnelles
- **Évolution adaptative** comme les grenouilles s'adaptant à leur environnement

### 🔧 **API REST Démocratique**
- **GET** `/api/villes` - Toutes les alvéoles urbaines
- **GET** `/api/departements` - Alvéoles départementales
- **GET** `/api/communes` - Gestion communale
- **POST/PUT/DELETE** - Opérations CRUD complètes
- **Documentation interactive** via ApiDemo

## 🏗️ Architecture de la Ruche

### 🐝 Structure des Alvéoles (20-30 membres)

```csharp
// Types d'Alvéoles Citoyennes
public enum TypeAlveole
{
    Professionnels,      // Agriculteurs, apiculteurs, viticulteurs, artisans
    AgentsPublics,       // Services municipaux et départementaux
    Generationnelle,     // Jeunes (16-25) et seniors (60+)
    CitoyensAssociatifs, // Tirage au sort et associations
    AlveoleDuVivant     // 3 experts avec veto écologique
}
```

### 📊 Modèles de Données

```csharp
// Citoyen ambassadeur avec géolocalisation
public class Person
{
    public int Id { get; set; }
    public string Pseudo { get; set; }           // Identifiant dans la ruche
    public string? Email { get; set; }           // Contact (optionnel)
    public string? VilleCode { get; set; }       // Code alvéole urbaine
    public string? DepartementCode { get; set; } // Code alvéole départementale
    public double? Latitude { get; set; }        // Coordonnée GPS
    public double? Longitude { get; set; }       // Coordonnée GPS
    public DateTime DateCreation { get; set; }   // Date d'inscription
}

// Alvéole territoriale (ville/département)
public class Ville
{
    public string CodeInsee { get; set; }        // Code INSEE officiel
    public string Nom { get; set; }             // Nom de la ville
    public string CodePostal { get; set; }      // Code postal
    public string DepartementCode { get; set; }  // Rattachement départemental
    public double? Latitude { get; set; }        // Centre géographique
    public double? Longitude { get; set; }       // Centre géographique
    public int Population { get; set; }          // Population officielle
    public List<Person> Persons { get; set; }    // Citoyens de l'alvéole
}

// Alvéole créée par les citoyens
public class Alveole
{
    public int Id { get; set; }
    public string Nom { get; set; }              // Nom de l'alvéole
    public string Description { get; set; }      // Description/mission
    public string Email { get; set; }            // Contact principal
    public string? VilleCode { get; set; }       // Ville de rattachement
    public bool EstValidee { get; set; }         // Validation administrative
    public DateTime DateCreation { get; set; }   // Date de création
}
```

## 🎨 Design System - Thème Alvéoles & Sentinelles

### 🐝 Éléments Visuels Alvéoles
- **Palette couleurs** : Or (#FFD700), Orange (#FFA500), Miel (#FFF8DC)
- **Images intégrées** :
  - `alveole.png` - Alvéole hexagonale pour les sections principales
  - `slogan_main.png` - Slogan principal du mouvement
  - `slogan_fra.png` - Slogan français
  - `slogan_change.png` - Slogan du changement
  - `slogan.png` - Slogan général
  - `rubrixcube.png` - Structure organisationnelle

### 🐸 Sentinelles Écologiques
- **4 Images symboliques** depuis `/img/` :
  - `frog_big.png` - Gardienne des traditions et données
  - `frog_peace.png` - Médiatrice pour la paix et transparence
  - `frog_wings.png` - Visionnaire ailée pour l'innovation
  - `frog_wings_front.png` - Guide leader du changement

### 🎯 Interface Utilisateur
- **Hero sections** sur toutes les pages avec dégradés dorés
- **Cards interactives** avec effets hover et animations
- **Navigation thématique** avec grenouilles et icônes
- **Footer enrichi** avec informations du Plan B
- **Design responsive** mobile-first
- **Animations CSS** : floating, bounce, scale, rotate

## 🚀 Démarrage du Plan B

### Prérequis Techniques
- .NET 9 SDK
- SQL Server (LocalDB ou Express)
- Navigateur web moderne
- Esprit démocratique et participatif 🐝

### Installation de la Ruche

```bash
# Cloner le repository démocratique
git clone https://github.com/Mafyou/JustBee.git
cd JustBee

# Restaurer les dépendances citoyennes
dotnet restore src/

# Configurer la base de données
dotnet ef database update --project src/Alveoles/JustBeeInfrastructure

# Activer la ruche démocratique
dotnet run --project src/Alveoles/JustBeeWeb
```

### Accès aux Alvéoles

| Alvéole | URL | Mission Démocratique |
|---------|-----|---------------------|
| 🏠 **Plan B Accueil** | `/` | Présentation de l'innovation démocratique |
| 🗺️ **MapBee Départemental** | `/MapBee` | Cartographie des alvéoles départementales |
| 🏙️ **MapVille Communal** | `/MapVille` | Cartographie des alvéoles urbaines |
| 👥 **Gestion Citoyens** | `/PersonManagement` | Administration des ambassadeurs |
| 🆕 **Créer Alvéole** | `/CreerAlveole` | Création de nouvelles alvéoles |
| ✅ **Vérifier Alvéole** | `/VerifierAlveole` | Validation des alvéoles |
| 📧 **Vérifier Email** | `/VerifierEmail` | Validation des emails |
| 🔒 **Confidentialité RGPD** | `/Privacy` | Protection des données citoyennes |
| 🔧 **API Démocratique** | `/ApiDemo` | Interface technique des services |

## 🛠️ Technologies de la Ruche

### 🐝 Backend Démocratique
- **ASP.NET Core 9** : Infrastructure moderne et robuste
- **Razor Pages** : Architecture adaptée aux interfaces citoyennes
- **C# 13** : Langage de développement de pointe
- **Entity Framework Core 9** : ORM pour la persistance des données
- **SQL Server** : Base de données relationnelle
- **HybridCache** : Système de cache avancé .NET 9
- **API REST** : Services web pour l'interopérabilité

### 🐸 Frontend Citoyen
- **HTML5/CSS3** : Standards web modernes
- **JavaScript ES6+** : Interactions dynamiques
- **Bootstrap 5** : Framework responsive et accessible
- **Leaflet.js** : Cartographie open source
- **Font Awesome** : Iconographie professionnelle
- **Animations CSS** : Expérience utilisateur engageante

### 🗺️ Services Intégrés
- **Brevo (ex-Sendinblue)** : Service d'emails transactionnels
- **API Gouvernementale** : Données INSEE des communes françaises
- **Géolocalisation** : Coordonnées GPS précises
- **Cache hybride** : Performance optimisée

## 📊 Architecture du Projet

### 📁 Structure du Solution
```
src/
├── 📁 Alveoles/
│   ├── 📁 JustBeeWeb/           # Application web principale
│   │   ├── 📁 Controllers/      # Contrôleurs API
│   │   ├── 📁 Pages/           # Pages Razor
│   │   ├── 📁 Services/        # Services métier
│   │   ├── 📁 Models/          # Modèles de données
│   │   └── 📁 wwwroot/         # Ressources statiques
│   └── 📁 JustBeeInfrastructure/ # Couche de données
│       ├── 📁 Context/         # DbContext Entity Framework
│       ├── 📁 Data/           # Configuration et seeding
│       ├── 📁 Models/         # Entités de base de données
│       └── 📁 Repositories/   # Accès aux données
└── 📁 SharedMap/              # Composants cartographiques
    └── ShareStandaloneMap.csproj
```

### 🔧 Services Clés
- **VilleService** : Gestion des alvéoles urbaines
- **DepartementService** : Gestion des alvéoles départementales
- **AlveoleService** : Création et gestion des alvéoles citoyennes
- **EmailService** : Notifications et validations par email
- **CacheService** : Optimisation des performances
- **VilleDataService** : Intégration données gouvernementales

## 🌱 Mise en Place Effective

### 📅 Fonctionnalités Opérationnelles

#### ✅ **Actuellement Disponible**
- ✅ **Interface web complète** avec thématique alvéoles
- ✅ **Cartographie interactive** départementale et communale
- ✅ **Gestion des citoyens** avec CRUD complet
- ✅ **Création d'alvéoles** par les citoyens
- ✅ **Validation par email** automatisée
- ✅ **API REST** documentée
- ✅ **Base de données** Entity Framework avec migrations
- ✅ **Cache hybride** .NET 9 pour les performances

#### 🔄 **En Développement**
- 🔄 **Système d'authentification** complet
- 🔄 **Notifications temps réel** WebSocket
- 🔄 **Application mobile** native
- 🔄 **Tableaux de bord** avancés
- 🔄 **Export de données** pour les alvéoles

## 💡 Finalités Démocratiques

### 🎯 Objectifs Principaux
- **🤝 Représentation équitable** : Inclusion des composantes socioprofessionnelles et générationnelles
- **🔗 Liens directs renforcés** : Connexion citoyens ↔ professionnels du vivant ↔ élus
- **🌱 Résilience territoriale** : Rôle structurant pour agriculture, viticulture, apiculture
- **🛡️ Veto écologique** : Protection environnementale avec pouvoir symbolique et réel

### 🌍 Impact Territorial
- **Expérimentation locale** : Tests dans différentes communes
- **Réplication possible** : Modèle exportable dans toute la France
- **Réseau démocratique** : Interconnexion des ruches territoriales
- **Innovation constitutionnelle** : Expérimentation dans le cadre légal français

## 🤝 Participation Citoyenne

### 🐝 Rejoindre la Ruche
- **Inscription ouverte** : Tous les citoyens peuvent participer
- **Interface simple** : Création de compte en quelques clics
- **Alvéoles thématiques** : Spécialisation par domaine d'expertise
- **Engagement flexible** : Participation selon disponibilités

### 🌱 Contribuer au Développement
```bash
# Environnement de développement citoyen
- Visual Studio 2024 / VS Code
- .NET 9 SDK 
- SQL Server LocalDB/Express
- Git pour collaboration
- Esprit démocratique et participatif
```

## 🔗 Configuration et Secrets

### 🛠️ Variables d'Environnement
```bash
# Chaîne de connexion base de données
ConnectionStrings__DefaultConnection="Server=(localdb)\\mssqllocaldb;Database=JustBeeDb;Trusted_Connection=true"

# Configuration Brevo (emails)
Brevo__ApiKey="votre-clé-api-brevo"
Brevo__SenderEmail="contact@justbee.fr"
Brevo__SenderName="JustBee Plan B"
```

### 🔐 Secrets Utilisateur
```bash
# Initialiser les secrets utilisateur
dotnet user-secrets init --project src/Alveoles/JustBeeWeb

# Ajouter la clé API Brevo
dotnet user-secrets set "Brevo:ApiKey" "votre-clé-api" --project src/Alveoles/JustBeeWeb
```

## 🎯 Vision d'Avenir

### 🚀 Développements Prévus
- **Authentification OAuth** : Connexion sécurisée avec réseaux sociaux
- **Notifications push** : Alerts temps réel pour les citoyens
- **Blockchain démocratique** : Traçabilité des votes et décisions
- **IA participative** : Assistance aux délibérations collectives
- **Application mobile** : PWA et applications natives
- **Intégration IoT** : Capteurs environnementaux pour les alvéoles

### 🌍 Impact National
- **Réseau des ruches** : Interconnexion des communes participantes
- **Formation dédiée** : Académie de la démocratie participative
- **Recherche universitaire** : Études d'impact démocratique
- **Politique publique** : Influence sur la législation française

## 📜 Engagement Démocratique

### 🎖️ Valeurs Fondamentales
- **🌱 Écologie** : Respect de l'environnement et du vivant
- **🤝 Inclusion** : Participation de toutes les composantes sociales
- **🔍 Transparence** : Processus ouverts et traçables  
- **⚖️ Légalité** : Respect du cadre constitutionnel français
- **🔄 Adaptabilité** : Évolution selon les besoins citoyens

### 🏛️ Statut Légal
**Innovation démocratique expérimentale** dans le cadre constitutionnel français.
Aucune illégalité - Extension consultative des outils démocratiques existants.

---

## 🎉 Conclusion

**JustBee - Plan B Démocratie Participative** n'est pas simplement une application web, mais une **véritable innovation démocratique** qui transforme la participation citoyenne. En s'inspirant de la sagesse des abeilles et de l'adaptabilité des grenouilles sentinelles, cette plateforme offre une alternative concrète et légale pour revitaliser la démocratie locale.

**Comme les grenouilles qui annoncent les changements de saison, nous sommes les sentinelles d'une nouvelle ère démocratique.**

---

**🐝 Développé avec passion démocratique et .NET 9** | **🗺️ Cartographie par Leaflet** | **🎨 Design par Bootstrap** | **🐸 Sentinelles écologiques intégrées**

*"La technologie au service de la démocratie participative. JustBee : parce que chaque citoyen compte dans la ruche démocratique."*