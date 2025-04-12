using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ServiceLicenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LandlordLicenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CategoryPriorityPackageServicePosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CategoryPriorityPackageRooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ServiceLicenses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LandlordLicenses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CategoryPriorityPackageServicePosts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CategoryPriorityPackageRooms");
        }
    }
}
