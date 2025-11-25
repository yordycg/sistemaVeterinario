# Guía para Implementar Hashing de Contraseñas

Este documento describe los pasos para dejar de almacenar contraseñas en texto plano y en su lugar, utilizar un método de hashing seguro con BCrypt. Esto es una mejora de seguridad crítica para el sistema.

## Paso 1: Agregar la Librería de Hashing

Añadiremos `BCrypt.Net-Next` al proyecto, una librería popular y confiable para hashing de contraseñas en .NET.

Ejecuta el siguiente comando en tu terminal:
```sh
dotnet add package BCrypt.Net-Next
```

## Paso 2: Actualizar el Modelo `Usuario`

El hash generado por BCrypt es una cadena de 60 caracteres. El límite actual en el modelo `Usuario` es muy bajo. Debemos aumentarlo.

1.  **Abre el archivo `Models/Usuario.cs`.**
2.  **Modifica el atributo `StringLength` de la propiedad `Password`**. Auméntalo a un valor mayor, por ejemplo, 100.

    ```csharp
    // Antes:
    [StringLength(10, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 10 caracteres.")]
    public string Password { get; set; } = null!;

    // Después:
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
    public string Password { get; set; } = null!;
    ```

## Paso 3: Crear una Migración de Base de Datos

El cambio en el modelo requiere una actualización en la base de datos para que la columna `password` acepte cadenas más largas.

1.  **Crea la migración:**
    ```sh
    dotnet ef migrations add UpdatePasswordLength
    ```

2.  **Aplica la migración a la base de datos:**
    ```sh
    dotnet ef database update
    ```

## Paso 4: Implementar el Hashing al Crear y Editar Usuarios

Debemos asegurarnos de que cualquier nueva contraseña se hashee antes de guardarse en la base de datos.

1.  **Abre el archivo `Controllers/UsuariosController.cs`**.
2.  **Añade `using BCrypt.Net;`** al inicio del archivo.
3.  **Modifica la acción `Create` (POST):** Hashea la contraseña antes de guardar el nuevo usuario.

    ```csharp
    // Dentro de public async Task<IActionResult> Create(...)
    if (ModelState.IsValid)
    {
        // Hashear la contraseña antes de guardarla
        usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
        _context.Add(usuario);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    ```

4.  **Modifica la acción `Edit` (POST):** Hashea la contraseña solo si se ha modificado.

    ```csharp
    // Dentro de public async Task<IActionResult> Edit(int id, ...)
    if (ModelState.IsValid)
    {
        try
        {
            // Obtener el usuario original de la DB para comparar
            var existingUser = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == id);

            // Hashear la contraseña solo si ha cambiado
            if (existingUser != null && existingUser.Password != usuario.Password)
            {
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            }

            _context.Update(usuario);
            await _context.SaveChangesAsync();
        }
        // ... resto del código ...
    }
    ```

## Paso 5: Implementar la Verificación en el Login

Ahora, el proceso de login debe cambiar. En lugar de comparar texto plano, debe verificar el hash.

1.  **Abre el archivo `Controllers/HomeController.cs`**.
2.  **Añade `using BCrypt.Net;`** al inicio del archivo.
3.  **Modifica la acción `Login` (POST):**

    ```csharp
    // Dentro de public async Task<IActionResult> Login(string email, string password)
    // 1. Buscar al usuario solo por email
    var user = await _context.Usuarios.Include(r => r.IdRolNavigation).FirstOrDefaultAsync(u => u.Email == email);

    // 2. Verificar si el usuario existe Y si la contraseña es correcta usando BCrypt.Verify
    if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
    {
        ViewBag.Error = "Email y/o password incorrectos.";
        return View();
    }

    // ... Si la verificación es exitosa, el resto del código de login sigue igual
    ```

## Paso 6: Manejo de Contraseñas Existentes (Importante)

Las contraseñas que ya están en tu base de datos están en texto plano y **dejarán de funcionar**.

**Solución Recomendada para este Proyecto:**
Actualiza manualmente los usuarios de prueba directamente en la base de datos o créalos de nuevo a través de la aplicación para que sus contraseñas se guarden hasheadas. Para un sistema en producción, se requeriría una estrategia más compleja (como forzar un reseteo de contraseña).
