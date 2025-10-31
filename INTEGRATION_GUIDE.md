# Guide d'intégration - Création automatique de Person "Responsable"

## 📋 Vue d'ensemble

Ce guide explique comment nous avons implémenté la fonctionnalité de création automatique d'une Person "Responsable" lors de la vérification d'une alvéole.

## 🔄 Processus automatisé

### 1. Quand une Alvéole est vérifiée

**Déclenchement :** Lors de la vérification d'une alvéole via `/VerifierAlveole?token=xxx`

**Flux :**
```
Page: VerifierAlveole.cshtml.cs
    ↓
Service: AlveoleService.VerifierAlveoleAsync()
    ↓
Repository: AlveoleRepository.VerifyEmailAsync()
    ↓
Logique ajoutée: Création automatique Person "Responsable"
```

**Code modifié :** `AlveoleRepository.VerifyEmailAsync()`

```csharp
// Vérifier si une Person "Responsable" existe déjà pour cette alvéole
var existingResponsable = await _context.Persons
    .FirstOrDefaultAsync(p => p.Email == alveole.Email && p.VilleCode == alveole.VilleCode);

if (existingResponsable == null)
{
    // Créer automatiquement une Person "Responsable" pour cette alvéole
    var responsablePerson = new Person
    {
        Pseudo = "Responsable",
        Email = alveole.Email,
        VilleCode = alveole.VilleCode,
        EmailVerifie = true, // Déjà vérifié via l'alvéole
        DateVerification = DateTime.UtcNow,
DateCreation = DateTime.UtcNow,
        Latitude = alveole.Latitude,
        Longitude = alveole.Longitude,
        TokenVerification = null // Pas de token car déjà vérifié
 };

    _context.Persons.Add(responsablePerson);
}
```

### 2. Quand une Person s'enregistre

**Déclenchement :** Lors de l'ajout d'une nouvelle Person

**Code modifié :** `PersonRepository.AddAsync()`

```csharp
// Générer un token de vérification si la Person n'est pas déjà vérifiée
if (!person.EmailVerifie && string.IsNullOrEmpty(person.TokenVerification))
{
    person.TokenVerification = Guid.NewGuid().ToString();
}

// S'assurer que la date de création est définie
if (person.DateCreation == default)
{
    person.DateCreation = DateTime.UtcNow;
}
```

## 🎯 Comportement attendu

### Scénario 1 : Vérification d'une nouvelle alvéole
1. Une alvéole est créée avec l'email `contact@alveole.fr` et ville `75001`
2. L'alvéole reçoit un token de vérification par email
3. **Lors du clic sur le lien de vérification :**
   - L'alvéole est marquée comme vérifiée (`EmailVerifie = true`)
   - **Automatiquement :** Une Person "Responsable" est créée avec :
     - Pseudo : "Responsable"
   - Email : `contact@alveole.fr` (même que l'alvéole)
     - VilleCode : `75001` (même que l'alvéole)
     - EmailVerifie : `true` (déjà vérifiée)
     - Coordonnées : celles de l'alvéole

### Scénario 2 : Vérification d'une alvéole existante
1. Si une Person avec le même email et la même ville existe déjà
2. **Aucune nouvelle Person n'est créée** (évite les doublons)

### Scénario 3 : Enregistrement direct d'une Person
1. Lors de la création d'une Person via l'interface ou l'API
2. **Automatiquement :**
   - Un token de vérification est généré si pas déjà vérifié
   - La date de création est définie si manquante

## 📊 Impact sur les données

### Nouvelles Persons créées automatiquement :
- **Pseudo :** toujours "Responsable"
- **Email :** identique à l'alvéole
- **VilleCode :** identique à l'alvéole
- **EmailVerifie :** `true` (hérite de la vérification de l'alvéole)
- **Coordonnées :** héritées de l'alvéole

### Avantages :
1. **Visibilité immédiate** de l'alvéole sur la carte (via la Person)
2. **Contact unifié** entre alvéole et responsable
3. **Pas de double vérification** nécessaire
4. **Évite les doublons** grâce à la vérification existante

## 🔍 Vérifications de sécurité

### Prévention des doublons :
```csharp
var existingResponsable = await _context.Persons
    .FirstOrDefaultAsync(p => p.Email == alveole.Email && p.VilleCode == alveole.VilleCode);
```

### Validation des données :
- Email valide (hérité de l'alvéole déjà validée)
- VilleCode valide (hérité de l'alvéole validée)
- Pas de token nécessaire (Person déjà vérifiée)

## 🧪 Test de fonctionnalité

### Pour tester manuellement :
1. Créer une nouvelle alvéole via `/CreerAlveole`
2. Vérifier l'email reçu
3. Cliquer sur le lien de vérification
4. Vérifier que :
   - L'alvéole est marquée comme vérifiée
   - Une Person "Responsable" a été créée automatiquement
   - La Person apparaît sur la carte `/MapBee`

### Requêtes SQL pour vérifier :
```sql
-- Vérifier les alvéoles vérifiées
SELECT * FROM Alveoles WHERE EmailVerifie = 1;

-- Vérifier les Persons "Responsable" créées automatiquement
SELECT * FROM Persons WHERE Pseudo = 'Responsable';

-- Voir la corrélation alvéole-responsable
SELECT a.Nom as AlveoleNom, a.Email as AlveoleEmail, 
       p.Pseudo, p.Email as PersonEmail, p.EmailVerifie
FROM Alveoles a
LEFT JOIN Persons p ON a.Email = p.Email AND a.VilleCode = p.VilleCode
WHERE a.EmailVerifie = 1;
```

## 📝 Notes importantes

1. **Pseudo fixe :** Toutes les Persons créées automatiquement ont le pseudo "Responsable"
2. **Email unique :** Une seule Person "Responsable" par combinaison email+ville
3. **Vérification héritée :** Pas besoin de vérification supplémentaire pour la Person
4. **Géolocalisation :** La Person hérite des coordonnées de l'alvéole

## 🔧 Extensions possibles

### Personnalisation du pseudo :
```csharp
// Au lieu de "Responsable", utiliser le nom de l'alvéole
Pseudo = $"Responsable-{alveole.Nom}",
```

### Liaison explicite :
```csharp
// Ajouter une propriété ResponsableAlveoleId sur Person
ResponsableAlveoleId = alveole.Id,
```

### Notification :
```csharp
// Envoyer un email à la Person créée automatiquement
await _emailService.NotifierPersonResponsableAsync(responsablePerson);
```