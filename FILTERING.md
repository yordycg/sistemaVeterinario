# Guía de Filtrado en ASP.NET Core MVC

Esta guía te mostrará cómo implementar una funcionalidad de filtrado en tus proyectos de ASP.NET Core MVC.

## Paso 1: Modificar el Controlador

Modifica el método `Index` de tu controlador para que acepte los parámetros `search` y `filter`. Estos parámetros se usarán para mantener el estado del filtro entre las páginas.

```csharp
public async Task<IActionResult> Index(int? pagNumber, string search, string filter)
{
    if (search == null)
    {
        search = filter;
    }
    else
    {
        pagNumber = 1;
    }

    ViewData["filtro"] = search;

    int pagSize = 10; // Define el tamaño de la página
    var source = _context.TuModelo.AsQueryable(); // Obtén la fuente de datos

    if (!string.IsNullOrEmpty(search))
    {
        // Reemplaza 'Nombre' con el campo por el que deseas filtrar
        source = source.Where(m => m.Nombre.ToLower().Contains(search.ToLower()));
    }

    return View(await PaginatedList<TuModelo>.CreateAsync(source, pagNumber ?? 1, pagSize));
}
```

## Paso 2: Modificar la Vista

Modifica tu vista `Index.cshtml` para agregar un formulario de búsqueda y actualizar los enlaces de paginación para que incluyan el parámetro del filtro.

### Formulario de Búsqueda

Agrega este código antes de la tabla para renderizar el campo de búsqueda y el botón.

```cshtml
<div class="text-end mt-3 mb-3">
    <form action="Index" method="get">
        <div class="input-group">
            <input value="@ViewData["filtro"]" name="search" type="text" class="form-control" placeholder="Buscar..." aria-label="Recipient’s username" aria-describedby="button-addon2">
            <button class="btn btn-outline-success" type="submit" id="button-addon2">Buscar</button>
        </div>
    </form>
</div>
```

### Enlaces de Paginación

Asegúrate de que tus enlaces de paginación (`Anterior`, los números de página y `Siguiente`) incluyan el parámetro `asp-route-filter` para mantener el término de búsqueda al cambiar de página.

```cshtml
<nav aria-label="...">
    <ul class="pagination pagination-circle justify-content-center">
        <li class="page-item @prev">
            <a class="page-link" asp-action="Index" asp-route-pagNumber="@(Model.PageIndex - 1)" asp-route-filter="@ViewData["filtro"]">Anterior</a>
        </li>
        @for (int i = 0; i < total; i++)
        {
            var activo = "";
            if ((i + 1) == Model.PageIndex)
            {
                activo = "active";
            }
            <li class="page-item @activo">
                <a class="page-link" asp-action="Index" asp-route-pagNumber="@(i + 1)" asp-route-filter="@ViewData["filtro"]">@(i + 1)</a>
            </li>
        }
        <li class="page-item @next">
            <a class="page-link" asp-action="Index" asp-route-pagNumber="@(Model.PageIndex + 1)" asp-route-filter="@ViewData["filtro"]">Siguiente</a>
        </li>
    </ul>
</nav>
```

Con estos cambios, tu aplicación ahora soportará el filtrado de datos en las vistas que lo implementen, manteniendo el estado del filtro durante la paginación.
