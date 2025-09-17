using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_workshops.Migrations
{
    /// <inheritdoc />
    public partial class dropCollumNumeroInscritos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroInscricoes",
                table: "Workshops");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumeroInscricoes",
                table: "Workshops",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
