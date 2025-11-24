# Guía: Exportación a PDF con un Helper Estático

Esta guía describe los pasos para implementar una función de exportación a PDF en la aplicación utilizando una clase de ayuda (helper) con un método estático, basándose en la librería **QuestPDF**.

## Objetivo:
Crear un servicio reutilizable (`PdfExporter`) que pueda tomar cualquier lista de datos (`IEnumerable<T>`) y la exporte a un archivo PDF, manteniendo la flexibilidad y la capacidad de personalización del contenido.

---

## Paso 1: Instalar Dependencias

Añadir el paquete NuGet de QuestPDF. Este paquete `QuestPDF` incluye el motor de renderizado necesario para generar los archivos PDF.

```bash
dotnet add package QuestPDF
```

## Paso 2: Crear el Helper Estático para PDF

Crear una clase estática `PdfExporter` en la carpeta `Helpers`. Esta clase contendrá un método estático y genérico `GenerarPdf<T>` que se encargará de toda la lógica de creación del archivo PDF.

A diferencia de Excel, donde simplemente listábamos propiedades, QuestPDF se basa en una API fluida para definir el diseño del documento. Nuestro helper deberá construir dinámicamente la tabla dentro del PDF.

### **Ejemplo de la estructura del helper (`Helpers/PdfExporter.cs`):**

```csharp
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection; // Todavía útil para leer propiedades de T

namespace sistemaVeterinario.Helpers
{
    public static class PdfExporter
    {
        public static byte[] GenerarPdf<T>(IEnumerable<T> data, string title, string worksheetName = "Reporte")
        {
            // Opcional: Registrar licencia comunitaria si no vas a usar la versión comercial
            // License.LicenseType = LicenseType.Community; // Descomentar si usas la licencia comunitaria

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2.54f, Unit.Centimetre); // 1 pulgada de margen
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text(title)
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium)
                        .AlignCenter()
                        .PaddingBottom(1, Unit.Centimetre);

                    page.Content()
                        .PaddingVertical(0.5f, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(5); // Espacio entre elementos de la columna

                            column.Item().Text(text =>
                            {
                                text.Span("Fecha del Reporte: ").SemiBold();
                                text.Span(DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
                            }).FontSize(8).AlignRight();

                            column.Item().PaddingTop(10).Table(table =>
                            {
                                // --- CLAVE: Construcción Dinámica de la Tabla ---
                                var properties = typeof(T).GetProperties();

                                // Cabecera de la tabla
                                table.Columns(columns =>
                                {
                                    foreach (var prop in properties)
                                    {
                                        columns.RelativeColumn(); // Cada columna tiene un ancho relativo
                                    }
                                });

                                table.Header(header =>
                                {
                                    header.DefaultTextStyle(x => x.SemiBold().FontSize(10).FontColor(Colors.White));
                                    header.Background(Colors.Blue.Medium);
                                    header.AlignCenter();

                                    foreach (var prop in properties)
                                    {
                                        header.Cell().Padding(5).Text(prop.Name);
                                    }
                                });

                                // Filas de datos
                                foreach (var item in data)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Column(cell =>
                                    {
                                        foreach (var prop in properties)
                                        {
                                            var value = prop.GetValue(item)?.ToString() ?? "";
                                            cell.Item().Padding(5).Text(value);
                                        }
                                    });
                                }
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Página ").FontSize(8);
                            x.CurrentPageNumber().FontSize(8);
                            x.Span(" de ").FontSize(8);
                            x.TotalPages().FontSize(8);
                        });
                });
            });

            return document.GeneratePdf(); // Genera el PDF y lo devuelve como byte[]
        }
    }
}
```

## Paso 3: Usar el Helper en el Controlador

El uso en el controlador será muy similar al de Excel. Deberás:
1.  Obtener los datos y transformarlos con `Select` (si es necesario para claves foráneas o formateo).
2.  Llamar al método `PdfExporter.GenerarPdf()`.
3.  Devolver el `byte[]` resultante como un `FileContentResult` con el tipo MIME `application/pdf`.

### **Ejemplo en `ClientesController.cs` (método `ExportToPdf`):**

```csharp
// GET: Clientes/ExportToPdf
public async Task<IActionResult> ExportToPdf(string search)
{
    var clientesQuery = from c in _context.Clientes select c;
    string nombreArchivo = "Clientes";

    if (!string.IsNullOrEmpty(search))
    {
        clientesQuery = clientesQuery.Where(c => c.Nombre.ToLower().Contains(search.ToLower()));
        nombreArchivo += $"_{search}";
    }

    // Usamos el mismo Select que para Excel, o uno adaptado si el PDF necesita algo distinto
    var exportData = await clientesQuery.Select(c => new {
        c.Run,
        c.Nombre,
        c.Telefono,
        c.Email,
        c.Direccion
    }).ToListAsync();

    // Llamamos al helper de PDF
    var contenidoArchivo = PdfExporter.GenerarPdf(exportData, "Reporte de Clientes"); // Título del reporte

    return File(
        contenidoArchivo,
        "application/pdf", // Tipo MIME para PDF
        $"{nombreArchivo}.pdf"
    );
}
```

## Paso 4: Añadir el Botón en la Vista

Añadir un botón en la vista (`Index.cshtml`) que apunte a la nueva acción `ExportToPdf`.

### **Ejemplo en `Views/Clientes/Index.cshtml`:**

```html
<a asp-action="ExportToPdf" asp-route-search="@ViewData["filtro"]" class="btn btn-danger btn-sm"><i class="bi bi-file-earmark-pdf me-2"></i>Exportar a PDF</a>
```

---

## Anexo: Personalización Avanzada del PDF con QuestPDF

La API de QuestPDF es muy potente para personalizar el diseño del documento:

*   **Diseño Fluido:** Puedes definir estructuras complejas (columnas, filas, tablas anidadas, imágenes, gráficos) directamente en el código.
*   **Componentes Reutilizables:** Puedes crear componentes de diseño (ej. una tarjeta de producto) y reutilizarlos en diferentes partes del documento.
*   **Estilos:** Control total sobre fuentes, colores, tamaños, alineaciones, bordes, etc.
*   **Encabezados y Pies de Página:** Fáciles de definir para que aparezcan en cada página.

La clave está en modificar la lógica dentro del `Document.Create(...)` de tu `PdfExporter.cs` para ajustarla a tus necesidades de diseño específicas, más allá de una tabla simple.
