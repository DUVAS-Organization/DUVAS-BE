//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace BusinessObject.Migrations
//{
//    /// <inheritdoc />
//    public partial class _2 : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropForeignKey(
//                name: "FK_RentalLists_Users_IDThue",
//                table: "RentalLists");

//            migrationBuilder.RenameColumn(
//                name: "GhiChu",
//                table: "Rooms",
//                newName: "Note");

//            migrationBuilder.RenameColumn(
//                name: "IDThue",
//                table: "RentalLists",
//                newName: "RenterID");

//            migrationBuilder.RenameIndex(
//                name: "IX_RentalLists_IDThue",
//                table: "RentalLists",
//                newName: "IX_RentalLists_RenterID");

//            migrationBuilder.AlterColumn<int>(
//                name: "RoleUser",
//                table: "Users",
//                type: "int",
//                nullable: true,
//                oldClrType: typeof(int),
//                oldType: "int");

//            migrationBuilder.AlterColumn<int>(
//                name: "RoleService",
//                table: "Users",
//                type: "int",
//                nullable: true,
//                oldClrType: typeof(int),
//                oldType: "int");

//            migrationBuilder.AlterColumn<int>(
//                name: "RoleLandlord",
//                table: "Users",
//                type: "int",
//                nullable: true,
//                oldClrType: typeof(int),
//                oldType: "int");

//            migrationBuilder.AddColumn<decimal>(
//                name: "Money",
//                table: "Users",
//                type: "decimal(18,2)",
//                nullable: false,
//                defaultValue: 0m);

//            migrationBuilder.AddColumn<int>(
//                name: "RoleAdmin",
//                table: "Users",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CategoryRoomId",
//                table: "Rooms",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<bool>(
//                name: "IsPermission",
//                table: "Rooms",
//                type: "bit",
//                nullable: false,
//                defaultValue: false);

//            migrationBuilder.CreateIndex(
//                name: "IX_Rooms_CategoryRoomId",
//                table: "Rooms",
//                column: "CategoryRoomId");

//            migrationBuilder.AddForeignKey(
//                name: "FK_RentalLists_Users_RenterID",
//                table: "RentalLists",
//                column: "RenterID",
//                principalTable: "Users",
//                principalColumn: "UserId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_Rooms_CategoryRooms_CategoryRoomId",
//                table: "Rooms",
//                column: "CategoryRoomId",
//                principalTable: "CategoryRooms",
//                principalColumn: "CategoryRoomId");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropForeignKey(
//                name: "FK_RentalLists_Users_RenterID",
//                table: "RentalLists");

//            migrationBuilder.DropForeignKey(
//                name: "FK_Rooms_CategoryRooms_CategoryRoomId",
//                table: "Rooms");

//            migrationBuilder.DropIndex(
//                name: "IX_Rooms_CategoryRoomId",
//                table: "Rooms");

//            migrationBuilder.DropColumn(
//                name: "Money",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "RoleAdmin",
//                table: "Users");

//            migrationBuilder.DropColumn(
//                name: "CategoryRoomId",
//                table: "Rooms");

//            migrationBuilder.DropColumn(
//                name: "IsPermission",
//                table: "Rooms");

//            migrationBuilder.RenameColumn(
//                name: "Note",
//                table: "Rooms",
//                newName: "GhiChu");

//            migrationBuilder.RenameColumn(
//                name: "RenterID",
//                table: "RentalLists",
//                newName: "IDThue");

//            migrationBuilder.RenameIndex(
//                name: "IX_RentalLists_RenterID",
//                table: "RentalLists",
//                newName: "IX_RentalLists_IDThue");

//            migrationBuilder.AlterColumn<int>(
//                name: "RoleUser",
//                table: "Users",
//                type: "int",
//                nullable: false,
//                defaultValue: 0,
//                oldClrType: typeof(int),
//                oldType: "int",
//                oldNullable: true);

//            migrationBuilder.AlterColumn<int>(
//                name: "RoleService",
//                table: "Users",
//                type: "int",
//                nullable: false,
//                defaultValue: 0,
//                oldClrType: typeof(int),
//                oldType: "int",
//                oldNullable: true);

//            migrationBuilder.AlterColumn<int>(
//                name: "RoleLandlord",
//                table: "Users",
//                type: "int",
//                nullable: false,
//                defaultValue: 0,
//                oldClrType: typeof(int),
//                oldType: "int",
//                oldNullable: true);

//            migrationBuilder.AddForeignKey(
//                name: "FK_RentalLists_Users_IDThue",
//                table: "RentalLists",
//                column: "IDThue",
//                principalTable: "Users",
//                principalColumn: "UserId",
//                onDelete: ReferentialAction.Restrict);
//        }
//    }
//}
