# 🏛️ RutasUArquitectura  
**Aplicación para explorar rutas en la arquitectura de la Universidad de Caldas.**  

Este proyecto permite visualizar recorridos basados en arquitectura dentro del campus universitario basados en la posicion donde se encontrará un profesor o un salon especifico,
utilizando tecnologías como C# .NET y la implementacion de IA para permitir usar un chat bot.  

## 🚀 Cómo descargar el proyecto  
### 1: Clonar el repositorio (recomendado para desarrollo)  
1. Abre tu terminal o Git Bash.  
    Ejecuta el siguiente comando:  
   ```bash
   git clone https://github.com/cristianp1993/RutasUArquitectura.git

2. Configurar la base de datos
El proyecto incluye un script SQL que debes ejecutar en SQL Server para crear la estructura de la base de datos necesaria.

Pasos:
Abre SQL Server Management Studio (SSMS) o la herramienta que uses para gestionar bases de datos SQL.
Conéctate a tu instancia local o servidor SQL.
Ejecuta el script de DB que ee encuentra en la carpeta DataBase en el proyecto
Este script contiene tanto la definición de tablas como datos iniciales necesarios para el funcionamiento del sistema.
Corre el script en este link , para llenar los datos de la DB: https://drive.google.com/file/d/1FATP98nz7Jig-z0M5ZV8g0wMzDOiaWoV/view?usp=sharing

3. Configurar la cadena de conexión
Dentro del proyecto, abre el archivo appsettings.json y actualiza la cadena de conexión a tu base de datos SQL Server:
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
 "ConnectionStrings": {
    "DefaultConnection": "Server=nombre_servidor;Database=RutasUArquitectura;User Id=usuario;Password=contraseña;"
  },
  "Authentication": {
    "Google": {
      "ClientId": "",
      "ClientSecret": ""
    }
  },
  "UrlImagenes": "https://rep-file.sagerp.cloud:54678/Pruebas/"

}
tambien el objeto de llave para Google que va en el appSettings pedir a un contacto(Administrador del sistema)

Esto iniciará la aplicación en modo desarrollo. Puedes acceder desde tu navegador visitando el localhost que abre C#

📦 Tecnologías utilizadas
Backend : C# .NET 8
Frontend : HTML, CSS, JavaScript (Bootstrap)
Base de Datos : Microsoft SQL Server

## 🤖 Puesta en marcha del microservicio de IA

> Estos pasos instalan **WSL 2 + Ubuntu**, compilan **llama.cpp** y arrancan el modelo **Gemma 1 B** como servidor HTTP en tu máquina local.  
> Requieren **privilegios de administrador** y una conexión a Internet estable.

---

### 1. Descargar el modelo Gemma

1. Descarga el archivo **`gemma-3-1b-it-Q4_K_M.gguf`**.  
2. Crea la carpeta **`C:\Models\`** (o la ruta que prefieras) y coloca el archivo dentro.

### 2. Instalar WSL 2 (Windows Subsystem for Linux)

powershell
# PowerShell como Administrador
wsl --install
Reinicia el equipo cuando lo solicite.

Al primer arranque se terminará de instalar Ubuntu; asigna un usuario y contraseña cuando lo pida.

3. Preparar Ubuntu
bash
Copiar
Editar
# En la consola de Ubuntu
sudo apt update && sudo apt upgrade -y
sudo apt install git build-essential cmake libcurl4-openssl-dev -y
4. Clonar y compilar llama.cpp
bash
Copiar
Editar
cd ~
git clone https://github.com/ggerganov/llama.cpp
cd llama.cpp
mkdir build && cd build
cmake ..
make -j        # compila con todos los núcleos disponibles
5. Iniciar el servidor Llama
bash
Copiar
Editar
# Siempre desde Ubuntu
cd ~/llama.cpp/build
./bin/llama-server -m /mnt/c/Models/gemma-3-1b-it-Q4_K_M.gguf
Si todo es correcto verás el mensaje:

arduino
Copiar
Editar
llama-server listening at http://localhost:8080 ...
6. Arranque rápido (sesiones futuras)
Cada vez que reinicies Windows → PowerShell como administrador:

powershell
Copiar
Editar
wsl                                 # abre Ubuntu
cd ~/llama.cpp/build
./bin/llama-server -m /mnt/c/Models/gemma-3-1b-it-Q4_K_M.gguf
7. Ajustar el microservicio en tu proyecto
Cadena de conexión de BD: actualiza appsettings.json en el proyecto de IA con la cadena correcta.

Endpoint del modelo: configura la URL que consume la IA (por defecto http://localhost:8080) en tu servicio o HttpClient.

Puerto: si cambias el puerto de llama-server, refleja el cambio en tu frontend/backend.
