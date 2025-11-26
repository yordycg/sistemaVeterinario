# Guía de Pruebas de Estilo

Entendido. Este documento describe dos **pruebas independientes** para explorar diferentes enfoques de estilo para la aplicación. No son fases consecutivas, sino dos caminos distintos que podemos evaluar.

---

### Prueba A: Reconstrucción Funcional solo con Bootstrap

**Objetivo:** Evaluar cómo se ve y funciona la aplicación utilizando *únicamente* el framework de Bootstrap 5, sin ningún estilo personalizado o de la plantilla actual. Esto nos dará una línea de base limpia y estándar.

**Plan de Acción:**
1.  **Limpiar Estilos Actuales:** En `Views/Shared/_Layout.cshtml`, comentaremos los `link` a `template.css`, `site.css` y `sistemaVeterinario.styles.css`.
2.  **Añadir Bootstrap Puro:** Agregaremos los enlaces a las versiones más recientes de Bootstrap CSS y JS desde su CDN oficial.
3.  **Reestructurar el Layout:** El paso más importante. El layout actual depende de clases como `admin-wrapper` y `admin-sidebar` que desaparecerán. Reconstruiremos la estructura de la página usando componentes estándar de Bootstrap:
    -   El cuerpo principal se organizará con el sistema de Grid de Bootstrap (`<div class="row">`, `<div class="col-md-3">`, etc.).
    -   El `_LateralMenu.cshtml` se colocará dentro de una de estas columnas para que actúe como una barra lateral fija.
    -   El header y el contenido principal ocuparán la otra columna.
4.  **Verificación:** Revisaremos que la navegación, las tablas y los formularios sigan siendo funcionales, aunque su apariencia sea la de Bootstrap por defecto.

**Resultado Esperado:** Una aplicación funcional, responsiva, pero visualmente muy simple (blanco y azul por defecto), demostrando la base que Bootstrap ofrece sin adornos.

---

### Prueba B: Aplicar un Tema Personalizado sobre el Layout Actual

**Objetivo:** Ver si podemos lograr la estética "calmado y pastel" deseada aplicando una nueva capa de estilos *sobre* la estructura y los archivos CSS que ya existen. Esta es una opción menos disruptiva.

**Plan de Acción:**
1.  **Mantener Estilos Actuales:** No tocaremos los enlaces existentes a `template.css` o `site.css`. La estructura de la página no se altera.
2.  **Crear un Archivo de Tema:** Crearemos un nuevo archivo en `wwwroot/css/theme-pastel.css`.
3.  **Cargar el Nuevo Tema:** Añadiremos un `link` a `theme-pastel.css` en `_Layout.cshtml`. Es crucial que se cargue **después** de `template.css` y `site.css` para que nuestras nuevas reglas puedan sobrescribir las anteriores.
4.  **Implementar Estilos del Tema:** En `theme-pastel.css`, escribiremos reglas de CSS para:
    -   Sobrescribir colores de fondo, de texto y de enlaces.
    -   Cambiar la fuente de los encabezados a una "soft serif" importada de Google Fonts.
    -   Ajustar `border-radius` y `box-shadow` en componentes clave como tarjetas, botones y el menú lateral.
    -   Modificar los colores primarios de Bootstrap (`--bs-primary`, etc.) para que los componentes interactivos adopten el nuevo tema.

**Resultado Esperado:** La misma aplicación y layout de siempre, pero con una nueva "piel". Los colores, fuentes y detalles visuales se habrán transformado a la estética "calmado y pastel".

---

Por favor, indícame cuál de las dos pruebas te gustaría realizar primero: **Prueba A** o **Prueba B**.