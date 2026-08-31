# Fase 4 — Diseño Lógico y Pseudocódigo

**Proyecto**: Sistema Veterinario

**Tamaño del proyecto**: [ ] Chico &nbsp; [x] Mediano &nbsp; [ ] Grande

---

## 1. Pseudocódigo — Flujo Principal

```
START
    FUNCTION newCita(data):
        IF NOT validar_permisos(usuario_acutal, rol) THEN:
            RETURN ERROR_ACCESS;

        IF NOT validar_existencia(data.cliente.id, data.cliente.mascota.id) THEN:
            RETURN ERROR_CLIENT_ENTITY;

        IF NOT validar_hora(data.solicitud.dia, data.solicitud.hora) THEN:
            RETURN ERROR_INVALID_DATE;

        // START SQL.Transaction
        IF NOT consultar_disponibilidad(data.vet.id, data.solicitud.dia, data.solicitud.hora) THEN:
            ROLLBACK TRANSACTION;
            RETURN ERROR_BUSY;

        CREATE registro_cita(datos, estado = pendiente);
        COMMIT TRANSACTION;
        RETURN SUCCESS;
        // END SQL.Transaction
    END FUNCTION
END
```

---

## 2. Funciones críticas

### Función: diagnostico_tratamiento

**Propósito**: Solo un usuario de tipo "veterinario" puede diagnosticar una mascota, asignar su tratamiento o su medicamento si corresponde, el contexto es la cita.

```
FUNCTION diagnostico_tratamiento(data)
    IF NOT validar_permiso(data.usuario_actual, rol) THEN;
        RETURN ERROR_ACCESS;
    IF NOT validar_existencia_cliente(data.cliente.id, data.cliente.mascota.id) THEN:
        RETURN ERROR_CLIENT_ENTITY;

    // hacer diagnostico...

END FUNCTION
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
