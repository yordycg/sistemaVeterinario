# Guía: Exportación a Excel con un Helper Estático

Esta guía describe los pasos para implementar una función de exportación a Excel en la aplicación utilizando una clase de ayuda (helper) con un método estático.

## Paso 1: Instalar Dependencias

Añadir los paquetes NuGet necesarios para la generación de archivos Excel y el manejo de fuentes.

- **ClosedXML:** La librería principal para crear los archivos `.xlsx`.
- **SixLabors.Fonts:** Dependencia recomendada por `ClosedXML` para el cálculo del tamaño de texto, asegurando que el auto-ajuste de columnas funcione en cualquier plataforma.

```bash
dotnet add package ClosedXML
dotnet add package SixLabors.Fonts
```

## Paso 2: Crear el Helper `static`

Crear una clase estática `ExcelExporter` en la carpeta `Helpers` con un método genérico `GenerarExcel<T>` que se encargará de toda la lógica de creación del archivo Excel.

```csharp
using ClosedXML.Excel;
using System.Reflection;

namespace sistemaVeterinario.Helpers;

/**
 * Se define como 'static' para evitar la necesidad de crear una instancia.
 */
public static class ExcelExporter
{
    /**
     * - Metodo Generic, que puede aceptar cualquier tipo de lista.
     * En este caso, puede ser un 'Clientes, Mascotas, Roles, etc.'
     * - byte[], se utiliza principalmente por eficiencia y limpieza.
     */
    public static byte[] GenerarExcel<T>(IEnumerable<T> data, string nombreHojaExel)
    {
        /**
         * Creamos un nuevo  libro de trabajo de Excel.
         * - 'using' -> nos aseguramos que los objetos se liberen de la memoria.
         */
        using (var libroTrabajo = new XLWorkbook())
        {
            // Agregar una nueva hoja de trabajo al libro, con el nombre que nos pasen.
            var hojaTrabajo = libroTrabajo.Worksheets.Add(nombreHojaExel);

            /**
             * - Usamos 'Reflection':
             *   - 'Reflection' -> mecanismo que permite a un programa examinar su propia estructura.
             *   - Se usa en este caso para concer y saber tanto sus propiedades (publicas) como sus valores,
             *   es importante ya que estamos trabajando con Generics <T>, no sabemos de antemos que vamos a recibir.
             * - Obtenemos todas las propiedades publicas del tipo 'T' (Clientes, Mascotas, etc.).
             */
            var propiedades = typeof(T).GetProperties();

            // Recorremos las 'propiedades' para obtener los encabezados de las columnas.
            for (int i = 0; i < propiedades.Length; i++)
            {
                // hojaTrabajo.Cell(fila, columna).Value
                hojaTrabajo.Cell(1, i + 1).Value = propiedades[i].Name;
            }

            //
            int indiceFila = 2; // empezar a escribir a partir de la segunda fila.

            // Recorremos cada objeto en nuestra lista de datos, por ejemplo: cada cliente...
            // Cada objeto es una FILA y las propiedades de ese objeto son las COLUMNAS.
            foreach (var item in data) // filas
            {
                for (int i = 0; i < propiedades.Length; i++) // columnas
                {
                    // Obtenemos las propiedades (columnas) del objeto que estamos recorriendo.
                    // En caso de que sea 'null', se reemplaza por un string vacio.
                    var value = propiedades[i].GetValue(item) ?? "";

                    // Asignamos el valor a la celda correspondiente.
                    hojaTrabajo.Cell(indiceFila, i + 1).Value = XLCellValue.FromObject(value);
                }
                indiceFila++; // avanzar a la siguiente fila.
            }

            // Ajustar automaticamente el ancho de todas las columnas al contenido.
            hojaTrabajo.Columns().AdjustToContents();

            /**
             * Guardar y devolver el archivo:
             * - Creamos una 'MemoryStrema', flujo de trabajo en la memoria RAM.
             * - Guardamos el libro de trabajo en ese 'stream'
             * - Devolvemos el contenido del 'stream' como un array de 'bytes'.
             * - 'stream' -> Es un flujo continuo de datos que fluyen de un origen a un destino.
             */
            using (var stream = new MemoryStream())
            {
                libroTrabajo.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}
```

## Paso 3: Usar el Helper en el Controlador

Modificar el controlador deseado para añadir una acción `ExportToExcel` que obtenga los datos, los prepare y llame al helper estático para generar y devolver el archivo.

```csharp
/**
 * Metodo que nos permite exportar una tabla a excel.
 */
public async Task<IActionResult> ExportToExcel(string search)
{
    var clientesQuery = from c in _context.Clientes select c;
    string nombreArchivo = "Clientes";

    // Verificar si se realizo una busqueda (filtro), exportar con ese filtro.
    if (!string.IsNullOrEmpty(search))
    {
        clientesQuery = clientesQuery.Where(c => c.Nombre.ToLower().Contains(search.ToLower()));
        nombreArchivo += " - " + search; // agregar el 'filtro' al nombre del archivo.
    }

    // Proyectar el resultado a un objeto anónimo con 'Select' para elegir columnas.
    var exportData = clientesQuery.Select(c => new {
        c.Run,
        c.Nombre,
        c.Telefono,
        c.Email,
        c.Direccion
        // Al no incluir otras propiedades aquí, no se exportarán.
    });

    // Ejecutar la consulta y pasarla al helper.
    var contenidoArchivo = ExcelExporter.GenerarExcel(await exportData.ToListAsync(), "Clientes");

    // Retornar el archivo para descargar.
    return File(
            contenidoArchivo,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"{nombreArchivo}.xlsx"
        );
}
```

## Paso 4: Añadir el Botón en la Vista

En el archivo `.cshtml` correspondiente, añadir un enlace `<a>` que apunte a la nueva acción `ExportToExcel`.

```csharp
// Views/Clientes/Index.cshtml
<div class="ms-auto">
    // Boton para agregar otro registro.
    <a asp-action="Create" class="btn btn-success btn-sm"><i class="bi bi-plus-lg me-2"></i>Agregar Cliente</a>
    // Boton para exportar.
    <a asp-action="ExportToExcel" asp-route-search="@ViewData["filtro"]" class="btn btn-info btn-sm"><i class="bi bi-file-earmark-excel me-2"></i>Exportar a Excel</a>
</div>
```

---

## Anexo: Personalización de Columnas Exportadas

El helper genérico exporta todas las propiedades públicas de un modelo, pero a menudo necesitamos más control. La solución para casi todos los casos es la misma: **transformar los datos usando `Select` antes de pasarlos al helper**.

Esta técnica permite tanto **incluir** datos de tablas relacionadas como **excluir** columnas no deseadas del modelo principal.

### Caso 1: Incluir Datos de Tablas Relacionadas

**Problema:** Queremos exportar una lista de `Mascotas`, pero en lugar de ver el `IdCliente`, queremos ver el nombre del dueño.

**Solución:** Usamos `Include()` para cargar los datos del cliente y `Select()` para crear una nueva estructura que incluya el nombre.

**Ejemplo Práctico en `MascotasController`:**

```csharp
public async Task<IActionResult> ExportToExcel()
{
    // 1. Usar Include() para cargar las relaciones necesarias
    var mascotasData = await _context.Mascotas
        .Include(m => m.Cliente)
        .Include(m => m.Especie)
        .ToListAsync();

    // 2. Usar Select() para transformar la lista a un formato ideal
    var exportData = mascotasData.Select(m => new {
        NombreMascota = m.Nombre,
        Dueño = m.Cliente?.Nombre, // Incluimos el nombre del cliente
        Especie = m.Especie?.Nombre, // Y el nombre de la especie
        FechaNacimiento = m.FechaNacimiento.ToString("dd-MM-yyyy")
    });

    // 3. Llamar al helper con los datos ya transformados
    var fileContents = ExcelExporter.GenerarExcel(exportData, "Mascotas");

    return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "mascotas.xlsx");
}
```

### Caso 2: Excluir Columnas del Modelo Principal

**Problema:** El modelo `Cliente` tiene muchas propiedades, pero en la tabla del Index y en el Excel solo queremos mostrar algunas, excluyendo `RunYNombre` o `Mascotas`.

**Solución:** Usamos `Select()` para especificar exactamente qué columnas queremos. Si una propiedad no está en el `Select`, no se exportará.

**Ejemplo Práctico en `ClientesController`:**

```csharp
public async Task<IActionResult> ExportToExcel(string search)
{
    var clientesQuery = from c in _context.Clientes select c;

    if (!string.IsNullOrEmpty(search))
    {
        clientesQuery = clientesQuery.Where(c => c.Nombre.ToLower().Contains(search.ToLower()));
    }

    // 1. Usar Select() para especificar las columnas a incluir
    var exportData = clientesQuery.Select(c => new {
        c.Run,
        c.Nombre,
        c.Telefono,
        c.Email,
        c.Direccion
        // Al no incluir 'RunYNombre' ni 'Mascota' aquí, no se exportarán.
    });

    // 2. Ejecutar la consulta y pasarla al helper
    var contenidoArchivo = ExcelExporter.GenerarExcel(await exportData.ToListAsync(), "Clientes");

    return File(contenidoArchivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Clientes.xlsx");
}
```
### Ventajas del Enfoque `Select`
*   **El `ExcelExporter` sigue siendo simple y genérico.** No se contamina con lógica de negocio.
*   **Control Total:** Tienes control absoluto sobre qué columnas incluir, excluir, renombrar (`NombreMascota`) y formatear.
*   **Eficiencia:** Al usar `Select` antes de `ToListAsync`, la consulta a la base de datos es más eficiente porque solo trae los datos que realmente necesitas.
