# Sistema Veterinario

## Descripción

Este proyecto es una aplicación web desarrollada con ASP.NET Core MVC y Entity Framework Core. El sistema está diseñado para gestionar las operaciones de una clínica veterinaria, permitiendo administrar clientes, mascotas, usuarios del sistema, roles, consultas y tratamientos.

## Objetivo del Proyecto

El objetivo principal de este proyecto es demostrar las habilidades en el desarrollo de aplicaciones web bajo el patrón MVC, implementando un sistema completo con las siguientes características:

*   **Base de Datos Relacional:** Diseño y gestión de un esquema de base de datos coherente.
*   **Mantenedores CRUD:** Implementación de 6 módulos para Crear, Leer, Actualizar y Eliminar (CRUD) las entidades principales del sistema.
*   **Validaciones Robustas:** Aplicación de validaciones tanto del lado del servidor (Data Annotations) como del lado del cliente (JavaScript) para garantizar la integridad de los datos.
*   **Navegación y Usabilidad:** Una interfaz de usuario clara y accesible a través de un menú de navegación.
*   **Organización del Proyecto:** Mantenimiento de una estructura de carpetas y código limpia y organizada, siguiendo las convenciones de ASP.NET Core.

## Tecnologías Utilizadas

*   **.NET 9**
*   **ASP.NET Core MVC**
*   **Entity Framework Core**
*   **SQL Server**
*   **Bootstrap 5**
*   **JavaScript**
*   **SweetAlert2**

## Pasos para la Puesta en Marcha

Sigue estos pasos para clonar y ejecutar el proyecto en tu máquina local.

**1. Clonar el Repositorio**

```bash
git clone <URL_DEL_REPOSITORIO>
```

**2. Navegar al Directorio del Proyecto**

```bash
cd ruta/a/tu/proyecto/sistemaVeterinario
```

**3. Restaurar Dependencias**

Ejecuta el siguiente comando para descargar todas las dependencias de .NET necesarias.

```bash
dotnet restore
```

**4. Configurar la Base de Datos**

Este proyecto utiliza Entity Framework Core Migrations para crear y gestionar la base de datos. 

*   **Importante:** Asegúrate de que la cadena de conexión en `appsettings.json` o en `Models/SistemaVeterinarioContext.cs` (`Server=localhost\SQLEXPRESS;`) apunte a tu instancia local de SQL Server.

Una vez configurada la conexión, solo necesitas ejecutar el siguiente comando. Esto creará la base de datos, aplicará todas las migraciones en orden y sembrará los datos iniciales (roles, estados, etc.).

```bash
dotnet ef database update
```

**5. Ejecutar la Aplicación**

Usa el siguiente comando para compilar y ejecutar el proyecto.

```bash
dotnet run
```

**6. Acceder a la Aplicación**

Una vez que la aplicación se esté ejecutando, abre tu navegador y ve a la dirección que se indica en la terminal (generalmente `https://localhost:7263` o `http://localhost:5263`).

```