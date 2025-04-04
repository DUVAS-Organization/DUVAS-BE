using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryPriorityPackageRooms",
                columns: table => new
                {
                    CategoryPriorityPackageRoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryPriorityPackageRoomValue = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPriorityPackageRooms", x => x.CategoryPriorityPackageRoomId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPriorityPackageServicePosts",
                columns: table => new
                {
                    CategoryPriorityPackageServicePostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryPriorityPackageServicePostValue = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPriorityPackageServicePosts", x => x.CategoryPriorityPackageServicePostId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryRooms",
                columns: table => new
                {
                    CategoryRoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryRooms", x => x.CategoryRoomId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryServices",
                columns: table => new
                {
                    CategoryServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryServices", x => x.CategoryServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalDateTimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentalDateTimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractFile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoleAdmin = table.Column<int>(type: "int", nullable: true),
                    RoleUser = table.Column<int>(type: "int", nullable: true),
                    RoleLandlord = table.Column<int>(type: "int", nullable: true),
                    RoleService = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartyAId = table.Column<int>(type: "int", nullable: false),
                    PartyBId = table.Column<int>(type: "int", nullable: false),
                    PdfUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizationContracts_Users_PartyAId",
                        column: x => x.PartyAId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_AuthorizationContracts_Users_PartyBId",
                        column: x => x.PartyBId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BuildingName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verify = table.Column<bool>(type: "bit", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                    table.ForeignKey(
                        name: "FK_Buildings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandlordLicenses",
                columns: table => new
                {
                    LandlordLicenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnhCCCDMatTruoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnhCCCDMatSau = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiayPhepKinhDoanh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandlordLicenses", x => x.LandlordLicenseId);
                    table.ForeignKey(
                        name: "FK_LandlordLicenses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSendID = table.Column<int>(type: "int", nullable: false),
                    UserGetID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Message_Users_UserGetID",
                        column: x => x.UserGetID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Message_Users_UserSendID",
                        column: x => x.UserSendID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ServiceLicenses",
                columns: table => new
                {
                    ServiceLicenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnhCCCDMatTruoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnhCCCDMatSau = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiayPhepKinhDoanh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiayPhepChuyenMon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLicenses", x => x.ServiceLicenseId);
                    table.ForeignKey(
                        name: "FK_ServiceLicenses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicePosts",
                columns: table => new
                {
                    ServicePostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPermission = table.Column<int>(type: "int", nullable: true),
                    CategoryServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePosts", x => x.ServicePostId);
                    table.ForeignKey(
                        name: "FK_ServicePosts_CategoryServices_CategoryServiceId",
                        column: x => x.CategoryServiceId,
                        principalTable: "CategoryServices",
                        principalColumn: "CategoryServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicePosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CassoId = table.Column<int>(type: "int", nullable: true),
                    TId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CusumBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    When = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BankSubAccID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubAccID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    bankAbbreviation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorresponsiveName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorresponsiveAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorresponsiveBankId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorresponsiveBankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: true),
                    CategoryRoomId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Acreage = table.Column<double>(type: "float", nullable: false),
                    Furniture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfBathroom = table.Column<int>(type: "int", nullable: false),
                    NumberOfBedroom = table.Column<int>(type: "int", nullable: false),
                    Garret = table.Column<bool>(type: "bit", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Dien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Nuoc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Internet = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rac = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GuiXe = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuanLy = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChiPhiKhac = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    IsPermission = table.Column<int>(type: "int", nullable: true),
                    reputation = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_CategoryRooms_CategoryRoomId",
                        column: x => x.CategoryRoomId,
                        principalTable: "CategoryRooms",
                        principalColumn: "CategoryRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PriorityPackageServicePosts",
                columns: table => new
                {
                    PriorityPackageServicePostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ServicePostId = table.Column<int>(type: "int", nullable: false),
                    CategoryPriorityPackageServicePostId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorityPackageServicePosts", x => x.PriorityPackageServicePostId);
                    table.ForeignKey(
                        name: "FK_PriorityPackageServicePosts_CategoryPriorityPackageServicePosts_CategoryPriorityPackageServicePostId",
                        column: x => x.CategoryPriorityPackageServicePostId,
                        principalTable: "CategoryPriorityPackageServicePosts",
                        principalColumn: "CategoryPriorityPackageServicePostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriorityPackageServicePosts_ServicePosts_ServicePostId",
                        column: x => x.ServicePostId,
                        principalTable: "ServicePosts",
                        principalColumn: "ServicePostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriorityPackageServicePosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentalServiceLists",
                columns: table => new
                {
                    RentalServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceppDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RentalDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServicePostID = table.Column<int>(type: "int", nullable: false),
                    RenterID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    RentalServiceStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalServiceLists", x => x.RentalServiceId);
                    table.ForeignKey(
                        name: "FK_RentalServiceLists_ServicePosts_ServicePostID",
                        column: x => x.ServicePostID,
                        principalTable: "ServicePosts",
                        principalColumn: "ServicePostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalServiceLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ServiceFeedbacks",
                columns: table => new
                {
                    ServiceFeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicePostId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Star = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFeedbacks", x => x.ServiceFeedbackId);
                    table.ForeignKey(
                        name: "FK_ServiceFeedbacks_ServicePosts_ServicePostId",
                        column: x => x.ServicePostId,
                        principalTable: "ServicePosts",
                        principalColumn: "ServicePostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WithdrawRequests_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WithdrawRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriorityPackageRooms",
                columns: table => new
                {
                    PriorityPackageRoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    CategoryPriorityPackageRoomId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorityPackageRooms", x => x.PriorityPackageRoomId);
                    table.ForeignKey(
                        name: "FK_PriorityPackageRooms_CategoryPriorityPackageRooms_CategoryPriorityPackageRoomId",
                        column: x => x.CategoryPriorityPackageRoomId,
                        principalTable: "CategoryPriorityPackageRooms",
                        principalColumn: "CategoryPriorityPackageRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriorityPackageRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriorityPackageRooms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentalLists",
                columns: table => new
                {
                    RentalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    RenterID = table.Column<int>(type: "int", nullable: false),
                    RentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthForRent = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentalStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalLists", x => x.RentalId);
                    table.ForeignKey(
                        name: "FK_RentalLists_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalLists_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalLists_Users_RenterID",
                        column: x => x.RenterID,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    ServicePostId = table.Column<int>(type: "int", nullable: true),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    ReportContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_ServicePosts_ServicePostId",
                        column: x => x.ServicePostId,
                        principalTable: "ServicePosts",
                        principalColumn: "ServicePostId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomLicenses",
                columns: table => new
                {
                    RoomLicenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    BienBanPCCC = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomLicenses", x => x.RoomLicenseId);
                    table.ForeignKey(
                        name: "FK_RoomLicenses_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ServicePostId = table.Column<int>(type: "int", nullable: true),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedPosts_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SavedPosts_ServicePosts_ServicePostId",
                        column: x => x.ServicePostId,
                        principalTable: "ServicePosts",
                        principalColumn: "ServicePostId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SavedPosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFeedbacks",
                columns: table => new
                {
                    UserFeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Star = table.Column<double>(type: "float", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeedbacks", x => x.UserFeedbackId);
                    table.ForeignKey(
                        name: "FK_UserFeedbacks_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_UserFeedbacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsiderTradings",
                columns: table => new
                {
                    InsiderTradingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Remitter = table.Column<int>(type: "int", nullable: false),
                    RemittersUserId = table.Column<int>(type: "int", nullable: true),
                    Receiver = table.Column<int>(type: "int", nullable: false),
                    ReceiversUserId = table.Column<int>(type: "int", nullable: true),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    PriorityPackageRoomId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoldUntil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsiderTradings", x => x.InsiderTradingId);
                    table.ForeignKey(
                        name: "FK_InsiderTradings_PriorityPackageRooms_PriorityPackageRoomId",
                        column: x => x.PriorityPackageRoomId,
                        principalTable: "PriorityPackageRooms",
                        principalColumn: "PriorityPackageRoomId");
                    table.ForeignKey(
                        name: "FK_InsiderTradings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_InsiderTradings_Users_ReceiversUserId",
                        column: x => x.ReceiversUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_InsiderTradings_Users_RemittersUserId",
                        column: x => x.RemittersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationContracts_PartyAId",
                table: "AuthorizationContracts",
                column: "PartyAId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationContracts_PartyBId",
                table: "AuthorizationContracts",
                column: "PartyBId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_UserId",
                table: "Buildings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InsiderTradings_PriorityPackageRoomId",
                table: "InsiderTradings",
                column: "PriorityPackageRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_InsiderTradings_ReceiversUserId",
                table: "InsiderTradings",
                column: "ReceiversUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InsiderTradings_RemittersUserId",
                table: "InsiderTradings",
                column: "RemittersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InsiderTradings_RoomId",
                table: "InsiderTradings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_LandlordLicenses_UserId",
                table: "LandlordLicenses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserGetID",
                table: "Message",
                column: "UserGetID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserSendID",
                table: "Message",
                column: "UserSendID");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityPackageRooms_CategoryPriorityPackageRoomId",
                table: "PriorityPackageRooms",
                column: "CategoryPriorityPackageRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityPackageRooms_RoomId",
                table: "PriorityPackageRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityPackageRooms_UserId",
                table: "PriorityPackageRooms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityPackageServicePosts_CategoryPriorityPackageServicePostId",
                table: "PriorityPackageServicePosts",
                column: "CategoryPriorityPackageServicePostId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityPackageServicePosts_ServicePostId",
                table: "PriorityPackageServicePosts",
                column: "ServicePostId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityPackageServicePosts_UserId",
                table: "PriorityPackageServicePosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalLists_ContractId",
                table: "RentalLists",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalLists_RenterID",
                table: "RentalLists",
                column: "RenterID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalLists_RoomId",
                table: "RentalLists",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalServiceLists_ServicePostID",
                table: "RentalServiceLists",
                column: "ServicePostID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalServiceLists_UserId",
                table: "RentalServiceLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_RoomId",
                table: "Reports",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ServicePostId",
                table: "Reports",
                column: "ServicePostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TransactionId",
                table: "Reports",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomLicenses_RoomId",
                table: "RoomLicenses",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BuildingId",
                table: "Rooms",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CategoryRoomId",
                table: "Rooms",
                column: "CategoryRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_UserId",
                table: "Rooms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_RoomId",
                table: "SavedPosts",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_ServicePostId",
                table: "SavedPosts",
                column: "ServicePostId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_UserId",
                table: "SavedPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFeedbacks_ServicePostId",
                table: "ServiceFeedbacks",
                column: "ServicePostId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLicenses_UserId",
                table: "ServiceLicenses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePosts_CategoryServiceId",
                table: "ServicePosts",
                column: "CategoryServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePosts_UserId",
                table: "ServicePosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFeedbacks_RoomId",
                table: "UserFeedbacks",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFeedbacks_UserId",
                table: "UserFeedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequests_TransactionId",
                table: "WithdrawRequests",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequests_UserId",
                table: "WithdrawRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationContracts");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "InsiderTradings");

            migrationBuilder.DropTable(
                name: "LandlordLicenses");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "PriorityPackageServicePosts");

            migrationBuilder.DropTable(
                name: "RentalLists");

            migrationBuilder.DropTable(
                name: "RentalServiceLists");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "RoomLicenses");

            migrationBuilder.DropTable(
                name: "SavedPosts");

            migrationBuilder.DropTable(
                name: "ServiceFeedbacks");

            migrationBuilder.DropTable(
                name: "ServiceLicenses");

            migrationBuilder.DropTable(
                name: "UserFeedbacks");

            migrationBuilder.DropTable(
                name: "WithdrawRequests");

            migrationBuilder.DropTable(
                name: "PriorityPackageRooms");

            migrationBuilder.DropTable(
                name: "CategoryPriorityPackageServicePosts");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "ServicePosts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "CategoryPriorityPackageRooms");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "CategoryServices");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "CategoryRooms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
