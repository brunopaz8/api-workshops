using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_workshops.Migrations
{
    /// <inheritdoc />
    public partial class addColumPresenca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Presente",
                table: "WorkshopColaboradores",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Presente",
                table: "WorkshopColaboradores");
        }
    }
}
