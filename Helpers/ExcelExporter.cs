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
