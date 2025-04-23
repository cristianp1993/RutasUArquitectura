const errorDialog = document.getElementById("errorDialog");
const errorMessage = document.getElementById("errorMessage");
const closeDialog = document.getElementById("closeDialog");

document.addEventListener("DOMContentLoaded", function () {
    const loginButton = document.getElementById("loginButton");
    const emailInput = document.getElementById("email");
    const passwordInput = document.getElementById("password");
   

  

    // Evento para cerrar el diálogo
    closeDialog.addEventListener("click", function () {
        errorDialog.close(); 
    });

    // Evento para el botón INGRESAR
    loginButton.addEventListener("click", function (e) {
        e.preventDefault()

        const email = emailInput.value.trim();
        const password = passwordInput.value.trim();

        // Validar el correo electrónico
        if (!email.endsWith("@ucaldas.edu.co")) {
            showError("Por favor, ingrese un correo válido de @ucaldas.edu.co.");
            return; // Detener la ejecución si el correo no es válido
        }

        // Validar la contraseña
        if (password.length < 6) {
            showError("La contraseña debe tener al menos 6 caracteres.");
            return; 
        }

        // Si todo está correcto, redirigir al usuario
        ValidateUser(email, password)
    });
});

// Función para abrir el diálogo con un mensaje específico
function showError(message) {
    errorMessage.textContent = message;
    errorDialog.showModal();
}

async function ValidateUser(email, password) {
    try {
        const response = await fetch("/Usuario/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ Email: email, Password: password }),
        });

        const result = await response.json();

        if (response.ok && result.success) {
            // Redirigir al perfil si las credenciales son válidas
            window.location.href = result.redirectUrl;
        } else {
            // Mostrar mensaje de error si las credenciales son inválidas
            showError(result.message || "Credenciales inválidas");
        }
    } catch (error) {
        showError("Ocurrió un error al intentar iniciar sesión.");
    }
}