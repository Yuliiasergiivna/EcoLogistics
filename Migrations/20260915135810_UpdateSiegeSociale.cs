using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoLogistics.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSiegeSociale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdressesExploitation_Clients_Id_client",
                table: "AdressesExploitation");

            migrationBuilder.AddColumn<Guid>(
                name: "Id_client",
                table: "SiegeSociales",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id_client",
                table: "AdressesExploitation",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_SiegeSociales_Id_client",
                table: "SiegeSociales",
                column: "Id_client");

            migrationBuilder.AddForeignKey(
                name: "FK_AdressesExploitation_Clients_Id_client",
                table: "AdressesExploitation",
                column: "Id_client",
                principalTable: "Clients",
                principalColumn: "Id_client");

            migrationBuilder.AddForeignKey(
                name: "FK_SiegeSociales_Clients_Id_client",
                table: "SiegeSociales",
                column: "Id_client",
                principalTable: "Clients",
                principalColumn: "Id_client",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdressesExploitation_Clients_Id_client",
                table: "AdressesExploitation");

            migrationBuilder.DropForeignKey(
                name: "FK_SiegeSociales_Clients_Id_client",
                table: "SiegeSociales");

            migrationBuilder.DropIndex(
                name: "IX_SiegeSociales_Id_client",
                table: "SiegeSociales");

            migrationBuilder.DropColumn(
                name: "Id_client",
                table: "SiegeSociales");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id_client",
                table: "AdressesExploitation",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_AdressesExploitation_Clients_Id_client",
                table: "AdressesExploitation",
                column: "Id_client",
                principalTable: "Clients",
                principalColumn: "Id_client",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
