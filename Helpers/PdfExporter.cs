using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace sistemaVeterinario.Helpers
{
    public static class PdfExporter
    {
        public static byte[] GenerarPdf<T>(IEnumerable<T> data, string titulo, string nombreHojaTrabajo = "Lista")
        {
            // Activar licencia comunitaria
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        // .PaddingBottom(1, Unit.Centimetre)
                        .Text(titulo)
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium)
                        .AlignCenter();

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(5); // Espacio entre elementos de la columna

                            column.Item().AlignRight().Text(text =>
                            {
                                text.Span("Fecha del Reporte: ").SemiBold();
                                text.Span(DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
                            });

                            column.Item().PaddingTop(5).Table(table =>
                            {
                                // Construcción Dinámica de la Tabla
                                var propiedades = typeof(T).GetProperties();

                                // Header de la tabla
                                table.ColumnsDefinition(columns =>
                                {
                                    foreach (var prop in propiedades)
                                    {
                                        columns.RelativeColumn(); // ancho relativo para cada columna.
                                    }
                                });

                                // Estilos del header
                                table.Header(header =>
                                {
                                    foreach (var prop in propiedades)
                                    {
                                        header.Cell().Padding(5).DefaultTextStyle(x =>
                                        {
                                            return x.SemiBold().FontSize(12).FontColor(Colors.White);
                                        }).Background(Colors.Blue.Medium).AlignCenter().Text(prop.Name);
                                    }
                                });

                                // Filas de datos
                                foreach (var item in data)
                                {
                                    foreach (var prop in propiedades)
                                    {
                                        var value = prop.GetValue(item)?.ToString() ?? "";
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(value);
                                    }
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

            return document.GeneratePdf(); // genera el PDF y lo retorna como byte[].
        }
    }
}
