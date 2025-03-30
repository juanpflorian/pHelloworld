using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pHelloworld.Migrations
{
    /// <inheritdoc />
    public partial class AddMensajesYNotificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Correo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo_Usuario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contrasena = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    foto_perfil = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Idiomas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nacionalidad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Especialidad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Experiencia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Disponibilidad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TarifaHora = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TarifaTour = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id_usuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mensaje",
                columns: table => new
                {
                    IdMensaje = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdEmisor = table.Column<int>(type: "int", nullable: false),
                    IdReceptor = table.Column<int>(type: "int", nullable: false),
                    Contenido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Leido = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensaje", x => x.IdMensaje);
                    table.ForeignKey(
                        name: "FK_Mensaje_Usuarios_IdEmisor",
                        column: x => x.IdEmisor,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mensaje_Usuarios_IdReceptor",
                        column: x => x.IdReceptor,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Planes",
                columns: table => new
                {
                    id_plan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_guia = table.Column<int>(type: "int", nullable: true),
                    nombre_plan = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    duracion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numero_personas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planes", x => x.id_plan);
                    table.ForeignKey(
                        name: "FK_Planes_Usuarios_id_guia",
                        column: x => x.id_guia,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    id_reserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_turista = table.Column<int>(type: "int", nullable: false),
                    id_guia = table.Column<int>(type: "int", nullable: false),
                    id_plan = table.Column<int>(type: "int", nullable: false),
                    fecha_reserva = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_programada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    duracion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    estado = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.id_reserva);
                    table.ForeignKey(
                        name: "FK_Reservas_Planes_id_plan",
                        column: x => x.id_plan,
                        principalTable: "Planes",
                        principalColumn: "id_plan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservas_Usuarios_id_guia",
                        column: x => x.id_guia,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Usuarios_id_turista",
                        column: x => x.id_turista,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Notificacion",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mensaje = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Leido = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdPlan = table.Column<int>(type: "int", nullable: true),
                    IdReserva = table.Column<int>(type: "int", nullable: true),
                    IdMensaje = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacion", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_Notificacion_Mensaje_IdMensaje",
                        column: x => x.IdMensaje,
                        principalTable: "Mensaje",
                        principalColumn: "IdMensaje",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacion_Planes_IdPlan",
                        column: x => x.IdPlan,
                        principalTable: "Planes",
                        principalColumn: "id_plan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacion_Reservas_IdReserva",
                        column: x => x.IdReserva,
                        principalTable: "Reservas",
                        principalColumn: "id_reserva",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacion_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_IdEmisor",
                table: "Mensaje",
                column: "IdEmisor");

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_IdReceptor",
                table: "Mensaje",
                column: "IdReceptor");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_IdMensaje",
                table: "Notificacion",
                column: "IdMensaje");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_IdPlan",
                table: "Notificacion",
                column: "IdPlan");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_IdReserva",
                table: "Notificacion",
                column: "IdReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_IdUsuario",
                table: "Notificacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_id_guia",
                table: "Planes",
                column: "id_guia");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_id_guia",
                table: "Reservas",
                column: "id_guia");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_id_plan",
                table: "Reservas",
                column: "id_plan");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_id_turista",
                table: "Reservas",
                column: "id_turista");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notificacion");

            migrationBuilder.DropTable(
                name: "Mensaje");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Planes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
