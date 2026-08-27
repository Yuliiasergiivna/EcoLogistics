using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoLogistics.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePersonneContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "PersonneContacts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "PersonneContacts",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
