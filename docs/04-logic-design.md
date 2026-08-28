# Fase 4 — Diseño Lógico y Pseudocódigo

**Proyecto**: Sistema Veterinario
**Tamaño del proyecto**: [ ] Chico &nbsp; [x] Mediano &nbsp; [ ] Grande

> **Nivel de detalle según tamaño**:
>
> - **Chico**: solo el flujo principal, 10-15 líneas.
> - **Mediano**: flujo principal + 2-3 funciones críticas.
> - **Grande**: todas las funciones no triviales.

---

## 1. Pseudocódigo — Flujo Principal

```
START
    FUNCTION newCita(data):
        IF NOT validarPermisos(usuario_acutal, rol) THEN:
            RETURN ERROR_ACCESS;

        IF NOT validarExistencia(data.cliente.id, data.cliente.mascota.id) THEN:
            RETURN ERROR_CLIENT_ENTITY;

        IF NOT validarHora(data.solicitud.dia, data.solicitud.hora) THEN:
            RETURN ERROR_INVALID_DATE;

        IF NOT vetDisponibilidad(data.vet.id, data.solicitud.dia, data.solicitud.hora) THEN:
            RETURN ERROR_USER_CONFLICT;

        // START SQL.Transaction
        IF NOT consultarDisponibilidad(data.vet.id, data.solicitud.dia, data.solicitud.hora) THEN:
            ROLLBACK TRANSACTION;
            RETURN ERROR_BUSY;

        CREATE registroCita(datos, estado = pendiente);
        COMMIT TRANSACTION;
        RETURN SUCCESS;
        // END SQL.Transaction
END
```

---

## 2. Funciones críticas

_(Solo Mediano/Grande — agregar un bloque por cada función no trivial)_

### Función: diagnosticoTratamiento

**Propósito**: Solo un usuario de tipo "veterinario" puede diagnosticar una mascta, el contexto es la cita.

```
FUNCION nombre_funcion(parametros)
    [lógica paso a paso en español]
    RETORNAR [resultado]
FIN FUNCION
```

### Función: asignarMedicamento

**Propósito**: Solo los usuarios de tipo "veterinario" pueden recetar y asignar medicamentos a una mascota, el contexto es la cita.

```
FUNCION nombre_funcion(parametros)
    [lógica paso a paso en español]
    RETORNAR [resultado]
FIN FUNCION
```

---

## 3. Manejo de Casos de Borde y Errores

| Escenario                                       | Qué debe pasar                | Mensaje al usuario             |
| ----------------------------------------------- | ----------------------------- | ------------------------------ |
| [Ej: la API externa no responde]                | [reintento / log / abortar]   | [mensaje claro, no stacktrace] |
| [Ej: dato nulo o vacío]                         | [validación previa]           | [mensaje claro]                |
| [Ej: se corta la conexión a mitad de ejecución] | [rollback / guardado parcial] | [mensaje claro]                |

---

## 4. Estructura de Carpetas y Módulos

```
proyecto/
├── src/
│   ├── [módulo1]/
│   └── [módulo2]/
├── tests/
└── docs/
```

---

## Preguntas de autochequeo antes de pasar a Fase 5

- [ ] ¿Qué pasa si la red falla, la API está caída o la página cambió su estructura? — Respondido arriba
- [ ] ¿Cómo se manejan los errores sin romper todo el programa? — Respondido arriba
- [ ] ¿Esta lógica es fácil de entender si la leo dentro de 6 meses?
- [ ] ¿Puedo explicar este pseudocódigo a alguien no técnico en 2 minutos?
