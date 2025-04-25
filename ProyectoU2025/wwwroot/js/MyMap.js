let map = L.map('my_map').setView([5.055337, -75.493689], 17);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap contributors'
}).addTo(map);

L.marker([5.055337, -75.493689]).addTo(map)
    .bindPopup('Universidad de Caldas');

let legend = null;

window.limpiarMapa = function () {
    map.eachLayer(layer => {
        if (!(layer instanceof L.TileLayer)) {
            map.removeLayer(layer);
        }
    });

    if (legend) {
        map.removeControl(legend);
        legend = null;
    }
};

window.pintarRuta = function (rutaJson) {
    console.log("pintarRuta fue llamada", rutaJson);
    if (!rutaJson) return;

    let ruta;

    try {
        ruta = JSON.parse(rutaJson);
    } catch (e) {
        console.error("Error al parsear JSON:", e.message);
        return;
    }

    if (!Array.isArray(ruta) || ruta.length === 0) {
        console.error("El array de la ruta está vacío");
        return;
    }

    limpiarMapa();

    const coordenadas = ruta.map(p => [parseFloat(p.Latitud), parseFloat(p.Longitud)]);

    ruta.forEach((punto, index) => {
        const lat = parseFloat(punto.Latitud);
        const lng = parseFloat(punto.Longitud);

        let color = 'blue';
        let descripcion = `Punto ${index + 1}`;

        if (index === 0) {
            color = 'red';
            descripcion = 'Entrada Universidad';
            map.setView([lat, lng], 20);
        } else if (index === ruta.length - 1) {
            color = 'green';
            descripcion = 'Entrada Edificio';
        }

        L.circleMarker([lat, lng], {
            radius: 4,
            color: color,
            fillColor: color,
            fillOpacity: 1
        })
            .addTo(map)
            .bindPopup(`${descripcion}<br>Lat ${lat}, Lng ${lng}`);
    });

    L.polyline(coordenadas, { color: 'blue' }).addTo(map);

    legend = L.control({ position: 'topright' });
    legend.onAdd = function () {
        const div = L.DomUtil.create('div', '');

        div.style.backgroundColor = 'rgba(255, 255, 255, 0.9)';
        div.style.padding = '10px';
        div.style.borderRadius = '8px';
        div.style.boxShadow = '0 2px 10px rgba(0,0,0,0.2)';
        div.style.fontFamily = '"Segoe UI", Roboto, sans-serif';
        div.style.color = '#2F3542';
        div.style.lineHeight = '1.5';
        div.style.maxWidth = '200px';

        div.innerHTML = `
            <h4 style="margin: 0 0 10px 0; font-size: 16px; border-bottom: 1px solid #DFE4EA; padding-bottom: 5px; color: #1E272E;">
                🗺️ Leyenda del Recorrido
            </h4>
            <div style="display: flex; align-items: center; margin-bottom: 5px;">
                <div style="width: 16px; height: 16px; background-color: #FF4757; border-radius: 50%; border: 2px solid white; margin-right: 8px;"></div>
                <span>Entrada Universidad</span>
            </div>
            <div style="display: flex; align-items: center; margin-bottom: 5px;">
                <div style="width: 16px; height: 16px; background-color: #2ED573; border-radius: 50%; border: 2px solid white; margin-right: 8px;"></div>
                <span>Entrada Edificio</span>
            </div>
            <div style="display: flex; align-items: center; margin-bottom: 5px;">
                <div style="width: 16px; height: 16px; background-color: #1E90FF; border-radius: 50%; border: 2px solid white; margin-right: 8px;"></div>
                <span>Puntos intermedios</span>
            </div>
            <div style="display: flex; align-items: center;">
                <div style="width: 20px; height: 4px; background-color: #3498DB; margin-right: 8px;"></div>
                <span>Ruta</span>
            </div>
        `;

        return div;
    };

    legend.addTo(map);
};
