# Guía de Implementación de Soft Delete (Eliminación Lógica)

Este documento detalla el proceso para implementar la funcionalidad de "Soft Delete" (eliminación lógica) en las entidades principales del sistema. En lugar de borrar registros permanentemente de la base de datos, los marcaremos como inactivos, preservando así la integridad histórica y la referencial.

## 1. Introducción al Soft Delete

La eliminación lógica es una práctica común en el desarrollo de aplicaciones que requiere preservar datos históricos, cumplir normativas o permitir la recuperación de información. Añadiremos una columna `EsActivo` (booleano) a las entidades `Cliente`, `Mascota` y `Usuario`. Por defecto, los registros serán `true` (activos), y al "eliminar" un registro, simplemente cambiaremos este valor a `false`.

## 2. Paso a Paso para la Implementación

### Paso 2.1: Actualizar Modelos

Añade la propiedad `EsActivo` de tipo `bool` a los modelos `Cliente.cs`, `Mascota.cs` y `Usuario.cs`. Establece su valor predeterminado en `true`.

*   **Archivo:** `Models/Cliente.cs`
    ```csharp
    // ... otras propiedades
    [Required]
    public bool EsActivo { get; set; } = true;
    ```
*   **Archivo:** `Models/Mascota.cs`
    ```csharp
    // ... otras propiedades
    [Required]
    public bool EsActivo { get; set; } = true;
    ```
*   **Archivo:** `Models/Usuario.cs`
    ```csharp
    // ... otras propiedades
    [Required]
    public bool EsActivo { get; set; } = true;
    ```

### Paso 2.2: Crear y Aplicar Migración de Base de Datos

El nuevo campo `EsActivo` debe ser añadido a las tablas correspondientes en la base de datos.

1.  **Crea una nueva migración:**
    ```sh
    dotnet ef migrations add AddSoftDeleteFlags
    ```
    *   **Verifica la migración:** Abre el archivo de migración generado (`<timestamp>_AddSoftDeleteFlags.cs`) y asegúrate de que contenga sentencias `AddColumn` para `EsActivo` en las tablas `clientes`, `mascotas` y `usuarios`.

2.  **Aplica la migración a la base de datos:**
    ```sh
    dotnet ef database update
    ```

### Paso 2.3: Modificar Acciones "Eliminar" (Soft Delete)

Cambia las acciones `Delete` en los controladores para que actualicen el campo `EsActivo` en lugar de eliminar el registro físicamente.

*   **Controladores:** `ClientesController.cs`, `MascotasController.cs`, `UsuariosController.cs`

    ```csharp
    // Ejemplo para ClientesController.cs (similar para Mascotas y Usuarios)
    public async Task<IActionResult> Delete(int id)
    {
        var entidad = await _context.Clientes.FindAsync(id); // O .Mascotas.FindAsync(id) / .Usuarios.FindAsync(id)
        if (entidad == null)
        {
            return Json("notfound");
        }

        // Antes de soft-delete, verificar dependencias (opcional pero recomendado)
        // Por ejemplo, para un Cliente, verificar si tiene mascotas activas
        // if (await _context.Mascotas.AnyAsync(m => m.IdCliente == id && m.EsActivo)) { return Json("has_children"); }

        try
        {
            entidad.EsActivo = false; // Marcar como inactivo
            _context.Update(entidad); // Actualizar el estado
            await _context.SaveChangesAsync();
            return Json("ok");
        }
        catch (Exception ex)
        {
            // Log de la excepción
            return Json("error_unexpected");
        }
    }
    ```

### Paso 2.4: Modificar Acciones "Listar" (Filtrar por EsActivo)

Las acciones `Index` (y cualquier otra que liste entidades) deben filtrar por defecto los registros donde `EsActivo` es `true`.

*   **Controladores:** `ClientesController.cs`, `MascotasController.cs`, `UsuariosController.cs`

    ```csharp
    // Ejemplo para ClientesController.cs (similar para Mascotas y Usuarios)
    public async Task<IActionResult> Index(...)
    {
        // ...
        var entidades = _context.Clientes
            .Where(e => e.EsActivo) // Filtrar solo registros activos
            // .Include(...) si es necesario
            .AsQueryable();

        // Si tienes filtro de búsqueda, combínalo:
        if (!string.IsNullOrEmpty(search))
        {
            entidades = entidades.Where(e => e.Nombre.ToLower().Contains(search.ToLower()) && e.EsActivo);
        }
        // ...
        return View(await PaginatedList<Cliente>.CreateAsync(entidades, pagNumber ?? 1, pagSize));
    }
    ```

### Paso 2.5: Ajustar Acciones "Detalles" y "Editar"

Es importante que las acciones `Details` y `Edit` consideren si el registro está lógicamente eliminado.

*   **Acciones `Details` y `Edit` (GET):**
    *   Por defecto, estas acciones deberían buscar solo registros activos. Si intentan acceder a un registro `EsActivo = false`, pueden redirigir a una página de error o mostrar un mensaje.
    *   Opcionalmente, permitir que un administrador acceda a registros inactivos.

    ```csharp
    // Ejemplo para ClientesController.cs (similar para Mascotas y Usuarios)
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) { return NotFound(); }
        var entidad = await _context.Clientes.FirstOrDefaultAsync(e => e.IdCliente == id && e.EsActivo); // Solo buscar activos
        if (entidad == null)
        {
            // O bien NotFound, o redirigir a un mensaje "Registro no encontrado o eliminado"
            return NotFound();
        }
        // ...
        return View(entidad);
    }
    ```

### Paso 2.6: Consideraciones Adicionales

*   **Restaurar Registros:** Puedes añadir una funcionalidad para "restaurar" registros, cambiando `EsActivo` de `false` a `true`.
*   **UI:** La interfaz de usuario deberá reflejar estos cambios, mostrando solo los registros activos por defecto, y quizás una opción para ver "registros inactivos" para administradores.
*   **Relaciones:** Asegúrate de que, al soft-delete una entidad padre (ej. `Cliente`), consideres el impacto en sus entidades hijas (ej. `Mascotas`). Podrías querer soft-delete también las hijas, o evitar eliminar el padre si tiene hijos activos.

---

**Nota:** La implementación de la interfaz de usuario para permitir la visualización de registros inactivos o la restauración de registros requerirá cambios adicionales en las vistas y posiblemente nuevos endpoints en los controladores. Esta guía se enfoca principalmente en la lógica de backend.
