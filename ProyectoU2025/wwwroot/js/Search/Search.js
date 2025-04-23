
document.addEventListener("DOMContentLoaded", function () {
    // Obtener referencias a los elementos del DOM
    const selectElement = document.getElementById("SelecSearcht");
    const inputElement = document.getElementById("ValueSearch");
    const buttonElement = document.getElementById("btn-buscar-info");
    const responseSection = document.querySelector(".response");

    // Referencias al diálogo
    const validationDialog = document.getElementById("validationDialog");
    const dialogMessage = document.getElementById("dialogMessage");
    const closeDialogButton = document.getElementById("closeDialogButton");

    // Cerrar el diálogo cuando se hace clic en el botón "Cerrar"
    closeDialogButton.addEventListener("click", function () {
        validationDialog.close();
    });

    // Agregar un evento click al botón
    buttonElement.addEventListener("click", async function () {
        // Limpiar la sección de respuesta
        responseSection.innerHTML = "";

        // Obtener los valores del select y del input
        const tipo = selectElement.value.trim();
        const valorInput = inputElement.value.trim();

        // Validar que ambos campos tengan valor
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
            // Realizar la solicitud fetch POST
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

            // Verificar si la respuesta es exitosa
            if (!response.ok) {
                throw new Error(`Error HTTP: ${response.status}`);
            }
            
            const data = await response.json();
            console.log(data)
            if (data.success) {
                
                responseSection.innerHTML = `
                    <div class="alert alert-success" role="alert">
                        <strong>Resultado:</strong> ${data.message}
                    </div>                  
                `;

                const ruta = data.data ? data.data["RutaEdificio"] : "";

                MostrarRecorrido(ruta)

            } else {
                // Mostrar un mensaje de error si success es false
                responseSection.innerHTML = `
                    <div class="alert alert-danger" role="alert">
                        <strong>Error:</strong> ${data.message}
                    </div>
                `;
            }
        } catch (error) {
            // Mostrar un mensaje de error en caso de excepción
            responseSection.innerHTML = `
                <div class="alert alert-danger" role="alert">
                    <strong>Error inesperado:</strong> ${error.message}
                </div>
            `;
        }
    });




});


function MostrarRecorrido(ruta) {

    if (ruta == "")
        return


    const rutaJson = JSON.parse(ruta)

    rutaJson.forEach((item)=>{

        console.log('Latitud:' + item.Latitud + ' - ' + 'Longitu:' + item.Longitud)
    })
}