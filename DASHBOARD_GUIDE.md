# Guía de Implementación de Dashboards

Este documento describe la estrategia y los pasos técnicos para la creación de dashboards personalizados según el rol del usuario en el sistema.

## 1. Visión General

A diferencia de un único dashboard genérico, el sistema contará con tres dashboards especializados para proporcionar información relevante y accionable a cada tipo de usuario. El objetivo es mejorar la eficiencia operativa, la toma de decisiones y la gestión diaria de la clínica veterinaria.

La implementación técnica se basará en un enfoque moderno y desacoplado:
- **Backend (C# / ASP.NET Core):** Se crearán endpoints de API seguros que consultarán la base de datos y devolverán los datos necesarios en formato JSON.
- **Frontend (JavaScript / ApexCharts):** Se utilizará la librería [ApexCharts](httpss://apexcharts.com/) para consumir los datos de los endpoints y renderizar gráficos interactivos y visualmente atractivos en el navegador del cliente.

## 2. Definición de Dashboards por Rol

### 2.1. Dashboard Administrativo (Visión Estratégica)

- **Propósito:** Ofrecer una vista de alto nivel sobre el rendimiento y crecimiento de la clínica.
- **Audiencia:** Administradores y dueños.
- **Métricas y Componentes Clave:**
    - **Tarjetas de KPIs:**
        - Número total de Clientes.
        - Número total de Mascotas.
        - Número total de Veterinarios.
        - Total de Consultas (histórico).
    - **Gráfico de Líneas/Barras (Crecimiento):**
        - `Nuevos Clientes por Mes`: Un gráfico que muestre cuántos clientes nuevos se han registrado en los últimos 6 o 12 meses.
        - `Consultas por Mes`: Volumen de consultas realizadas mensualmente para identificar tendencias y estacionalidad.
    - **Gráfico de Torta/Dona (Distribución):**
        - `Distribución de Mascotas por Especie`: Un gráfico que muestre la proporción de perros, gatos, etc.
    - **Tabla de Actividad (Opcional):**
        - Una tabla que resuma la cantidad de consultas atendidas por cada veterinario en el último mes.

### 2.1.1. Funcionalidad de Exportación

Para facilitar el análisis de datos fuera del sistema, el dashboard administrativo incluirá opciones para exportar las tablas completas de datos a formatos estándar.

- **Ubicación:** Al lado del título de cada sección que presente datos tabulares (como Clientes, Mascotas, Consultas), se encontrarán dos botones.
- **Botones:**
    - `Descargar Excel`: Generará y descargará un archivo `.xlsx` con todos los datos de la tabla correspondiente.
    - `Descargar PDF`: Generará y descargará un archivo `.pdf` con una versión imprimible de los datos.

**Implementación técnica:**
- Se crearán nuevas acciones en los controladores correspondientes (ej. `ClientesController`, `ConsultasController`).
- Estas acciones recuperarán los datos completos, los procesarán utilizando las clases de ayuda `ExcelExporter` y `PdfExporter` existentes en la carpeta `Helpers`, y devolverán el archivo generado al usuario.

### 2.2. Dashboard Veterinario (Visión Clínica y Operativa)

- **Propósito:** Facilitar al veterinario la gestión de su agenda, consultas y pacientes.
- **Audiencia:** Veterinarios.
- **Métricas y Componentes Clave:**
    - **Agenda del Día:**
        - Una vista de calendario o una lista cronológica con las consultas asignadas para el día actual.
        - Indicadores visuales para el estado de la consulta (Pendiente, En Progreso, Finalizada).
    - **Tarjetas de Acceso Rápido:**
        - `Consultas Pendientes Hoy`: Número de citas que aún no han comenzado.
        - `Consultas en Progreso`: Número de consultas que están actualmente activas.
    - **Tabla de Pacientes Recientes:**
        - Una lista de las últimas 5-10 mascotas atendidas, con un enlace directo a su historial.
    - **Gráfico Personal (Opcional):**
        - `Mis Diagnósticos Comunes`: Un gráfico de barras que muestre los diagnósticos más frecuentes emitidos por el veterinario.

### 2.3. Dashboard Recepcionista (Visión Logística y de Cliente)

- **Propósito:** Optimizar la gestión de la recepción, agendamiento y flujo de clientes.
- **Audiencia:** Recepcionistas y personal de secretaría.
- **Métricas y Componentes Clave:**
    - **Tarjetas de Tareas Diarias:**
        - `Nuevos Clientes Registrados Hoy`.
        - `Nuevas Mascotas Registradas Hoy`.
        - `Consultas del Día (Total)`.
    - **Agenda General del Día:**
        - Similar a la del veterinario, pero mostrando las citas de *todos* los veterinarios para tener una visión completa de la ocupación de la clínica.
    - **Tabla de "Acciones Requeridas":**
        - Una lista de consultas en estado "Pendiente" para confirmar con los clientes.
        - Una lista de consultas "Finalizadas" que podrían requerir agendar un próximo control.
    - **Formulario de Búsqueda Rápida:**
        - Un campo para buscar rápidamente un cliente o mascota por nombre o RUN.

## 3. Pasos de Implementación Técnica (Arquitectura Revisada)

La página de inicio del sistema (`/Home/Index`) actuará como un "despachador" que mostrará el dashboard correcto según el rol del usuario que ha iniciado sesión.

1.  **Crear Endpoints de Datos (Sin cambios):**
    - En un controlador apropiado (ej. `HomeController.cs`), crear acciones `async Task<IActionResult>` para cada dashboard.
    - Ejemplo: `GetAdminDashboardData()`, `GetVetDashboardData()`, etc.
    - Cada acción estará protegida por `[Authorize]` y devolverá los datos en formato `JsonResult`.

```csharp
// Ejemplo Endpoint para el Dashboard Administrativo:
// Metodo para obtener los datos del dashboard del admin.
[Authorize(Roles  = "Admin")]
public async Task<IActionResult> ObtenerAdminDashboardData()
{
	try
	{
		// Obtener conteos totales de los clientes, mascotas y consultas.
		var totalClientes = await _context.Clientes.CountAsync();
		var totalMascotas = await _context.Mascotas.CountAsync();
		var totalConsultas = await _context.Consultas.CountAsync();

		// Crear query para obtener las consultas de las ultimos 6 meses.
		var ultimos6Meses = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6));

		var consultas = await _context.Consultas
			.Where(c => c.FechaConsulta >= ultimos6Meses) // filtrar por fecha.
			.ToListAsync();

		var consultasPorMes = consultas
			.GroupBy(c => new { c.FechaConsulta.Year, c.FechaConsulta.Month }) // agrupar por año y mes.
			.Select(g => new // asignar los resultados en un objeto anonimo.
			{
				Mes = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"), // etiqueta mas legible.
				Cantidad = g.Count() // contar los elementos en cada grupo para obtener el total mensual.
			})
			.OrderBy(x => x.Mes) // ordenar por mes, asi el grafico tiene el orden cronologico.
			.ToList();

		// Crear objeto para enviar como objeto JSON.
		var data = new { totalClientes, totalMascotas, totalConsultas, consultasPorMes };

		return Ok(data);
	}
	catch(Exception ex)
	{
		var respuestaError = new
		{
			error = "Ocurrio un error inesperado al procesar la solicitud."
#if DEBUG
			// Esta linea solo se ejecuta en modo DEBUG.
			// Recomendado para evitar mostrar o exponer detalles internos...
			,
			message = ex.Message
#endif
		};

		return StatusCode(500, respuestaError);
	}
}
```

2.  **Crear las Vistas Parciales:**
    - Para cada rol, crear una **vista parcial** en `Views/Home/` o `Views/Shared/`. El nombre debe empezar con un guion bajo.
      - `_AdminDashboard.cshtml`
      - `_VetDashboard.cshtml`
      - `_RecepDashboard.cshtml`
    - Cada vista parcial contendrá únicamente el HTML de su respectivo dashboard (los `div` para los gráficos, las tarjetas de KPI, etc.).

3.  **Implementar Frontend en cada Vista Parcial:**
    - Dentro de cada archivo de vista parcial, se incluirá el código JavaScript necesario para su funcionamiento.
    - Esto incluye el `fetch` para llamar a su endpoint de datos correspondiente y el código de inicialización de **ApexCharts**.
    - Esto mantiene el código de cada dashboard encapsulado y organizado.
      ```html
      <!-- Ejemplo dentro de _AdminDashboard.cshtml -->
      <div id="admin-chart-1"></div>
      <script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
      <script>
        document.addEventListener('DOMContentLoaded', function() {
          fetch('/Home/GetAdminDashboardData')
            .then(response => response.json())
            .then(data => {
              // Lógica para renderizar gráficos con ApexCharts usando 'data'
            });
        });
      </script>
      ```

4.  **Modificar la Vista Principal `Index.cshtml`:**
    - La vista en `Views/Home/Index.cshtml` se convertirá en un simple despachador.
    - Usará la directiva `@if (User.IsInRole("..."))` para determinar qué vista parcial debe renderizar.
      ```csharp
      @if (User.IsInRole("Administrador"))
      {
          <partial name="_AdminDashboard" />
      }
      else if (User.IsInRole("Veterinario"))
      {
          <partial name="_VetDashboard" />
      }
      else if (User.IsInRole("Recepcionista"))
      {
          <partial name="_RecepDashboard" />
      }
      else
      {
          // Una vista por defecto para otros roles o si no hay dashboard definido
          <h2>Bienvenido al sistema.</h2>
      }
      ```

5.  **Simplificar el Menú de Navegación:**
    - En `_LateralMenu.cshtml`, el enlace principal o de "Inicio" simplemente apuntará a `/Home/Index`. El sistema se encargará de mostrar el contenido correcto. No se necesita lógica condicional en el menú para los dashboards.
      ```html
      <a class="nav-link" asp-controller="Home" asp-action="Index">
          <i class="fas fa-fw fa-tachometer-alt"></i>
          <span>Dashboard</span>
      </a>
      ```

---
*Este documento servirá como referencia para el desarrollo de los dashboards.*