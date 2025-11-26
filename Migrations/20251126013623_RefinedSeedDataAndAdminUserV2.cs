using Microsoft.EntityFrameworkCore.Migrations;
using BCrypt.Net; // Necesario para BCrypt.Net.BCrypt.HashPassword
using System; // Necesario para DateOnly.FromDateTime

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sistemaVeterinario.Migrations
{
    /// <inheritdoc />
    public partial class RefinedSeedDataAndAdminUserV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar datos de tablas en orden de dependencia para evitar conflictos de clave externa
            migrationBuilder.Sql("DELETE FROM tratamientos");
            migrationBuilder.Sql("DELETE FROM consultas");
            migrationBuilder.Sql("DELETE FROM mascotas");
            migrationBuilder.Sql("DELETE FROM usuarios");
            migrationBuilder.Sql("DELETE FROM clientes");

            // Eliminar los roles existentes para evitar la violación de clave primaria al insertar los nuevos
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id_rol",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id_rol",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id_rol",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "especies",
                columns: new[] { "id_especie", "nombre_especie" },
                values: new object[,]
                {
                    { 6, "Tortuga" },
                    { 7, "Pez" }
                });

            migrationBuilder.InsertData(
                table: "estado_usuarios",
                columns: new[] { "id_estado_usuario", "nombre_estado" },
                values: new object[,]
                {
                    { 3, "Pendiente de Activación" },
                    { 4, "Bloqueado" },
                    { 5, "Suspendido" }
                });

            migrationBuilder.InsertData(
                table: "razas",
                columns: new[] { "id_raza", "id_especie", "nombre_raza" },
                values: new object[,]
                {
                    { 8, 1, "Pastor Alemán" },
                    { 9, 2, "Maine Coon" }
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
                table: "razas",
                columns: new[] { "id_raza", "id_especie", "nombre_raza" },
                values: new object[,]
                {
                    { 10, 6, "Tortuga de Tierra" },
                    { 11, 7, "Goldfish" }
                });
            
            // Insertar el usuario Admin
            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "id_usuario", "id_rol", "id_estado_usuario", "nombre", "email", "password", "FechaRegistro" },
                values: new object[] { 
                    1, 
                    1, // Rol Admin
                    1, // Estado Activo
                    "Admin", 
                    "admin@vet.com", 
                    BCrypt.Net.BCrypt.HashPassword("Admin123"), // Contraseña hasheada
                    DateOnly.FromDateTime(DateTime.Now) 
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar el usuario Admin
            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "id_usuario",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "estado_usuarios",
                keyColumn: "id_estado_usuario",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "estado_usuarios",
                keyColumn: "id_estado_usuario",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "estado_usuarios",
                keyColumn: "id_estado_usuario",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id_rol",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id_rol",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id_rol",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "especies",
                keyColumn: "id_especie",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "especies",
                keyColumn: "id_especie",
                keyValue: 7);
        }
    }
}
