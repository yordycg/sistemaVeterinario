# Tareas — Rebuild v2 (Sistema Veterinario)

> Tracker del rebuild para portfolio, basado en el framework de fases del desarrollador
> (Fase 0 — Prospección/Cotización — omitida: no es un proyecto a la venta).
> Fases 1–4 en `main`; rama `v2` para Fase 5 en adelante, merge a `main` al final.

## Cómo usar

- `[ ]` pendiente · `[x]` completada
- Tareas accionables en 1–2 horas, numeradas por fase (1.1, 1.2…)
- Marcar las tareas en el commit/PR que las resuelve

---

## FASE 1: Descubrimiento y Definición del Problema

**Objetivo**: Entender el problema real del dominio veterinario antes de proponer tecnología.

- [x] 1.1 Identificar el "dolor principal" que resuelve el sistema (proceso manual/ineficiente)
- [x] 1.2 Analizar cómo se gestiona hoy el problema sin software
- [x] 1.3 Definir los usuarios y su nivel técnico (Admin, Recepcionista, Veterinari@)
- [x] 1.4 Definir restricciones (tiempo de estudio disponible, limitaciones técnicas)
- [x] 1.5 Determinar los Criterios de Éxito del portfolio

### Preguntas claves

1. ¿Cuál es el problema real que estamos resolviendo? (sin mencionar software).
2. ¿Qué pasa si NO hacemos este proyecto?
3. ¿Quién decide el alcance final?

## FASE 2: Especificación de Requerimientos (Sin Código)

**Objetivo**: Aterrizar el problema en funcionalidades concretas y definir el scope.

- [x] 2.1 Inventario funcional del proyecto actual (módulos que ya existen)
- [x] 2.2 Requerimientos Funcionales (RF): lo que el sistema debe hacer
- [x] 2.3 Requerimientos No Funcionales (RNF): seguridad, rendimiento, disponibilidad
- [x] 2.4 Historias de usuario: "Como [rol], quiero [acción], para [beneficio]"
- [x] 2.5 Definir el Alcance Excluido (Out of Scope)
- [x] 2.6 Decidir features nuevas: agregar / quitar / mejorar de lo actual

### Preguntas claves

1. ¿Cuáles son las 3 funciones indispensables (MVP)?
2. ¿Qué datos entran al sistema (Inputs) y qué datos salen (Outputs)?
3. ¿Qué cosas quedan EXCLUIDAS explícitamente de esta versión?

## FASE 3: Arquitectura y Modelado (Diagramas & Flujos)

**Objetivo**: Dibujar el mapa del sistema antes de escribir una sola línea de código.

- [x] 3.1 Diagrama de Flujo / Proceso: camino de los datos de entrada a salida
- [x] 3.2 Diseñar el DER / Modelo de Datos (tablas, campos, relaciones) en Mermaid
- [x] 3.3 Resolver la doble fuente de verdad de la BD (migraciones vs schema.sql)
- [x] 3.4 Decisiones de capas/estructura (MVC plano vs. Services/ViewModels/DTOs)
- [x] 3.5 Estructura de carpetas y módulos del proyecto final
- [x] 3.6 Generar `docs/03-architecture.md`

### Preguntas claves

1. ¿Cómo viaja la información desde el origen hasta el destino?
2. ¿Dónde se almacena la información y qué estructura tiene?
3. ¿Qué módulos o librerías independientes necesito?

## FASE 4: Diseño Lógico y Pseudocódigo

**Objetivo**: Resolver la lógica pesada en español simple, antes de la sintaxis del lenguaje.

- [ ] 4.1 Pseudocódigo del flujo principal (login, CRUD genérico, dashboard)
- [ ] 4.2 Manejo de Casos de Borde y Errores (datos nulos, BD caída, timeouts)
- [ ] 4.3 Pseudocódigo de búsqueda/filtros/paginación y exportaciones
- [ ] 4.4 Revisión socrática del coach sobre el pseudocódigo
- [ ] 4.5 Generar `docs/04-pseudocodigo.md`

### Preguntas claves

1. ¿Qué pasa si la BD falla o la conexión se corta a mitad de operación?
2. ¿Cómo manejo errores sin que el programa se rompa?
3. ¿Entendería este pseudocódigo dentro de 6 meses?

## FASE 5: Construcción / Codificación

**Objetivo**: Traducir la arquitectura y el pseudocódigo a código real de producción.

- [ ] 5.1 Configurar entorno: rama `v2`, `.gitignore`, esqueleto del proyecto
- [ ] 5.2 Configurar `.editorconfig`, analyzers y `dotnet format`
- [ ] 5.3 Gestión segura de variables: `.env.example` + `user-secrets` (nunca hardcodear)
- [ ] 5.4 Desarrollo iterativo: entrada (datos) → lógica → salida (vistas)
- [ ] 5.5 Implementar auth y claims (roles, anti-CSRF, hashing)
- [ ] 5.6 Implementar CRUD de cada entidad
- [ ] 5.7 Implementar búsqueda, filtros y paginación
- [ ] 5.8 Implementar dashboards por rol
- [ ] 5.9 Implementar exportaciones PDF / Excel
- [ ] 5.10 Implementar features nuevas acordadas en 2.6
- [ ] 5.11 `Dockerfile` multi-stage + `docker-compose` (web + mssql)
- [ ] 5.12 Commits pequeños y descriptivos a medida que avanzas

### Preguntas claves

1. ¿Estoy cumpliendo exactamente el pseudocódigo o me estoy desviando a inventar cosas?
2. ¿Tengo credenciales/claves privadas hardcodeadas en el código fuente?

## FASE 6: Pruebas y Control de Calidad (Testing & QA)

**Objetivo**: Garantizar que el código funciona en escenarios reales y no solo en tu máquina.

- [ ] 6.1 Pruebas de Camino Feliz (Happy Path)
- [ ] 6.2 Pruebas con Datos Destructivos / Erróneos (nulos, formatos, cortar BD)
- [ ] 6.3 Pruebas de Rendimiento Básicas (10 vs. 10.000 registros)
- [ ] 6.4 Unit tests en funciones críticas (paginación, validaciones, agregaciones)
- [ ] 6.5 Ejecutar `dotnet format` y verificar linter
- [ ] 6.6 Documentar resultados

### Preguntas claves

1. ¿Qué pasa si el usuario ingresa un dato inválido?
2. ¿El programa muestra un error claro o se cae con una excepción incomprensible?

## FASE 7: Despliegue, Documentación, Entrega y Post-Entrega

**Objetivo**: Entregar el proyecto de forma profesional y cerrar el ciclo del portfolio.

- [ ] 7.1 Escribir el `README.md` ejecutable en <10 minutos
- [ ] 7.2 Verificar artefactos Docker a la primera (limpiar y recrear)
- [ ] 7.3 Demo: capturas de pantalla del sistema por rol
- [ ] 7.4 Revisión final: ¿se cumplen los Criterios de Éxito de la Fase 1?
- [ ] 7.5 Merge de `v2` a `main`
- [ ] 7.6 Post-entrega: definir la "siguiente fase" (features futuras para el portfolio)

### Preguntas claves

1. ¿Si alguien toma este repositorio hoy, lo ejecuta en menos de 10 minutos con el `README.md`?
2. ¿Cumplí todos los criterios de éxito definidos en la Fase 1?
3. ¿Dejé claro qué sigue después de esta entrega?
