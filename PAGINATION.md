# Guía de Paginación en ASP.NET Core MVC

Esta guía te mostrará cómo implementar una paginación personalizada en tus proyectos de ASP.NET Core MVC.

## Paso 1: Crear la clase `PaginatedList`

Esta clase nos ayudará a manejar la lógica de la paginación.

Crea un nuevo archivo `PaginatedList.cs` en la carpeta `Helpers` y agrega el siguiente código:

```csharp
using Microsoft.EntityFrameworkCore;

namespace sistemaVeterinario.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public bool HasPreviuosPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
```

## Paso 2: Modificar el Controlador

Modifica el método `Index` de tu controlador para que acepte un parámetro de número de página y use la clase `PaginatedList`.

```csharp
using sistemaVeterinario.Helpers;

// ...

public class TuControlador : Controller
{
    private readonly TuDbContext _context;

    public TuControlador(TuDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? pagNumber)
    {
        int pagSize = 10; // Define el tamaño de la página
        var source = _context.TuModelo.AsQueryable(); // Obtén la fuente de datos
        return View(await PaginatedList<TuModelo>.CreateAsync(source, pagNumber ?? 1, pagSize));
    }
}
```

## Paso 3: Modificar la Vista

Modifica tu vista `Index.cshtml` para que use la clase `PaginatedList` y muestre los controles de paginación.

```cshtml
@using sistemaVeterinario.Helpers
@model PaginatedList<sistemaVeterinario.Models.TuModelo>

@{
    ViewData["Title"] = "Tu Título";
}

<h1>Tu Título</h1>

<p>
    <a asp-action="Create">Crear Nuevo</a>
</p>
<table class="table">
    <thead>
        <tr>
            <th>
                <!-- Tus encabezados de tabla -->
            </th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>
                    <!-- Tus datos de tabla -->
                </td>
            </tr>
        }
    </tbody>
</table>

@{
    var prev = !Model.HasPreviuosPage ? "disabled" : ""; // Deshabilita el botón "Anterior" si no hay página previa
    var next = !Model.HasNextPage ? "disabled" : "";     // Deshabilita el botón "Siguiente" si no hay página siguiente
    var total = Model.TotalPages;                       // Obtiene el número total de páginas
}
<nav aria-label="...">
    <ul class="pagination pagination-circle justify-content-center">
        <li class="page-item @prev">
            <a class="page-link" asp-action="Index" asp-route-pagNumber="@(Model.PageIndex - 1)">Anterior</a>
        </li>
        @for (int i = 0; i < total; i++)
        {
            var activo = "";
            if ((i + 1) == Model.PageIndex)
            {
                activo = "active";
            }
            <li class="page-item @activo">
                <a class="page-link" asp-action="Index" asp-route-pagNumber="@(i+1)">@(i+1)</a>
            </li>
        }
        <li class="page-item @next">
            <a class="page-link" asp-action="Index" asp-route-pagNumber="@(Model.PageIndex + 1)">Siguiente</a>
        </li>
    </ul>
</nav>
```

## Paso 4 (Opcional): Estilos de Paginación

Puedes agregar los siguientes estilos a tu archivo `site.css` para mejorar la apariencia de los controles de paginación.

```css
.pagination-circle .page-item .page-link {
    border-radius: 50%;
    margin: 0 2px;
}
```
