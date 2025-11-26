-- SQL Script para Limpiar y Rellenar un Gran Volumen de Datos de Ejemplo
-- Versión 3, Final, Post-SoftDelete
-- Este script asume que la estructura de la base de datos y los datos estáticos ya han sido aplicados.
-- El usuario admin (ID 1) ya está sembrado por migración.

-- Advertencia: Esto eliminará todos los datos de las tablas especificadas.

PRINT 'Iniciando la limpieza de datos dinámicos existentes...'

-- 1. Eliminar datos de tablas en orden de dependencia inversa
DELETE FROM tratamientos;
DELETE FROM consultas;
DELETE FROM mascotas;
DELETE FROM usuarios WHERE id_usuario > 1; -- No borra el admin principal
DELETE FROM clientes;

PRINT 'Limpieza de datos dinámicos completada.'
PRINT 'Insertando un gran volumen de datos de ejemplo...'

-- ===============================================
-- Datos para Usuarios (10 en total, incluyendo Admin)
-- ===============================================
SET IDENTITY_INSERT usuarios ON;
INSERT INTO usuarios (id_usuario, id_rol, id_estado_usuario, nombre, email, password, FechaRegistro, EsActivo) VALUES
(2, 2, 1, 'Dra. Ana López', 'ana.lopez@vet.com', '$2a$11$0wG.xW4a8fW2gH8kL9oP.uY1zM2n3o4p5q6r7s8t9u0v1w2x3y4z5', '2023-01-10', 1),
(3, 2, 1, 'Dr. Carlos Mena', 'carlos.mena@vet.com', '$2a$11$0wG.xW4a8fW2gH8kL9oP.uY1zM2n3o4p5q6r7s8t9u0v1w2x3y4z5', '2023-02-15', 1),
(4, 3, 1, 'Sofía Recepción', 'sofia.r@vet.com', '$2a$11$aB8.cD4e5fF6gH7iJ8kL.mN1o2p3q4r5s6t7u8v9w0x1y2z3a4b5c6', '2023-03-20', 1),
(5, 3, 1, 'Martín Recepción', 'martin.r@vet.com', '$2a$11$aB8.cD4e5fF6gH7iJ8kL.mN1o2p3q4r5s6t7u8v9w0x1y2z3a4b5c6', '2023-04-25', 1),
(6, 2, 2, 'Dr. Pedro Inactivo', 'pedro.inactivo@vet.com', '$2a$11$0wG.xW4a8fW2gH8kL9oP.uY1zM2n3o4p5q6r7s8t9u0v1w2x3y4z5', '2023-05-30', 1),
(7, 1, 1, 'Super Admin', 'super@vet.com', '$2a$11$0wG.xW4a8fW2gH8kL9oP.uY1zM2n3o4p5q6r7s8t9u0v1w2x3y4z5', '2023-06-01', 1),
(8, 2, 1, 'Dra. Laura Torres', 'laura.torres@vet.com', '$2a$11$0wG.xW4a8fW2gH8kL9oP.uY1zM2n3o4p5q6r7s8t9u0v1w2x3y4z5', '2023-07-15', 1),
(9, 2, 1, 'Dr. Javier Ríos', 'javier.rios@vet.com', '$2a$11$0wG.xW4a8fW2gH8kL9oP.uY1zM2n3o4p5q6r7s8t9u0v1w2x3y4z5', '2023-08-20', 1),
(10, 3, 2, 'Andrea Eliminada', 'andrea.eliminada@vet.com', '$2a$11$aB8.cD4e5fF6gH7iJ8kL.mN1o2p3q4r5s6t7u8v9w0x1y2z3a4b5c6', '2023-09-01', 0); -- Estado 2 = Inactivo
SET IDENTITY_INSERT usuarios OFF;

-- ===============================================
-- Datos para Clientes (30)
-- ===============================================
SET IDENTITY_INSERT clientes ON;
INSERT INTO clientes (id_cliente, run, nombre, telefono, email, direccion, FechaRegistro, EsActivo) VALUES
(1, '11111111-1', 'Juan Pérez', '911111111', 'juan.perez@mail.com', 'Calle Falsa 123', '2023-11-15', 1),
(2, '22222222-2', 'María García', '922222222', 'maria.garcia@mail.com', 'Avenida Siempre Viva 456', '2023-11-20', 1),
(3, '33333333-3', 'Pedro López', '933333333', 'pedro.lopez@mail.com', 'Pasaje Secreto 789', '2023-12-01', 1),
(4, '44444444-4', 'Luisa Martínez', '944444444', 'luisa.martinez@mail.com', 'Boulevard de los Sueños Rotos 101', '2023-12-12', 1),
(5, '55555555-5', 'Andrés Soto', '955555555', 'andres.soto@mail.com', 'Calle Luna 246', '2024-01-05', 1),
(6, '66666666-6', 'Carla Núñez', '966666666', 'carla.nunez@mail.com', 'Avenida Sol 357', '2024-01-15', 1),
(7, '77777777-7', 'Jorge Reyes', '977777777', 'jorge.reyes@mail.com', 'Plaza Mayor 1', '2024-02-02', 1),
(8, '88888888-8', 'Fernanda Castro', '988888888', 'fernanda.castro@mail.com', 'Calle del Río 8', '2024-02-18', 1),
(9, '99999999-9', 'Ricardo Morales', '999999999', 'ricardo.morales@mail.com', 'Avenida del Mar 12', '2024-03-05', 1),
(10, '10101010-1', 'Camila Flores', '910101010', 'camila.flores@mail.com', 'Paseo del Bosque 23', '2024-03-20', 1),
(11, '12121212-1', 'Sebastián Rojas', '912121212', 'sebastian.rojas@mail.com', 'Calle del Monte 45', '2024-04-01', 1),
(12, '13131313-1', 'Valentina Ortiz', '913131313', 'valentina.ortiz@mail.com', 'Avenida de la Montaña 67', '2024-04-10', 1),
(13, '14141414-1', 'Matías Silva', '914141414', 'matias.silva@mail.com', 'Plaza de la Luna 89', '2024-04-22', 1),
(14, '15151515-1', 'Isidora Valenzuela', '915151515', 'isidora.valenzuela@mail.com', 'Calle de las Estrellas 100', '2024-05-03', 1),
(15, '16161616-1', 'Benjamín Castillo', '916161616', 'benjamin.castillo@mail.com', 'Avenida del Ocaso 200', '2024-05-15', 1),
(16, '17171717-1', 'Antonia Bravo', '917171717', 'antonia.bravo@mail.com', 'Pasaje de la Brisa 300', '2024-05-25', 1),
(17, '18181818-1', 'Agustín Tapia', '918181818', 'agustin.tapia@mail.com', 'Calle del Alba 400', '2024-06-05', 1),
(18, '19191919-1', 'Renata Herrera', '919191919', 'renata.herrera@mail.com', 'Avenida del Crepúsculo 500', '2024-06-18', 1),
(19, '20202020-2', 'Tomás Figueroa', '920202020', 'tomas.figueroa@mail.com', 'Plaza del Sol 600', '2024-07-01', 1),
(20, '21212121-2', 'Javiera Paredes', '921212121', 'javiera.paredes@mail.com', 'Calle de la Noche 700', '2024-07-12', 1),
(21, '23232323-2', 'Lucas Donoso', '923232323', 'lucas.donoso@mail.com', 'Calle de la Aurora 800', '2024-07-25', 1),
(22, '24242424-2', 'Sofía Campos', '924242424', 'sofia.campos@mail.com', 'Avenida del Amanecer 900', '2024-08-05', 1),
(23, '25252525-2', 'Vicente Araya', '925252525', 'vicente.araya@mail.com', 'Plaza del Rocío 1000', '2024-08-18', 1),
(24, '26262626-2', 'Amanda Riquelme', '926262626', 'amanda.riquelme@mail.com', 'Calle del Viento 1100', '2024-09-01', 1),
(25, '27272727-2', 'Diego Pizarro', '927272727', 'diego.pizarro@mail.com', 'Avenida de la Lluvia 1200', '2024-09-14', 1),
(26, '28282828-2', 'Florencia Orellana', '928282828', 'florencia.orellana@mail.com', 'Paseo de las Nubes 1300', '2024-09-28', 1),
(27, '29292929-2', 'Joaquín Olivares', '929292929', 'joaquin.olivares@mail.com', 'Calle del Trueno 1400', '2024-10-10', 1),
(28, '30303030-3', 'Emilia San Martín', '930303030', 'emilia.sanmartin@mail.com', 'Avenida del Relámpago 1500', '2024-10-22', 1),
(29, '31313131-3', 'Maximiliano Soto', '931313131', 'maximiliano.soto@mail.com', 'Plaza de la Tormenta 1600', '2024-11-05', 1),
(30, '32323232-3', 'Martina Vega', '932323232', 'martina.vega@mail.com', 'Calle del Arcoiris 1700', '2024-11-18', 0); -- Cliente inactivo
SET IDENTITY_INSERT clientes OFF;

-- ===============================================
-- Datos para Mascotas (35)
-- ===============================================
SET IDENTITY_INSERT mascotas ON;
INSERT INTO mascotas (id_mascota, id_cliente, id_raza, nombre, sexo, edad, FechaRegistro, EsActivo) VALUES
(1, 1, 1, 'Fido', 'M', 5, '2023-11-16', 1), (2, 1, 3, 'Mishi', 'F', 3, '2023-11-17', 1),
(3, 2, 2, 'Princesa', 'F', 2, '2023-11-21', 1), (4, 3, 8, 'Max', 'M', 7, '2023-12-02', 1),
(5, 4, 10, 'Tortuguita', 'F', 10, '2023-12-13', 1), (6, 5, 1, 'Rocky', 'M', 4, '2024-01-06', 1),
(7, 6, 4, 'Pelusa', 'F', 1, '2024-01-16', 1), (8, 7, 6, 'Tambor', 'M', 2, '2024-02-03', 1),
(9, 8, 7, 'Pipo', 'M', 3, '2024-02-19', 1), (10, 9, 9, 'Leo', 'M', 5, '2024-03-06', 1),
(11, 10, 11, 'Nemo', 'M', 1, '2024-03-21', 1), (12, 1, 8, 'Sasha', 'F', 6, '2024-03-25', 1),
(13, 2, 1, 'Buddy', 'M', 8, '2024-04-02', 1), (14, 11, 2, 'Lola', 'F', 2, '2024-04-11', 1),
(15, 12, 3, 'Simba', 'M', 4, '2024-04-23', 1), (16, 13, 4, 'Nala', 'F', 4, '2024-05-04', 1),
(17, 14, 5, 'Jerry', 'M', 1, '2024-05-16', 1), (18, 15, 6, 'Bugs', 'M', 3, '2024-05-26', 1),
(19, 16, 7, 'Piolín', 'M', 2, '2024-06-06', 1), (20, 17, 9, 'Garfield', 'M', 6, '2024-06-19', 1),
(21, 18, 10, 'Donatello', 'M', 15, '2024-07-02', 1), (22, 19, 11, 'Dory', 'F', 2, '2024-07-13', 1),
(23, 20, 1, 'Rex', 'M', 9, '2024-07-26', 1), (24, 21, 2, 'Chloe', 'F', 3, '2024-08-06', 1),
(25, 22, 8, 'Kaiser', 'M', 5, '2024-08-19', 1), (26, 23, 3, 'Salem', 'M', 7, '2024-09-02', 1),
(27, 24, 4, 'Bola de Nieve', 'F', 2, '2024-09-15', 1), (28, 25, 1, 'Zeus', 'M', 6, '2024-09-29', 1),
(29, 26, 9, 'Mufasa', 'M', 8, '2024-10-11', 1), (30, 27, 10, 'Franklin', 'M', 20, '2024-10-23', 1),
(31, 28, 11, 'Wanda', 'F', 1, '2024-11-06', 1), (32, 29, 1, 'Apolo', 'M', 3, '2024-11-19', 1),
(33, 30, 2, 'Luna', 'F', 5, '2024-11-20', 0), (34, 5, 8, 'Sombra', 'M', 4, '2024-01-08', 1),
(35, 10, 3, 'Cleo', 'F', 2, '2024-03-22', 1);
SET IDENTITY_INSERT mascotas OFF;

-- ===============================================
-- Datos para Consultas (25)
-- ===============================================
SET IDENTITY_INSERT consultas ON;
INSERT INTO consultas (id_consulta, id_mascota, id_usuario, id_estado_consulta, fecha_consulta, motivo, diagnostico, FechaCreacion, EsActivo) VALUES
(1, 1, 2, 3, '2024-01-10', 'Control anual', 'Todo en orden', '2024-01-09', 1),
(2, 2, 8, 1, '2024-01-12', 'Vacunación', NULL, '2024-01-11', 1),
(3, 3, 9, 2, '2024-01-15', 'Revisión por tos', 'Posible resfriado', '2024-01-14', 1),
(4, 4, 2, 3, '2024-02-18', 'Chequeo general', 'Perro sano', '2024-02-17', 1),
(5, 5, 8, 1, '2024-02-20', 'Consulta rutinaria', NULL, '2024-02-19', 1),
(6, 6, 9, 3, '2024-03-10', 'Herida en pata', 'Corte superficial, desinfectado', '2024-03-09', 1),
(7, 7, 2, 1, '2024-03-12', 'Alergia', NULL, '2024-03-11', 1),
(8, 8, 8, 2, '2024-03-15', 'Desparasitación', 'Se aplica pipeta', '2024-03-14', 1),
(9, 9, 9, 3, '2024-04-18', 'Control de peso', 'Peso ideal', '2024-04-17', 1),
(10, 10, 2, 1, '2024-04-20', 'Problemas digestivos', NULL, '2024-04-19', 1),
(11, 11, 8, 3, '2024-05-10', 'Cambio de agua', 'Chequeo general de pez', '2024-05-09', 1),
(12, 12, 9, 1, '2024-05-12', 'Vacuna anual', NULL, '2024-05-11', 1),
(13, 13, 2, 2, '2024-05-15', 'Cojera', 'Esguince leve', '2024-05-14', 1),
(14, 14, 8, 3, '2024-06-18', 'Corte de uñas', 'Realizado sin problemas', '2024-06-17', 1),
(15, 15, 9, 1, '2024-06-20', 'Revisión ocular', NULL, '2024-06-19', 1),
(16, 16, 2, 3, '2024-07-10', 'Control de alergia', 'Mejora notable', '2024-07-09', 1),
(17, 17, 8, 1, '2024-07-12', 'Examen de sangre', NULL, '2024-07-11', 1),
(18, 18, 9, 2, '2024-07-15', 'Dieta', 'Ajuste de dieta', '2024-07-14', 1),
(19, 19, 2, 3, '2024-08-18', 'Revisión de alas', 'Plumas en buen estado', '2024-08-17', 1),
(20, 20, 8, 1, '2024-08-20', 'Problema de piel', NULL, '2024-08-19', 1),
(21, 21, 9, 3, '2024-09-10', 'Limpieza de caparazón', 'Realizada', '2024-09-09', 1),
(22, 22, 2, 1, '2024-09-12', 'Chequeo de escamas', NULL, '2024-09-11', 1),
(23, 23, 8, 2, '2024-09-15', 'Vacuna antirrábica', 'Aplicada', '2024-09-14', 1),
(24, 24, 9, 3, '2024-10-18', 'Control post-operatorio', 'Recuperación excelente', '2024-10-17', 1),
(25, 25, 2, 1, '2024-10-20', 'Consulta por comportamiento', NULL, '2024-10-19', 1);
SET IDENTITY_INSERT consultas OFF;

-- ===============================================
-- Datos para Tratamientos (25)
-- ===============================================
SET IDENTITY_INSERT tratamientos ON;
INSERT INTO tratamientos (id_tratamiento, id_consulta, descripcion, medicamento, FechaRegistro, EsActivo) VALUES
(1, 1, 'Desparasitación interna', 'Pastilla Milbactor', '2024-01-10', 1),
(2, 3, 'Antibiótico para tos', 'Doxiciclina 100mg', '2024-01-15', 1),
(3, 3, 'Antiinflamatorio', 'Meloxicam 0.5mg', '2024-01-15', 1),
(4, 6, 'Limpieza y sutura', 'Clorhexidina y sutura absorbible', '2024-03-10', 1),
(5, 7, 'Antihistamínico', 'Clorfenamina', '2024-03-12', 1),
(6, 8, 'Pipeta antipulgas', 'Frontline Plus', '2024-03-15', 1),
(7, 10, 'Probióticos', 'Fortiflora', '2024-04-20', 1),
(8, 13, 'Reposo y antiinflamatorio', 'Carprofeno 25mg', '2024-05-15', 1),
(9, 14, 'Ninguno', 'Solo corte de uñas', '2024-06-18', 1),
(10, 15, 'Gotas oftálmicas', 'Tobramicina', '2024-06-20', 1),
(11, 16, 'Continuar antihistamínico', 'Clorfenamina', '2024-07-10', 1),
(12, 18, 'Cambio de pienso', 'Pienso hipoalergénico', '2024-07-15', 1),
(13, 19, 'Suplemento vitamínico', 'Vitaminas para aves', '2024-08-18', 1),
(14, 20, 'Champú medicado', 'Champú con ketoconazol', '2024-08-20', 1),
(15, 21, 'Ninguno', 'Limpieza manual', '2024-09-10', 1),
(16, 23, 'Ninguno', 'Solo aplicación de vacuna', '2024-09-15', 1),
(17, 24, 'Analgésicos post-operatorios', 'Tramadol', '2024-10-18', 1),
(18, 2, 'Vacuna Triple Felina', 'Leucogen', '2024-01-12', 1),
(19, 4, 'Ninguno', 'Chequeo general', '2024-02-18', 1),
(20, 5, 'Suplemento de calcio', 'Calcio para reptiles', '2024-02-20', 1),
(21, 9, 'Ninguno', 'Control de peso', '2024-04-18', 1),
(22, 11, 'Acondicionador de agua', 'Prime Seachem', '2024-05-10', 1),
(23, 12, 'Vacuna Óctuple Canina', 'Canigen MHA2PPi/L', '2024-05-12', 1),
(24, 17, 'Ninguno', 'Toma de muestra de sangre', '2024-07-12', 1),
(25, 25, 'Terapia de comportamiento', 'Adaptil', '2024-10-20', 1);
SET IDENTITY_INSERT tratamientos OFF;


PRINT 'Datos de ejemplo insertados correctamente.'