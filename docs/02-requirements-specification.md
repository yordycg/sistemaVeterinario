# Fase 2 — Especificación de Requerimientos (Sin Código)

**Proyecto**: Sistema Veterinario
**Fecha**: 13-agosto-2026
**Versión del documento**: [v1.0]

---

## 1. MVP — Funciones indispensables

¿Cuáles son las 3 funciones sin las cuales el sistema no sirve?

1. Gestión de citas.
2. Gestión de clientes/mascotas.
3. Exportación de datos a PDF/Excel.

---

## 2. Inputs / Outputs

| Dato que entra (Input)             | Dato que sale (Output) |
| ---------------------------------- | ---------------------- |
| Registrar cliente/mascota/usuario. | Exportar PDF/Excel.    |
| Citas (todos los estados)          | Exportar PDF/Excel.    |

---

## 3. Requerimientos Funcionales (RF)

**Formato**: `RF-XX`: [descripción] — Prioridad: [Alta/Media/Baja]

- **RF-01**: Gestionar usuarios del sistema - Media.
  - Criterios de aceptación:
    - [ ] Crear usuario.
    - [ ] Modificar usuario.
    - [ ] Mostrar usuario.
    - [ ] Eliminar usuario.
- **RF-02**: Gestionar clientes/mascotas del sistema - Alta.
  - Criterios de aceptación:
    - [ ] Crear cliente/mascota.
    - [ ] Modificar cliente/mascota.
    - [ ] Mostrar cliente/mascota.
    - [ ] Eliminar cliente/mascota.
- **RF-03**: Gestionar citas del sistema - Alta.
  - Criterios de aceptación:
    - [ ] Crear/modificar/mostrar/eliminar cita.
      - [ ] Al momento de crear una cita, implementar con "transaction - SQL", para evitar las dobles reservas.
    - [ ] Doble confirmación, la ultima el dia anterior.
- **RF-04**: Gestionar tratamientos del sistema - Media.
  - Criterios de aceptación:
    - [ ] Crear tratamiento.
    - [ ] Modificar tratamiento.
    - [ ] Mostrar tratamiento.
    - [ ] Eliminar tratamiento.
- **RF-05**: Gestionar roles del sistema - Baja.
  - Criterios de aceptación:
    - [ ] Crear rol.
    - [ ] Modificar rol.
    - [ ] Eliminar rol.
    - [ ] Admin - Acceso a todo el sistema.
    - [ ] Recepcionista - Acceso a: gestión de citas, gestión de clientes.
    - [ ] Veterinario - Acceso a: gestión de citas, gestión de mascotas, gestión de tratamientos.
- **RF-06**: Exportar tablas del sistema - Alta.
  - Criterios de aceptación:
    - [ ] Exportar a PDF.
    - [ ] Exportar a Excel.
- **RF-07**: Autenticación al hacer login - Alta.
  - Criterios de aceptación:
    - [ ] Verificación de correo/rut y contraseña.
    - [ ] Re-dirección al dashboard correspondiente.
- **RF-08**: Dashboards - Media.
  - Criterios de aceptación:
    - [ ] Dashboard por rol.
- **RF-09**: Historial clínico mascota - Media.
  - Criterios de aceptación:
    - [ ] Vista detallada del historial de la mascota.

---

## 4. Requerimientos No Funcionales (RNF)

**Formato**: `RNF-XX`: [descripción] — Categoría: [Rendimiento/Seguridad/Disponibilidad/Usabilidad]

- **RNF-01**: Se debe poder crear un registro (usuario, rol, tratamiento, cliente, mascota) en menos de 2 minutos - Rendimiento.
- **RNF-02**: Se debe tener passwords con hash, autorización por rol - Seguridad.
- **RNF-03**: Los registros grandes (clientes, mascotas) deben tener paginación - Usabilidad.
- **RNF-04**: Portabilidad usando contenedores - Usabilidad.
- **RNF-05**: Implementar el estándar en seguridad para la autenticación (por ejemplo: usar JWT, usar cookies, etc.) - Seguridad.

---

## 5. Historias de Usuario

> Como [usuario/rol], quiero [acción], para [beneficio].

- [ ] Como [recepcionista], quiero cree/confirme citas y cree clientes/mascotas, para que pueda desempeñar correctamente su rol.
- [ ] Como [veterinario], quiero cree/confirme citas y gestione los tratamientos, para que pueda mejorar el flujo del sistema (con gestión de citas).
- [ ] Como [admin], quiero gestione a los usuarios/roles/citas/tratamientos/exportaciones, para que pueda reparar y solucionar cualquier bug en el sistema.

---

## 6. Alcance Excluido (Out of Scope)

> Tu principal defensa contra peticiones infinitas gratis. Sé explícito.

- No incluye la gestión de pagos.
- No incluye la funcionalidad del cliente, solo los usuarios puedes acceder al sistema.

---

## 7. Procedimiento ante pedidos fuera de alcance

¿Qué pasa si el cliente pide algo fuera de este alcance a mitad de proyecto?

- [ ] Se cotiza como Fase 2 / adicional aparte
- [ ] Se cobra por hora extra: [tarifa]
- [x] Otro: Como es un proyecto para el portfolio no tiene sentido definir esto.

---

## Checklist de cierre de Fase 2

- [x] MVP definido (máx. 3 funciones)
- [x] RF y RNF documentados con criterios de aceptación
- [x] Out of Scope explícito y comunicado al cliente
- [x] Procedimiento para pedidos adicionales acordado
