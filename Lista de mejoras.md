# Lista de Mejoras y Detalles a Pulir en el Proyecto

Esta lista detalla áreas identificadas para mejorar la seguridad, la experiencia de usuario, la calidad del código y el rendimiento de la aplicación.

## Prioridad Alta (Seguridad y Funcionalidad Crítica)

1.  [ ] **Seguridad Crítica - Almacenamiento y Hashing de Contraseñas**
    *   **Detalle:** El esquema de la base de datos (`password varchar(10) NOT NULL`) sugiere que las contraseñas no se están almacenando de forma segura (hasheadas). Un `varchar(10)` es demasiado corto para un hash seguro.
    *   **Acción Recomendada:** Implementar un hashing de contraseñas robusto (por ejemplo, usando `ASP.NET Core Identity` o librerías como `BCrypt.Net`) y actualizar el tipo de dato de la columna `password` en la base de datos a un `varchar` de mayor longitud (ej. `varchar(255)`).

2.  [ ] **Seguridad Crítica - No Mostrar Contraseñas en `Usuarios/Index.cshtml`**
    *   **Detalle:** La vista `Usuarios/Index.cshtml` mostraba la contraseña de los usuarios. Esto es una vulnerabilidad de seguridad grave.
    *   **Estado:** **Corregido** (se eliminó la columna de la vista).

3.  [ ] **Calidad de Código - Centralización de Lógica JavaScript de Eliminación**
    *   **Detalle:** La lógica de SweetAlert2 y `fetch` para la eliminación está duplicada en cada vista `Index.cshtml`. Esto dificulta el mantenimiento y la actualización.
    *   **Acción Recomendada:** Extraer esta lógica común a una función JavaScript reutilizable en un archivo `.js` compartido (ej. `wwwroot/js/delete-handler.js`) y llamarla desde cada vista.

4.  [ ] **Calidad de Código - "Magic Strings" en Respuestas JSON del Controlador**
    *   **Detalle:** Los controladores devuelven cadenas literales como `"ok"`, `"notfound"`, `"has_children"`, `"error_db_constraint"`.
    *   **Acción Recomendada:** Definir estas respuestas como constantes o enums en C# para mejorar la legibilidad, reducir errores tipográficos y facilitar el mantenimiento.

## Prioridad Media (UX y Estructura de Código)

5.  [ ] **Calidad de Código - Consolidar Formularios `Create`/`Edit` en una Vista Parcial**
    *   **Detalle:** Las vistas `Create.cshtml` y `Edit.cshtml` de un mismo mantenedor suelen compartir la mayor parte de su estructura de formulario y lógica de validación. Duplicar este código dificulta el mantenimiento.
    *   **Acción Recomendada:** Crear una vista parcial (ej. `_Form.cshtml`) que contenga los campos comunes del formulario y la lógica de validación. Luego, `Create.cshtml` y `Edit.cshtml` renderizarían esta vista parcial.

6.  [ ] **UX - Consistencia en Iconos de Acción (Editar/Eliminar)**
    *   **Detalle:** Las vistas `Index.cshtml` de `Mascotas`, `Roles`, `Consultas` y `Tratamientos` aún utilizan texto para los botones de "Editar" y "Eliminar" en las filas de la tabla.
    *   **Acción Recomendada:** Aplicar el mismo estilo de botones con iconos (`btn-icon-action btn-outline-warning/danger btn-sm` con `bi bi-pencil/trash`) a todas las demás vistas `Index.cshtml`.

7.  [ ] **UX - Consistencia en Estilo de Encabezados de Tabla (`<thead>`)**
    *   **Detalle:** Asegurar que todos los `<thead>` en las vistas `Index.cshtml` tengan la clase `table-dark text-center` para una apariencia uniforme.
    *   **Acción Recomendada:** Verificar y aplicar `table-dark text-center` a todos los `<thead>` restantes.

8.  [ ] **UX - Usar `form-select` para elementos `<select>`**
    *   **Detalle:** Bootstrap 5 recomienda usar la clase `form-select` para los elementos `<select>` en lugar de `form-control` para un estilo más moderno y consistente.
    *   **Acción Recomendada:** Cambiar `class="form-control"` a `class="form-select"` en todos los elementos `<select>` de las vistas `Create` y `Edit`.

9.  [ ] **UX - Espaciado Consistente en Formularios (Create/Edit)**
    *   **Detalle:** Asegurar que todos los `div` con `class="form-group"` en las vistas `Create` y `Edit` de los demás controladores tengan la clase `mb-3` para un espaciado uniforme.
    *   **Acción Recomendada:** Revisar y añadir `mb-3` a todos los `div` con `class="form-group"` restantes.

10. [ ] **UX - Títulos Descriptivos en Vistas `Create`/`Edit`**
    *   **Detalle:** Los títulos genéricos como "Create" o "Edit" en `ViewData["Title"]` y en los `<h1>` de las vistas `Create`/`Edit` podrían ser más descriptivos (ej. "Crear Cliente", "Editar Mascota").
    *   **Acción Recomendada:** Asegurar que los títulos sean específicos para cada entidad y estén centrados.

## Prioridad Baja (Rendimiento y Funcionalidades Amplias)

11. [ ] **Seguridad - Validación de Entrada (Server-Side)**
    *   **Detalle:** Aunque hay validación del lado del cliente y se usa `ModelState.IsValid`, es crucial asegurar que todos los modelos tengan atributos de validación (`[Required]`, `[StringLength]`, `[EmailAddress]`, etc.) adecuados para una validación robusta del lado del servidor.
    *   **Acción Recomendada:** Realizar una revisión exhaustiva de los modelos para asegurar que todas las propiedades tengan los atributos de validación necesarios.

12. [ ] **Rendimiento y UX - Paginación, Ordenamiento y Filtrado de Tablas**
    *   **Detalle:** Las tablas actualmente muestran todos los registros. Para grandes volúmenes de datos, esto puede ser lento y poco usable.
    *   **Acción Recomendada:** Implementar una solución de paginación, ordenamiento y filtrado para las tablas (ej. usando una librería JavaScript como DataTables.js o implementando lógica de servidor).
