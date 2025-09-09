# 🐝 Plan B - Démocratie Participative

![.NET 10](https://img.shields.io/badge/.NET-10-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET-Core-blue)
![Razor Pages](https://img.shields.io/badge/Razor-Pages-green)
![Leaflet](https://img.shields.io/badge/Leaflet-Maps-red)
![Democracy](https://img.shields.io/badge/Democracy-Participative-yellow)

## 📋 Vue d'ensemble

**Plan B - Démocratie Participative** est une application web révolutionnaire développée avec ASP.NET Core 10 et Razor Pages, conçue pour mettre en œuvre une innovation démocratique expérimentale. Cette plateforme permet de gérer et visualiser géographiquement les **alvéoles citoyennes** réparties dans les départements français, favorisant une **réappropriation citoyenne bottom-up** dans le cadre constitutionnel français.

L'application s'inspire de la structure des ruches avec ses **alvéoles modulaires** et intègre des **ambassadeurs de la biodiversité** (grenouilles) comme indicateurs de la santé démocratique territoriale.

## 🎯 Contexte du Plan B

### Origine et Justification
Le Plan B fait suite à **une lettre envoyée le 6 juillet 2025** au Président et au Maire de Carcès, proposant un organe consultatif représentatif avec un symbole écologique (pot de miel). L'absence de réponse après plus de 60 jours justifie cette **réponse pragmatique à une crise de défiance institutionnelle**.

### Fondements Juridiques
- **Articles L2143-1 à L2143-4 du CGCT** : Conseils consultatifs
- **Article L1112-17 du CGCT** : Participation des habitants  
- **Chartes de démocratie participative** (Région Sud)
- **Loi 1901** : Organisation associative
- **Conformité totale** : Extension consultative légale des outils existants

## ✨ Fonctionnalités Principales

### 🏛️ **Accueil - Plan B Présentation**
- **Hero section** avec thématique abeilles et présentation du plan
- **Section ambassadeurs biodiversité** avec les 4 grenouilles symboliques :
  - 🐸 **Gardienne des Traditions** (`frog_big.png`) - Sagesse ancestrale
  - 🐸 **Médiatrice Citoyenne** (`frog_peace.png`) - Dialogue pacifique  
  - 🐸 **Visionnaire du Futur** (`frog_wings.png`) - Innovation démocratique
  - 🐸 **Guide du Changement** (`frog_wings_front.png`) - Leadership de transition
- **Présentation détaillée** : Contexte, finalités, fondements juridiques
- **Structure de la Ruche** : Organisation en alvéoles thématiques
- **Timeline de mise en place** : Du lancement immédiat à l'extension

### 🗺️ **MapBee - Cartographie des Alvéoles**
- **Carte interactive** des départements français avec visualisation des citoyens
- **Statistiques en temps réel** avec indicateurs grenouilles
- **Marqueurs couleur** selon la densité citoyenne (vert: 1, orange: 2, rouge: 3+)
- **Popups thématiques** avec informations des alvéoles territoriales
- **Panneau de bord** avec métriques de couverture territoriale
- **Intégration visuelle** : abeilles et grenouilles dans l'interface

### 👥 **Gestion des Citoyens**
- **Interface complète** pour administrer les ambassadeurs démocratiques
- **Terminologie adaptée** : "Citoyens", "Alvéoles territoriales", "Ruche"
- **Statistiques visuelles** avec les 4 grenouilles indicatrices
- **Génération automatique** de citoyens pour tester le système
- **Recherche temps réel** et gestion par alvéoles
- **Design cohérent** avec hero section et thème abeilles/grenouilles

### 🔒 **Politique de Confidentialité**
- **Protection RGPD** complète avec thématique grenouilles
- **4 principes** représentés par les ambassadeurs biodiversité
- **Droits citoyens** détaillés dans le contexte démocratique
- **Contact DPO** : `dpo@planb-democratie.fr` à Carcès (83)
- **Évolution adaptative** comme les grenouilles s'adaptant à leur environnement

### 🔧 **API REST Démocratique**
- **GET** `/api/departements` - Toutes les alvéoles territoriales
- **GET** `/api/departements/{code}` - Alvéole spécifique
- **GET** `/api/departements/with-persons` - Alvéoles avec citoyens
- **POST** `/api/departements/{code}/persons` - Intégrer un citoyen
- **DELETE** `/api/departements/{code}/persons/{id}` - Retirer un citoyen
- **GET** `/api/departements/persons` - Tous les ambassadeurs

## 🏗️ Architecture de la Ruche

### 🐝 Structure des Alvéoles (20-30 membres)

```csharp
// Alvéole Professionnels
// Agriculteurs, apiculteurs, viticulteurs, artisans, santé, enseignants

// Alvéole Agents Publics  
// Services municipaux

// Alvéole Générationnelle
// Jeunes (16-25) et seniors (60+)

// Alvéole Citoyens et Associatifs
// Tirage au sort

// Alvéole du Vivant (3 experts avec veto écologique)
// Protection environnementale avec pouvoir décisionnel
```

### Modèles de Données Citoyennes

```csharp
// Citoyen ambassadeur avec géolocalisation
public class Person
{
    public int Id { get; set; }
    public string Pseudo { get; set; }           // Identifiant unique dans la ruche
    public string? DepartementCode { get; set; }  // Code alvéole territoriale
    public double? Latitude { get; set; }        // Coordonnée GPS
    public double? Longitude { get; set; }       // Coordonnée GPS
}

// Alvéole territoriale (département)
public class Departement
{
    public string Code { get; set; }           // Ex: "83" (Var), "13" (Bouches-du-Rhône)
    public string Nom { get; set; }            // Ex: "Var", "Bouches-du-Rhône"  
    public string Region { get; set; }         // Ex: "Provence-Alpes-Côte d'Azur"
    public double Latitude { get; set; }       // Centre géographique
    public double Longitude { get; set; }      // Centre géographique
    public List<Person> Persons { get; set; }  // Citoyens de l'alvéole
}
```

## 🎨 Design System - Thème Abeilles & Grenouilles

### 🐝 Éléments Visuels Abeilles
- **Palette couleurs** : Or (#FFD700), Orange (#FFA500), Miel (#FFF8DC)
- **Images intégrées** :
  - `bee-hero.svg` - Abeille héroïque pour les sections principales
  - `honeycomb.svg` - Alvéoles hexagonales pour les arrière-plans
  - `beehive.svg` - Ruche pour représenter la structure

### 🐸 Ambassadeurs Grenouilles
- **4 Images symboliques** depuis `/img/` :
  - `frog_big.png` - Gardienne des traditions et données
  - `frog_peace.png` - Médiatrice pour la paix et transparence
  - `frog_wings.png` - Visionnaire ailée pour l'innovation
  - `frog_wings_front.png` - Guide leader du changement

### 🎯 Interface Utilisateur
- **Hero sections** sur toutes les pages avec dégradés dorés
- **Cards interactives** avec hover effects et animations
- **Navigation thématique** avec grenouilles et icônes
- **Footer enrichi** avec informations du Plan B
- **Responsive design** mobile-first
- **Animations CSS** : floating, bounce, scale, rotate

## 🚀 Démarrage du Plan B

### Prérequis Techniques
- .NET 10 SDK
- Navigateur web moderne
- Esprit démocratique et participatif 🐝

### Installation de la Ruche

```bash
# Cloner le repository démocratique
git clone https://github.com/Mafyou/JustBee.git
cd JustBee

# Restaurer les dépendances citoyennes
dotnet restore src/

# Activer la ruche démocratique
dotnet run --project src/
```

### Accès aux Alvéoles

| Alvéole | URL | Mission Démocratique |
|---------|-----|---------------------|
| 🏠 **Plan B Accueil** | `/` | Présentation de l'innovation démocratique |
| 🗺️ **MapBee Territorial** | `/MapBee` | Cartographie des alvéoles citoyennes |
| 👥 **Gestion Citoyens** | `/PersonManagement` | Administration des ambassadeurs |
| 🔒 **Confidentialité RGPD** | `/Privacy` | Protection des données citoyennes |
| 🔧 **API Démocratique** | `/ApiDemo` | Interface technique des services |

## 🌱 Mise en Place Effective

### 📅 Timeline d'Activation

#### **Phase Immédiate** (Septembre 2025)
- ✅ **Assemblée citoyenne** à Carcès
- ✅ **Plateforme numérique** opérationnelle
- ✅ **Invitation au Maire** Alain Ravanello

#### **Semaines 1-2**
- 🔄 **Installation de la Ruche** et constitution des alvéoles
- 🔄 **Recrutement** par appels publics et tirage au sort
- 🔄 **Formation** des ambassadeurs grenouilles

#### **Mois 1**
- 📊 **Réunions mensuelles** : Délibérations par alvéoles puis plénière
- 📢 **Avis publics** transmis au conseil municipal
- 📋 **Règlement interne** avec veto écologique
- 💰 **Financement** par dons et subventions associatives

#### **Mois 3+**
- 📈 **Évaluation d'impact** et mesure des résultats
- 🌐 **Extension réseau** via plateforme [Aurore](https://aurores.org)
- 🏛️ **Duplication** dans d'autres communes
- 🔄 **Amélioration continue** du système

## 💡 Finalités Démocratiques

### 🎯 Objectifs Principaux
- **🤝 Représentation équitable** : Inclusion des composantes socioprofessionnelles et générationnelles
- **🔗 Liens directs renforcés** : Connexion citoyens ↔ professionnels du vivant ↔ élus
- **🌱 Résilience territoriale** : Rôle structurant pour agriculture, viticulture, apiculture
- **🛡️ Veto écologique** : Protection environnementale avec pouvoir symbolique et réel

### 🌍 Impact Territorial
- **Carcès comme pilote** : Commune test dans le Var (83)
- **Réplication possible** : Modèle exportable dans toute la France
- **Réseau démocratique** : Interconnexion des ruches territoriales
- **Innovation constitutionnelle** : Expérimentation dans le cadre légal français

## 🛠️ Technologies de la Ruche

### 🐝 Backend Démocratique
- **ASP.NET Core 10** : Infrastructure moderne et robuste
- **Razor Pages** : Architecture adaptée aux interfaces citoyennes
- **C# 13** : Langage de développement de pointe
- **API REST** : Services web pour l'interopérabilité

### 🐸 Frontend Citoyen
- **HTML5/CSS3** : Standards web modernes
- **JavaScript ES6+** : Interactions dynamiques
- **Bootstrap 5** : Framework responsive et accessible
- **Leaflet.js** : Cartographie open source
- **Font Awesome** : Iconographie professionnelle
- **Animations CSS** : Expérience utilisateur engageante

### 🗺️ Données Territoriales
- **101 Départements français** : Couverture nationale complète
- **Coordonnées GPS précises** : Géolocalisation des alvéoles
- **Stockage en mémoire** : Performance optimale pour la démo
- **JSON API** : Format d'échange standard

## 📊 Métriques Démocratiques

### 🏛️ Indicateurs Clés
- **Citoyens enregistrés** : Compteur en temps réel
- **Départements actifs** : Alvéoles avec ambassadeurs
- **Couverture territoriale** : Pourcentage national
- **Moyenne par département** : Répartition équilibrée

### 🐸 Ambassadeurs Biodiversité
- **Frog Big** : Gardienne des traditions (données personnelles)
- **Frog Peace** : Médiatrice citoyenne (transparence)
- **Frog Wings** : Visionnaire du futur (innovation)
- **Frog Wings Front** : Guide du changement (leadership)

## 🔗 Liens Démocratiques

### 📚 Ressources Officielles
- **Code général des collectivités territoriales** : Base légale
- **Constitution française** : Article 72 (décentralisation)
- **RGPD européen** : Protection des données
- **Chartes de démocratie participative** : Bonnes pratiques

### 🌐 Plateforme Numérique
- **Repository GitHub** : [Plan B Democracy](https://github.com/Mafyou/JustBee)
- **Documentation technique** : Code source et APIs
- **Plateforme Aurore** : [aurores.org](https://aurores.org) (à venir)
- **Contact DPO** : dpo@planb-democratie.fr

## 🤝 Participation Citoyenne

### 🐝 Rejoindre la Ruche
- **Inscription ouverte** : Tous les citoyens peuvent participer
- **Tirage au sort** : Sélection équitable et démocratique
- **Alvéoles thématiques** : Spécialisation par domaine d'expertise
- **Engagement flexible** : Participation selon disponibilités

### 🌱 Contribuer au Développement
```bash
# Environnement de développement citoyen
- Visual Studio 2024 / VS Code
- .NET 10 SDK 
- Git pour collaboration
- Esprit démocratique et participatif
```

### 📁 Structure Démocratique
```
src/
├── 📁 Models/              # Modèles citoyens et alvéoles
├── 📁 Services/            # Services démocratiques
├── 📁 Controllers/         # APIs participatives  
├── 📁 Pages/              # Interfaces citoyennes
├── 📁 wwwroot/            # Ressources statiques (abeilles/grenouilles)
│   ├── 📁 images/         # SVG thématiques (bee-hero, honeycomb, beehive)
│   ├── 📁 img/            # PNG ambassadeurs (4 grenouilles)
│   └── 📁 css/            # Styles Plan B
└── 📄 Program.cs          # Point d'entrée démocratique
```

## 🎯 Vision d'Avenir

### 🚀 Développements Prévus
- **Base de données persistante** : Migration vers PostgreSQL
- **Authentification citoyenne** : Système de connexion sécurisé
- **Notifications temps réel** : WebSocket pour les délibérations
- **Mobile app** : Application native pour smartphones
- **Blockchain démocratique** : Traçabilité des votes et décisions
- **IA participative** : Assistance aux délibérations collectives

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

Le **Plan B - Démocratie Participative** n'est pas simplement une application web, mais une **véritable innovation démocratique** qui transforme la participation citoyenne. En s'inspirant de la sagesse des abeilles et de l'adaptabilité des grenouilles, cette plateforme offre une alternative concrète et légale pour revitaliser la démocratie locale.

**Comme les grenouilles qui annoncent les changements de saison, nous sommes les sentinelles d'une nouvelle ère démocratique.**

---

**🐝 Développé avec passion démocratique et .NET 10** | **🗺️ Cartographie par Leaflet** | **🎨 Design par Bootstrap** | **🐸 Ambassadeurs biodiversité intégrés**

*"Au 9 septembre 2025, le Plan B n'est pas encore effectif publiquement, mais prêt à l'être via initiative citoyenne. Mobilisez-vous dès maintenant !"*