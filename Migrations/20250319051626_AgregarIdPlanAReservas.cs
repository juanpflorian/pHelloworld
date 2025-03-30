using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pHelloworld.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIdPlanAReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Usuarios_IdGuia",
                table: "Planes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Usuarios_IdGuia",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Usuarios_IdTurista",
                table: "Reservas");

            migrationBuilder.RenameColumn(
                name: "TipoUsuario",
                table: "Usuarios",
                newName: "usuario");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Usuarios",
                newName: "Fecha_Creacion");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Reservas",
                newName: "precio");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Reservas",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "Duracion",
                table: "Reservas",
                newName: "duracion");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Reservas",
                newName: "descripcion");

            migrationBuilder.RenameColumn(
                name: "IdTurista",
                table: "Reservas",
                newName: "id_turista");

            migrationBuilder.RenameColumn(
                name: "IdGuia",
                table: "Reservas",
                newName: "id_guia");

            migrationBuilder.RenameColumn(
                name: "FechaReserva",
                table: "Reservas",
                newName: "fecha_reserva");

            migrationBuilder.RenameColumn(
                name: "FechaProgramada",
                table: "Reservas",
                newName: "fecha_programada");

            migrationBuilder.RenameColumn(
                name: "IdReserva",
                table: "Reservas",
                newName: "id_reserva");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_IdTurista",
                table: "Reservas",
                newName: "IX_Reservas_id_turista");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_IdGuia",
                table: "Reservas",
                newName: "IX_Reservas_id_guia");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Planes",
                newName: "precio");

            migrationBuilder.RenameColumn(
                name: "Duracion",
                table: "Planes",
                newName: "duracion");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Planes",
                newName: "descripcion");

            migrationBuilder.RenameColumn(
                name: "NumeroPersonas",
                table: "Planes",
                newName: "numero_personas");

            migrationBuilder.RenameColumn(
                name: "NombrePlan",
                table: "Planes",
                newName: "nombre_plan");

            migrationBuilder.RenameColumn(
                name: "IdGuia",
                table: "Planes",
                newName: "id_guia");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Planes",
                newName: "fecha_creacion");

            migrationBuilder.RenameColumn(
                name: "IdPlan",
                table: "Planes",
                newName: "id_plan");

            migrationBuilder.RenameIndex(
                name: "IX_Planes_IdGuia",
                table: "Planes",
                newName: "IX_Planes_id_guia");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "TarifaTour",
                table: "Usuarios",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TarifaHora",
                table: "Usuarios",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nacionalidad",
                table: "Usuarios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Usuarios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuarios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Tipo_Usuario",
                table: "Usuarios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "duracion",
                table: "Reservas",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "Reservas",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "id_plan",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "precio",
                table: "Planes",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "nombre_plan",
                table: "Planes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "id_guia",
                table: "Planes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_id_plan",
                table: "Reservas",
                column: "id_plan");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Usuarios_id_guia",
                table: "Planes",
                column: "id_guia",
                principalTable: "Usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Planes_id_plan",
                table: "Reservas",
                column: "id_plan",
                principalTable: "Planes",
                principalColumn: "id_plan",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Usuarios_id_guia",
                table: "Reservas",
                column: "id_guia",
                principalTable: "Usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Usuarios_id_turista",
                table: "Reservas",
                column: "id_turista",
                principalTable: "Usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Usuarios_id_guia",
                table: "Planes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Planes_id_plan",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Usuarios_id_guia",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Usuarios_id_turista",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_id_plan",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Tipo_Usuario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "id_plan",
                table: "Reservas");

            migrationBuilder.RenameColumn(
                name: "usuario",
                table: "Usuarios",
                newName: "TipoUsuario");

            migrationBuilder.RenameColumn(
                name: "Fecha_Creacion",
                table: "Usuarios",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "precio",
                table: "Reservas",
                newName: "Precio");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Reservas",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "duracion",
                table: "Reservas",
                newName: "Duracion");

            migrationBuilder.RenameColumn(
                name: "descripcion",
                table: "Reservas",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "id_turista",
                table: "Reservas",
                newName: "IdTurista");

            migrationBuilder.RenameColumn(
                name: "id_guia",
                table: "Reservas",
                newName: "IdGuia");

            migrationBuilder.RenameColumn(
                name: "fecha_reserva",
                table: "Reservas",
                newName: "FechaReserva");

            migrationBuilder.RenameColumn(
                name: "fecha_programada",
                table: "Reservas",
                newName: "FechaProgramada");

            migrationBuilder.RenameColumn(
                name: "id_reserva",
                table: "Reservas",
                newName: "IdReserva");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_id_turista",
                table: "Reservas",
                newName: "IX_Reservas_IdTurista");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_id_guia",
                table: "Reservas",
                newName: "IX_Reservas_IdGuia");

            migrationBuilder.RenameColumn(
                name: "precio",
                table: "Planes",
                newName: "Precio");

            migrationBuilder.RenameColumn(
                name: "duracion",
                table: "Planes",
                newName: "Duracion");

            migrationBuilder.RenameColumn(
                name: "descripcion",
                table: "Planes",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "numero_personas",
                table: "Planes",
                newName: "NumeroPersonas");

            migrationBuilder.RenameColumn(
                name: "nombre_plan",
                table: "Planes",
                newName: "NombrePlan");

            migrationBuilder.RenameColumn(
                name: "id_guia",
                table: "Planes",
                newName: "IdGuia");

            migrationBuilder.RenameColumn(
                name: "fecha_creacion",
                table: "Planes",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "id_plan",
                table: "Planes",
                newName: "IdPlan");

            migrationBuilder.RenameIndex(
                name: "IX_Planes_id_guia",
                table: "Planes",
                newName: "IX_Planes_IdGuia");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "TarifaTour",
                table: "Usuarios",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TarifaHora",
                table: "Usuarios",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nacionalidad",
                table: "Usuarios",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Usuarios",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuarios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duracion",
                table: "Reservas",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Reservas",
                keyColumn: "Descripcion",
                keyValue: null,
                column: "Descripcion",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Reservas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "Planes",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NombrePlan",
                table: "Planes",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "IdGuia",
                table: "Planes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Usuarios_IdGuia",
                table: "Planes",
                column: "IdGuia",
                principalTable: "Usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Usuarios_IdGuia",
                table: "Reservas",
                column: "IdGuia",
                principalTable: "Usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Usuarios_IdTurista",
                table: "Reservas",
                column: "IdTurista",
                principalTable: "Usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
