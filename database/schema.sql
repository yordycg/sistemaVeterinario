USE master;
GO

IF DB_ID('sistema_veterinario_db') IS NOT NULL
BEGIN
    DROP DATABASE sistema_veterinario_db;
END
GO

CREATE DATABASE sistema_veterinario_db;
GO

USE sistema_veterinario_db;
GO


-- ------------------------------------
-- TABLAS
-- ------------------------------------
CREATE TABLE roles (
    id_rol INT PRIMARY KEY IDENTITY(1,1),
    nombre_rol VARCHAR(25) NOT NULL
);

CREATE TABLE estado_usuarios (
    id_estado_usuario INT PRIMARY KEY IDENTITY(1,1),
    nombre_estado VARCHAR(25) NOT NULL
);

CREATE TABLE estado_consultas (
    id_estado_consulta INT PRIMARY KEY IDENTITY(1,1),
    nombre_estado VARCHAR(25) NOT NULL
);

CREATE TABLE razas (
    id_raza INT PRIMARY KEY IDENTITY(1,1),
    nombre_raza VARCHAR(30) NOT NULL
);

CREATE TABLE especies (
    id_especie INT PRIMARY KEY IDENTITY(1,1),
    nombre_especie VARCHAR(30) NOT NULL
);

CREATE TABLE clientes (
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    run VARCHAR(15) UNIQUE NOT NULL,
    nombre VARCHAR(50) NOT NULL,
    telefono VARCHAR(15),
    email VARCHAR(100) UNIQUE NOT NULL,
    direccion VARCHAR(100)
);

CREATE TABLE usuarios (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    id_rol INT NOT NULL,
    id_estado_usuario INT NOT NULL,
    nombre VARCHAR(25) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(10) NOT NULL CHECK (LEN(password) >= 8), -- password minimo de 8 caracteres.
    FOREIGN KEY (id_rol) REFERENCES roles(id_rol),
    FOREIGN KEY (id_estado_usuario) REFERENCES estado_usuarios(id_estado_usuario)
);

CREATE TABLE mascotas (
    id_mascota INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT NOT NULL,
    id_especie INT NOT NULL,
    id_raza INT NOT NULL,
    nombre VARCHAR(50) NOT NULL,
    sexo CHAR(1) CHECK (sexo IN ('M', 'H')), -- 'Macho' o 'Hembra'.
    edad INT CHECK (edad >= 0), -- edad debe ser positivo.
    FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente),
    FOREIGN KEY (id_especie) REFERENCES especies(id_especie),
    FOREIGN KEY (id_raza) REFERENCES razas(id_raza)
);

CREATE TABLE consultas (
    id_consulta INT PRIMARY KEY IDENTITY(1,1),
    id_mascota INT NOT NULL,
    id_usuario INT NOT NULL,
    id_estado_consulta INT NOT NULL,
    fecha_consulta DATE NOT NULL,
    motivo TEXT,
    diagnostico TEXT,
    FOREIGN KEY (id_mascota) REFERENCES mascotas(id_mascota),
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (id_estado_consulta) REFERENCES estado_consultas(id_estado_consulta)
);

CREATE TABLE tratamientos (
    id_tratamiento INT PRIMARY KEY IDENTITY(1,1),
    id_consulta INT NOT NULL,
    descripcion VARCHAR(255),
    medicamento VARCHAR(100),
    FOREIGN KEY (id_consulta) REFERENCES consultas(id_consulta)
);
