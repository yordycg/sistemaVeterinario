using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sistemaVeterinario.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndUserStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    run = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    telefono = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__clientes__677F38F5448FD6C5", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "especies",
                columns: table => new
                {
                    id_especie = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_especie = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__especies__96DDB0B982955345", x => x.id_especie);
                });

            migrationBuilder.CreateTable(
                name: "estado_consultas",
                columns: table => new
                {
                    id_estado_consulta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_estado = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__estado_c__19A53235F51D74B3", x => x.id_estado_consulta);
                });

            migrationBuilder.CreateTable(
                name: "estado_usuarios",
                columns: table => new
                {
                    id_estado_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_estado = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__estado_u__CEFB9B89DD0EA143", x => x.id_estado_usuario);
                });

            migrationBuilder.CreateTable(
                name: "razas",
                columns: table => new
                {
                    id_raza = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_raza = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__razas__084F250A6A7054C8", x => x.id_raza);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_rol = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__6ABCB5E0E52DCD0F", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "mascotas",
                columns: table => new
                {
                    id_mascota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_especie = table.Column<int>(type: "int", nullable: false),
                    id_raza = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    sexo = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    edad = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mascotas__6F0373524828A893", x => x.id_mascota);
                    table.ForeignKey(
                        name: "FK__mascotas__id_cli__4CA06362",
                        column: x => x.id_cliente,
                        principalTable: "clientes",
                        principalColumn: "id_cliente");
                    table.ForeignKey(
                        name: "FK__mascotas__id_esp__4D94879B",
                        column: x => x.id_especie,
                        principalTable: "especies",
                        principalColumn: "id_especie");
                    table.ForeignKey(
                        name: "FK__mascotas__id_raz__4E88ABD4",
                        column: x => x.id_raza,
                        principalTable: "razas",
                        principalColumn: "id_raza");
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    id_estado_usuario = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usuarios__4E3E04AD4CB7E388", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK__usuarios__id_est__47DBAE45",
                        column: x => x.id_estado_usuario,
                        principalTable: "estado_usuarios",
                        principalColumn: "id_estado_usuario");
                    table.ForeignKey(
                        name: "FK__usuarios__id_rol__46E78A0C",
                        column: x => x.id_rol,
                        principalTable: "roles",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "consultas",
                columns: table => new
                {
                    id_consulta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_mascota = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_estado_consulta = table.Column<int>(type: "int", nullable: false),
                    fecha_consulta = table.Column<DateOnly>(type: "date", nullable: false),
                    motivo = table.Column<string>(type: "text", nullable: true),
                    diagnostico = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__consulta__6F53588BC63A384F", x => x.id_consulta);
                    table.ForeignKey(
                        name: "FK__consultas__id_es__534D60F1",
                        column: x => x.id_estado_consulta,
                        principalTable: "estado_consultas",
                        principalColumn: "id_estado_consulta");
                    table.ForeignKey(
                        name: "FK__consultas__id_ma__5165187F",
                        column: x => x.id_mascota,
                        principalTable: "mascotas",
                        principalColumn: "id_mascota");
                    table.ForeignKey(
                        name: "FK__consultas__id_us__52593CB8",
                        column: x => x.id_usuario,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "tratamientos",
                columns: table => new
                {
                    id_tratamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_consulta = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    medicamento = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tratamie__C8825F4CB6B1F22C", x => x.id_tratamiento);
                    table.ForeignKey(
                        name: "FK__tratamien__id_co__5629CD9C",
                        column: x => x.id_consulta,
                        principalTable: "consultas",
                        principalColumn: "id_consulta");
                });

            migrationBuilder.InsertData(
                table: "estado_usuarios",
                columns: new[] { "id_estado_usuario", "nombre_estado" },
                values: new object[,]
                {
                    { 1, "Activo" },
                    { 2, "Inactivo" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id_rol", "nombre_rol" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Veterinario" },
                    { 3, "Recepcionista" }
                });

            migrationBuilder.CreateIndex(
                name: "UQ__clientes__AB6E6164286AA167",
                table: "clientes",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__clientes__C2B74E6CB8485CC1",
                table: "clientes",
                column: "run",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consultas_id_estado_consulta",
                table: "consultas",
                column: "id_estado_consulta");

            migrationBuilder.CreateIndex(
                name: "IX_consultas_id_mascota",
                table: "consultas",
                column: "id_mascota");

            migrationBuilder.CreateIndex(
                name: "IX_consultas_id_usuario",
                table: "consultas",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_mascotas_id_cliente",
                table: "mascotas",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_mascotas_id_especie",
                table: "mascotas",
                column: "id_especie");

            migrationBuilder.CreateIndex(
                name: "IX_mascotas_id_raza",
                table: "mascotas",
                column: "id_raza");

            migrationBuilder.CreateIndex(
                name: "IX_tratamientos_id_consulta",
                table: "tratamientos",
                column: "id_consulta");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_id_estado_usuario",
                table: "usuarios",
                column: "id_estado_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_id_rol",
                table: "usuarios",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "UQ__usuarios__AB6E616446B1A300",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tratamientos");

            migrationBuilder.DropTable(
                name: "consultas");

            migrationBuilder.DropTable(
                name: "estado_consultas");

            migrationBuilder.DropTable(
                name: "mascotas");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "especies");

            migrationBuilder.DropTable(
                name: "razas");

            migrationBuilder.DropTable(
                name: "estado_usuarios");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
