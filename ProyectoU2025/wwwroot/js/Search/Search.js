document.addEventListener("DOMContentLoaded", function () {
    const selectElement = document.getElementById("SelecSearcht");
    const inputElement = document.getElementById("ValueSearch");
    const buttonElement = document.getElementById("btn-buscar-info");
    const responseSection = document.querySelector(".response");

    const validationDialog = document.getElementById("validationDialog");
    const dialogMessage = document.getElementById("dialogMessage");
    const closeDialogButton = document.getElementById("closeDialogButton");

    closeDialogButton.addEventListener("click", function () {
        validationDialog.close();
    });

    buttonElement.addEventListener("click", async function () {
        responseSection.innerHTML = "";

        const tipo = selectElement.value.trim();
        const valorInput = inputElement.value.trim();

        if (!tipo) {
            dialogMessage.textContent = "Por favor, selecciona una opción válida.";
            validationDialog.showModal();
            return;
        }

        if (!valorInput) {
            dialogMessage.textContent = "Por favor, ingresa un valor para buscar.";
            validationDialog.showModal();
            return;
        }

        try {
            const response = await fetch("/Ubicacion/Salon", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    tipo: tipo,
                    valorInput: valorInput
                })
            });

            if (!response.ok) {
                throw new Error(`Error HTTP: ${response.status}`);
            }

            const data = await response.json();
            console.log(data);

            if (data.success) {
                let html = `
                    <div class="alert alert-success" role="alert">
                        <strong>Resultado:</strong> ${data.message}
                    </div>
                `;

                data.data.forEach((mensaje, index) => {
                    const rutaFicticia = `Ruta-Salon-${index}`;

                    html += `
                        <div class="alert alert-info d-flex justify-content-between align-items-center" role="alert">
                            <span>${mensaje.mensajes}</span>
                            <button
                              class="btn btn-outline-success btn-sm"
                              data-ruta='${mensaje.ruta}' 
                              onclick="verRecorrido(this)">
                              <i class="fas fa-route"></i>
                            </button>
                        </div>
                    `;
                });


                responseSection.innerHTML = html;
            } else {
                responseSection.innerHTML = `
                    <div class="alert alert-danger" role="alert">
                        <strong>Error:</strong> ${data.message}
                    </div>
                `;
            }
        } catch (error) {
            responseSection.innerHTML = `
                <div class="alert alert-danger" role="alert">
                    <strong>Error inesperado:</strong> ${error.message}
                </div>
            `;
        }
    });
});

function verRecorrido(btn) {
    const ruta = JSON.parse(btn.getAttribute("data-ruta"));
    console.log(`Ruta a mostrar: ${ruta}`);

    const coordenadas = ruta.map(punto => [punto.Latitud, punto.Longitud]);

    console.log(coordenadas);

    
    ruta.forEach((punto, index) => {
        const latitud = punto.Latitud;
        const longitud = punto.Longitud;

        // Determinar color según posición
        let color = 'blue';
        if (index === 0) {
            color = 'red';
            map.setView([latitud, longitud], 20); 
        } else if (index === ruta.length - 1) {
            color = 'green';
        }

        // Crear el punto circular con color dinámico
        L.circleMarker([latitud, longitud], {
            radius: 4,
            color: color,
            fillColor: color,
            fillOpacity: 1
        })
            .addTo(map)
            .bindPopup(`Punto ${index + 1}: Lat ${latitud}, Lng ${longitud}`);
    });


        const linea = L.polyline(coordenadas, { color: 'blue' }).addTo(map);

}

