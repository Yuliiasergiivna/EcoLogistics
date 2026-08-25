using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoLogistics.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Localites_CommuneBXLS_Id_commune",
                table: "Localites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommuneBXLS",
                table: "CommuneBXLS");

            migrationBuilder.RenameTable(
                name: "CommuneBXLS",
                newName: "CommuneBXLs");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Adresse",
                table: "Donnees_persos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommuneBXLs",
                table: "CommuneBXLs",
                column: "Id_commune");

            migrationBuilder.AddForeignKey(
                name: "FK_Localites_CommuneBXLs_Id_commune",
                table: "Localites",
                column: "Id_commune",
                principalTable: "CommuneBXLs",
                principalColumn: "Id_commune");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Localites_CommuneBXLs_Id_commune",
                table: "Localites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommuneBXLs",
                table: "CommuneBXLs");

            migrationBuilder.RenameTable(
                name: "CommuneBXLs",
                newName: "CommuneBXLS");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Email",
                keyValue: null,
                column: "Email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Donnees_persos",
                keyColumn: "Adresse",
                keyValue: null,
                column: "Adresse",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Adresse",
                table: "Donnees_persos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommuneBXLS",
                table: "CommuneBXLS",
                column: "Id_commune");

            migrationBuilder.AddForeignKey(
                name: "FK_Localites_CommuneBXLS_Id_commune",
                table: "Localites",
                column: "Id_commune",
                principalTable: "CommuneBXLS",
                principalColumn: "Id_commune");
        }
    }
}
