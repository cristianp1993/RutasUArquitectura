document.addEventListener("DOMContentLoaded", function () {
    const selectElement = document.getElementById("SelecSearcht");
    const inputElement = document.getElementById("ValueSearch");
    const buttonElement = document.getElementById("btn-buscar-info");
    const responseSection = document.querySelector(".response");
    const carouselUbicacion = document.getElementById("carouselUbicacion");
    const carouselInner = carouselUbicacion.querySelector(".carousel-inner");
    const baseUrlImg = carouselUbicacion.getAttribute("data-base-url");

    const validationDialog = document.getElementById("validationDialog");
    const dialogMessage = document.getElementById("dialogMessage");
    const closeDialogButton = document.getElementById("closeDialogButton");

    closeDialogButton.addEventListener("click", function () {
        validationDialog.close();
    });

    buttonElement.addEventListener("click", async function () {
        responseSection.innerHTML = "";

        // Ocultar carrusel
        carouselUbicacion.classList.remove("mostrar");
        carouselInner.innerHTML = "";

        if (typeof limpiarMapa === "function") {
            limpiarMapa();
        }

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
                body: JSON.stringify({ tipo, valorInput })
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
                    html += `
                        <div class="alert alert-info d-flex justify-content-between align-items-center" role="alert">
                            <span>${mensaje.mensajes}</span>
                            <button
                              class="btn btn-outline-success btn-sm"
                              data-ruta='${mensaje.ruta}' 
                              data-index='${index}'
                              onclick="verRecorrido(this)">
                              <i class="fas fa-route"></i>
                            </button>
                        </div>
                    `;
                });

                responseSection.innerHTML = html;

                // Guardar resultados en variable global
                window._resultadosBusqueda = data.data;

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

// Función global para usarla desde el botón
function verRecorrido(btn) {
    const ruta = btn.getAttribute("data-ruta");
    const index = btn.getAttribute("data-index");

    console.log("Click en botón de ruta", ruta); // ✅ ahora sí funciona

    // Pintar ruta
    if (typeof pintarRuta === "function") {
        pintarRuta(ruta);
    }

    // Mostrar carrusel con imágenes de la selección
    const data = window._resultadosBusqueda;
    if (!data || !data[index]) return;

    const item = data[index];
    const baseUrlImg = document.getElementById("carouselUbicacion").getAttribute("data-base-url");
    const salonImg = item.salonImagenUrl ? `${baseUrlImg}/${item.salonImagenUrl}` : `${baseUrlImg}/default_salon.jpg`;
    const edificioImg = item.edificioImagenUrl ? `${baseUrlImg}/${item.edificioImagenUrl}` : `${baseUrlImg}/default_edificio.jpg`;

    const carouselInner = document.querySelector("#carouselUbicacion .carousel-inner");
    carouselInner.innerHTML = `
        <div class="carousel-item active">
            <img src="${edificioImg}" class="d-block w-100" alt="Edificio">
        </div>
        <div class="carousel-item">
            <img src="${salonImg}" class="d-block w-100" alt="Salón">
        </div>
    `;

    // Mostrar con animación
    const carrusel = document.getElementById("carouselUbicacion");
    carrusel.classList.add("mostrar");
    $('.carousel').carousel(0);
}

