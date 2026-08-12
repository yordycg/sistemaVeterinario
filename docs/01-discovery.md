# Fase 1 — Descubrimiento y Definición del Problema (Discovery)

**Proyecto**: Sistema Veterinario

**Fecha**: 12-agosto-2026

**Participantes de la reunión**:

- Yordy Carmona / Developer

---

## 1. Dolor principal

¿Qué proceso manual o ineficiente está afectando al cliente?

La forma en que una clínica veterinaria esta gestionando a sus clientes y pacientes,
es todo de forma manual, registros, citas, etc.

---

## 2. Cómo se resuelve hoy (sin el software)

Un cliente se contacta con la clínica (presencial o remota) para pedir una hora para un animal
(generalmente su mascota), esta cita depende de la situación: si el cliente puede movilizar o
no al animal o el veterinario debe movilizar todo su equipo.

El/la recepcionista es la encargada de administrar todo el proceso, donde agenda un dia y
hora para esa cita, el dia anterior a la cita se comunica con el cliente para confirmar la hora,
todo esto proceso es gestionado de forma manual.

---

## 3. Usuarios del sistema

| Rol/Usuario   | Nivel técnico | Impacto                       | Qué necesita hacer en el sistema                                 |
| ------------- | ------------- | ----------------------------- | ---------------------------------------------------------------- |
| Administrador | Alto          | Necesita una buena UX.        | Gestiona todas las funcionalidades del sistema.                  |
| Recepcionista | Básico        | Necesita una detallada UI/UX. | Se encarga de gestionar las citas y de gestionar a los clientes. |
| Veterinario   | Básico        | Necesita una detallada UI/UX. | Se encarga de gestionar las citas, su núcleo es lo clínico.      |

---

## 4. Restricciones

- **Tiempo**: 2 meses
- **Presupuesto**: 0 $
- **Limitaciones técnicas**:
  - .NET 9
  - MVC
  - SQL Server

---

## 5. Criterios de éxito

¿Cómo sabremos que el proyecto fue un éxito? (medible, no vago)

- El proyecto cuenta con una base solida en cuanto a infraestructura necesaria para su funcionamiento:
  - Cuenta con un `compose.yml`, `Dockerfile`, etc.
- Gestión de citas y Gestión de clientes:
  - Antes 10 minutos ahora 2 minutos.

---

## 6. Qué pasa si NO hacemos este proyecto

El no poder completar este proyecto la clínica veterinaria no va a poder agilizar todos su proceso.

---

## 7. Toma de decisión

- **Quién decide finalmente**:
  - Yordy Carmona / Stakeholder
- **Quién más debe aprobar**:
  - Yordy Carmona / Product Owner

---

## Checklist de cierre de Fase 1

- [x] Dolor principal identificado y validado con el cliente
- [x] Restricciones documentadas
- [x] Criterios de éxito definidos y medibles
- [x] Decidor(es) identificados
