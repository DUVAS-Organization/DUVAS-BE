using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Authorization",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "AuthorizationContracts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authorization",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "status",
                table: "AuthorizationContracts");
        }
    }
}
