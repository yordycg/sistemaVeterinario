using Microsoft.EntityFrameworkCore.Migrations;
using BCrypt.Net; // Necesario para BCrypt.Net.BCrypt.HashPassword
using System; // Necesario para DateOnly.FromDateTime

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sistemaVeterinario.Migrations
{
    /// <inheritdoc />
    public partial class ConsolidateSeedDataAndSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Limpiar todas las tablas para un re-seeding limpio
            migrationBuilder.Sql("DELETE FROM tratamientos");
            migrationBuilder.Sql("DELETE FROM consultas");
            migrationBuilder.Sql("DELETE FROM mascotas");
            migrationBuilder.Sql("DELETE FROM usuarios");
            migrationBuilder.Sql("DELETE FROM clientes");
            migrationBuilder.Sql("DELETE FROM roles");
            migrationBuilder.Sql("DELETE FROM estado_usuarios");
            migrationBuilder.Sql("DELETE FROM razas");
            migrationBuilder.Sql("DELETE FROM especies");
            migrationBuilder.Sql("DELETE FROM estado_consultas");

            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "usuarios",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "tratamientos",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "mascotas",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "consultas",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "clientes",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.InsertData(
                table: "especies",
                columns: new[] { "id_especie", "nombre_especie" },
                values: new object[,]
                {
                    { 1, "Perro" },
                    { 2, "Gato" },
                    { 3, "Hámster" },
                    { 4, "Conejo" },
                    { 5, "Ave" },
                    { 6, "Tortuga" },
                    { 7, "Pez" }
                });

            migrationBuilder.InsertData(
                table: "estado_usuarios",
                columns: new[] { "id_estado_usuario", "nombre_estado" },
                values: new object[,]
                {
                    { 1, "Activo" },
                    { 2, "Inactivo" },
                    { 3, "Pendiente de Activación" },
                    { 4, "Bloqueado" },
                    { 5, "Suspendido" }
                });

            migrationBuilder.InsertData(
                table: "razas",
                columns: new[] { "id_raza", "id_especie", "nombre_raza" },
                values: new object[,]
                {
                    { 1, 1, "Labrador" },
                    { 2, 1, "Poodle" },
                    { 8, 1, "Pastor Alemán" },
                    { 3, 2, "Siamés" },
                    { 4, 2, "Persa" },
                    { 9, 2, "Maine Coon" },
                    { 5, 3, "Dorado" },
                    { 6, 4, "Cabeza de León" },
                    { 7, 5, "Canario" },
                    { 10, 6, "Tortuga de Tierra" },
                    { 11, 7, "Goldfish" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id_rol", "nombre_rol" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Veterinari@" },
                    { 3, "Recepcionista" }
                });

            migrationBuilder.InsertData(
                table: "estado_consultas",
                columns: new[] { "id_estado_consulta", "nombre_estado" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "En Progreso" },
                    { 3, "Finalizada" }
                });

            // Insertar el usuario Admin
            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "id_usuario", "id_rol", "id_estado_usuario", "nombre", "email", "password", "FechaRegistro", "EsActivo" },
                values: new object[] { 
                    1, 
                    1, // Rol Admin
                    1, // Estado Activo
                    "Admin", 
                    "admin@vet.com", 
                    BCrypt.Net.BCrypt.HashPassword("Admin123"), // Contraseña hasheada
                    DateOnly.FromDateTime(DateTime.Now),
                    true
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM tratamientos");
            migrationBuilder.Sql("DELETE FROM consultas");
            migrationBuilder.Sql("DELETE FROM mascotas");
            migrationBuilder.Sql("DELETE FROM usuarios");
            migrationBuilder.Sql("DELETE FROM clientes");
            migrationBuilder.Sql("DELETE FROM roles");
            migrationBuilder.Sql("DELETE FROM estado_usuarios");
            migrationBuilder.Sql("DELETE FROM razas");
            migrationBuilder.Sql("DELETE FROM especies");
            migrationBuilder.Sql("DELETE FROM estado_consultas");

            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "tratamientos");

            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "mascotas");

            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "consultas");

            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "clientes");
        }
    }
}
