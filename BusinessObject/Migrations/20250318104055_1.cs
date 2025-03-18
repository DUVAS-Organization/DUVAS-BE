using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsPermission",
                table: "ServicePosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "reputation",
                table: "Rooms",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPermission",
                table: "ServicePosts");

            migrationBuilder.DropColumn(
                name: "reputation",
                table: "Rooms");
        }
    }
}
