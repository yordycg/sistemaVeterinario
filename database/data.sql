-- Script de siembra de datos iniciales
USE sistema_veterinario_db;

-- Insertar Especies
INSERT INTO especies (nombre_especie) VALUES 
('Perro'), 
('Gato'), 
('Hámster'), 
('Conejo'), 
('Ave');
GO

-- Insertar Razas
INSERT INTO razas (nombre_raza) VALUES 
('Labrador'), 
('Poodle'),
('Siamés'), 
('Persa'),
('Dorado'),
('Cabeza de León'),
('Canario');
GO

PRINT 'Datos de Especie y Raza insertados correctamente.';
