function initializeMap() {
    // Initialisation de la carte centrée sur la France
    var map = L.map('map').setView([46.603354, 1.888334], 6);

    // Ajout de la couche de tuiles OpenStreetMap
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    // Vérifier s'il y a des données à afficher
    if (villesData && villesData.length > 0) {
        // Création des marqueurs pour chaque ville avec des éléments vérifiés
        villesData.forEach(function (ville) {
            // Couleur du marqueur basée sur le nombre total d'éléments
            var markerColor = ville.totalCount > 2 ? 'red' : ville.totalCount > 1 ? 'orange' : 'green';

            // Création du contenu du popup
            var popupContent = `
                    <div class="popup-content">
                        <div class="d-flex align-items-center mb-2">
                            <img src="/img/alveole.png" style="width: 25px; height: 25px; margin-right: 8px;">
                            <h6 class="mb-0"><strong>🏙️ ${ville.nom}</strong></h6>
                        </div>
                        <p class="text-muted mb-2"><em>📍 ${ville.region} (${ville.departement})</em></p>
                        
                        ${ville.personCount > 0 ? `
                            <div class="mb-2">
                                <strong>🐝 ${ville.personCount} abeille(s) citoyenne(s) :</strong>
                                <ul class="list-unstyled mt-1">
                                    ${ville.persons.map(person => `<li>🐝 ${person.pseudo} <small class="text-muted">(ID: ${person.id})</small></li>`).join('')}
                                </ul>
                            </div>
                        ` : ''}
                        
                        ${ville.alveoleCount > 0 ? `
                            <div class="mb-2">
                                <strong>🏠 ${ville.alveoleCount} alvéole(s) :</strong>
                                <ul class="list-unstyled mt-1">
                                    ${ville.alveoles.map(alveole => `<li>🏠 ${alveole.nom}</li>`).join('')}
                                </ul>
                            </div>
                        ` : ''}
                        
                        <div class="text-center">
                            <a href="/CreerAlveole" class="btn btn-bee btn-sm">
                                <i class="fas fa-plus"></i> Créer une alvéole
                            </a>
                        </div>
                    </div>
                `;

            // Création du marqueur
            var marker = L.marker([ville.latitude, ville.longitude], {
                title: `${ville.nom} (${ville.totalCount} élément(s) vérifié(s))`
            }).addTo(map);

            // Ajout du popup au marqueur
            marker.bindPopup(popupContent);
        });

        // Ajustement de la vue pour inclure tous les marqueurs
        if (villesData.length > 0) {
            var group = new L.featureGroup(map._layers);
            if (Object.keys(group._layers).length > 0) {
                map.fitBounds(group.getBounds().pad(0.1));
            }
        }
    } else {
        // Afficher un message si aucune donnée
        var noDataPopup = L.popup()
            .setLatLng([46.603354, 1.888334])
            .setContent(`
                    <div class="text-center popup-content">
                        <img src="/img/alveole.png" style="width: 40px; height: 40px; margin-bottom: 10px;">
                        <h6>🗺️ Aucune alvéole vérifiée</h6>
                        <p>Commencez l'essaimage démocratique!</p>
                        <a href="/CreerAlveole" class="btn btn-bee btn-sm">
                            <i class="fas fa-plus"></i> Créer une alvéole
                        </a>
                    </div>
                `)
            .openOn(map);
    }

    // Ajout d'une légende
    var legend = L.control({ position: 'bottomright' });
    legend.onAdd = function (map) {
        var div = L.DomUtil.create('div', 'info legend');
        div.innerHTML += '<h6>🍯 Légende</h6>';
        div.innerHTML += '<i style="background: green; width: 18px; height: 18px; display: inline-block; margin-right: 8px; border-radius: 50%;"></i> 1 élément<br>';
        div.innerHTML += '<i style="background: orange; width: 18px; height: 18px; display: inline-block; margin-right: 8px; border-radius: 50%;"></i> 2 éléments<br>';
        div.innerHTML += '<i style="background: red; width: 18px; height: 18px; display: inline-block; margin-right: 8px; border-radius: 50%;"></i> 3+ éléments<br>';
        div.innerHTML += '<small style="color: #666;">🐝 Abeilles + 🏠 Alvéoles vérifiées</small>';
        return div;
    };
    legend.addTo(map);
}