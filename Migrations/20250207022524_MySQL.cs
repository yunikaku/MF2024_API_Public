using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MF2024_API.Migrations
{
    /// <inheritdoc />
    public partial class MySQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserPasswoedUpdataTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserDateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserGender = table.Column<string>(type: "longtext", nullable: false),
                    UserAddress = table.Column<string>(type: "longtext", nullable: false),
                    UserDateOfEntry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserDateOfRetirement = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserUpdataDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserUpdataUser = table.Column<string>(type: "longtext", nullable: true),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: true),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__1788CC4C3A3D3A3A", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DepartmentName = table.Column<string>(type: "char(10)", fixedLength: true, maxLength: 10, nullable: false),
                    DepartmentNameKana = table.Column<string>(type: "char(10)", fixedLength: true, maxLength: 10, nullable: false),
                    DiscordURL = table.Column<string>(type: "longtext", nullable: true),
                    DepartmentFlag = table.Column<int>(type: "int", nullable: false),
                    DepartmentAddUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    DepartmentAddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DepartmentUpDateUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    DepartmentUpDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Departme__B2079BCD8440F670", x => x.DepartmentID);
                    table.ForeignKey(
                        name: "FK_Department_AddUserID",
                        column: x => x.DepartmentAddUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Department_UpDateUserID",
                        column: x => x.DepartmentUpDateUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NFC",
                columns: table => new
                {
                    NfcID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NfcState = table.Column<int>(type: "int", nullable: false),
                    NfcUID = table.Column<string>(type: "longtext", nullable: false),
                    NfcAddTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    NfcAddUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    NfcUpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    NfcUpdateUserID = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NFC__628F58637E500CAF", x => x.NfcID);
                    table.ForeignKey(
                        name: "FK_NFC_AddUserID",
                        column: x => x.NfcAddUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NFC_UpDateUserID",
                        column: x => x.NfcUpdateUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NoReservation",
                columns: table => new
                {
                    NoReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NoReservationName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NoReservationNameKana = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NoReservationNumberOfPrsesons = table.Column<int>(type: "int", nullable: false),
                    NoReservationRequirement = table.Column<string>(type: "longtext", nullable: false),
                    NoReservationState = table.Column<int>(type: "int", nullable: false),
                    NoReservationEmail = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NoReservationPhoneNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NoReservationCompanyName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NoReservationCompanyNameKana = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NoReservationCompanyPosition = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NoReservationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    NoReservationAddUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    NoReservationAddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NoReservationUpdateUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    NoReservationUpDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NoReserv__64ADE52735B426D1", x => x.NoReservationID);
                    table.ForeignKey(
                        name: "FK_NoReservation_AddUserID",
                        column: x => x.NoReservationAddUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoReservation_UpdateUserID",
                        column: x => x.NoReservationUpdateUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    OfficeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OfficeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    OfficeNameKana = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    OfficeLocation = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    OfficeFlag = table.Column<int>(type: "int", nullable: false),
                    OfficeAddUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    OfficeAddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OfficeUpDateUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    OfficeUpDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Office__4B61930F387A0FF8", x => x.OfficeID);
                    table.ForeignKey(
                        name: "FK_Office_AddUserID",
                        column: x => x.OfficeAddUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Office_UpDateUserID",
                        column: x => x.OfficeUpDateUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    SectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SectionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SectionNameKana = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DiscordURL = table.Column<string>(type: "longtext", nullable: true),
                    SectionFlag = table.Column<int>(type: "int", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    SectionAddUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    SectionAddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SectionUpDateUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    SectionUpDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Section__80EF08927BE474C1", x => x.SectionID);
                    table.ForeignKey(
                        name: "FK_Section_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "DepartmentID");
                    table.ForeignKey(
                        name: "FK_Section_SectionAddUserID",
                        column: x => x.SectionAddUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Section_SectionUpDateUserID",
                        column: x => x.SectionUpDateUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DepartmentOffice",
                columns: table => new
                {
                    DepartmentsDepartmentId = table.Column<int>(type: "int", nullable: false),
                    OfficesOfficeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentOffice", x => new { x.DepartmentsDepartmentId, x.OfficesOfficeId });
                    table.ForeignKey(
                        name: "FK_DepartmentOffice_Department_DepartmentsDepartmentId",
                        column: x => x.DepartmentsDepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentOffice_Office_OfficesOfficeId",
                        column: x => x.OfficesOfficeId,
                        principalTable: "Office",
                        principalColumn: "OfficeID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ReservationName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ReservationNameKana = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ReservationNumberOfPersons = table.Column<int>(type: "int", nullable: false),
                    ReservationRequirement = table.Column<string>(type: "longtext", nullable: false),
                    ReservationCompanyName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ReservationCompanyNameKana = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ReservationCompanyPosition = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    ReservationEmail = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ReservationPhoneNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ReservationState = table.Column<int>(type: "int", nullable: false),
                    ReservationReception = table.Column<int>(type: "int", nullable: false),
                    ReservationType = table.Column<int>(type: "int", nullable: false),
                    ReservationQRcode = table.Column<byte[]>(type: "longblob", nullable: false),
                    ReservationCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Token = table.Column<string>(type: "longtext", nullable: true),
                    ReservationAddUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    ReservationAddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReservationUpdateUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    ReservationUpDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reservat__B7EE5F0420AEDDCE", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_Reservation_AddUserID",
                        column: x => x.ReservationAddUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservation_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "SectionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_UpdateUserID",
                        column: x => x.ReservationUpdateUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoomName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    RoomCapacity = table.Column<int>(type: "int", nullable: false),
                    OfficeID = table.Column<int>(type: "int", nullable: false),
                    RoomState = table.Column<int>(type: "int", nullable: false),
                    SectionID = table.Column<int>(type: "int", nullable: true),
                    RoomAddUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    RommAddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RoompDateUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoomUpDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Room__328639194D197077", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_Room_OfficeID",
                        column: x => x.OfficeID,
                        principalTable: "Office",
                        principalColumn: "OfficeID");
                    table.ForeignKey(
                        name: "FK_Room_SectionAddUserID",
                        column: x => x.RoomAddUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Room_SectionID",
                        column: x => x.SectionID,
                        principalTable: "Section",
                        principalColumn: "SectionID");
                    table.ForeignKey(
                        name: "FK_Room_SectionUpDateUserID",
                        column: x => x.RoompDateUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NFCAllotment",
                columns: table => new
                {
                    NFCAllotmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AllotmentTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    NfcID = table.Column<int>(type: "int", nullable: false),
                    ReservationID = table.Column<int>(type: "int", nullable: true),
                    NoReservationID = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true),
                    AddUserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateUserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NFCAllot__5C5F4CB94877DFBC", x => x.NFCAllotmentID);
                    table.ForeignKey(
                        name: "FK_NFCAllotment_AddUserID",
                        column: x => x.AddUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NFCAllotment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NFCAllotment_NfcID",
                        column: x => x.NfcID,
                        principalTable: "NFC",
                        principalColumn: "NfcID");
                    table.ForeignKey(
                        name: "FK_NFCAllotment_NoReservationID",
                        column: x => x.NoReservationID,
                        principalTable: "NoReservation",
                        principalColumn: "NoReservationID");
                    table.ForeignKey(
                        name: "FK_NFCAllotment_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservation",
                        principalColumn: "ReservationID");
                    table.ForeignKey(
                        name: "FK_NFCAllotment_UpdateUserID",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DeviceName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DeviceLocation = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DeviceCategory = table.Column<int>(type: "int", nullable: false),
                    DeviceFlag = table.Column<int>(type: "int", nullable: false),
                    DeviceUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    DeviceAddUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    DeviceAddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeviceUpdateUserID = table.Column<string>(type: "varchar(255)", nullable: false),
                    DeviceUpDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Device__49E1233136A11D86", x => x.DeviceID);
                    table.ForeignKey(
                        name: "FK_Device_AddUserID",
                        column: x => x.DeviceAddUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Device_AspNetUsers_DeviceUserID",
                        column: x => x.DeviceUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Device_Room_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Room",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Device_UserID",
                        column: x => x.DeviceUpdateUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EquipmentName = table.Column<string>(type: "longtext", nullable: false),
                    EquipmentData = table.Column<byte[]>(type: "longblob", nullable: true),
                    EquipmentFlag = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipment__80EF08927BE474C1", x => x.EquipmentID);
                    table.ForeignKey(
                        name: "FK_Equipment_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomID");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    RoomImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoomImageName = table.Column<string>(type: "longtext", nullable: false),
                    RoomImageData = table.Column<byte[]>(type: "longblob", nullable: false),
                    roomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Image__80EF08927BE474C1", x => x.RoomImageID);
                    table.ForeignKey(
                        name: "FK_Image_Room_roomID",
                        column: x => x.roomID,
                        principalTable: "Room",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConferenceRoomReservation",
                columns: table => new
                {
                    ConferenceRoomReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ConferenceRoomReservationRequirement = table.Column<string>(type: "longtext", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceRoomReservation", x => x.ConferenceRoomReservationId);
                    table.ForeignKey(
                        name: "FK_ConferenceRoomReservation_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "DeviceID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Entrants",
                columns: table => new
                {
                    EntrantsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DeviceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrants", x => x.EntrantsID);
                    table.ForeignKey(
                        name: "FK_Entrants_Device_DeviceID",
                        column: x => x.DeviceID,
                        principalTable: "Device",
                        principalColumn: "DeviceID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OptOut",
                columns: table => new
                {
                    OptOutID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DeviceID = table.Column<int>(type: "int", nullable: false),
                    NFCAllotmentID = table.Column<int>(type: "int", nullable: false),
                    OptOutState = table.Column<int>(type: "int", nullable: false),
                    OptOutTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OptOut__82926A1C961131F5", x => x.OptOutID);
                    table.ForeignKey(
                        name: "FK_OptOut_Device",
                        column: x => x.DeviceID,
                        principalTable: "Device",
                        principalColumn: "DeviceID");
                    table.ForeignKey(
                        name: "FK_OptOut_NFCAllotment",
                        column: x => x.NFCAllotmentID,
                        principalTable: "NFCAllotment",
                        principalColumn: "NFCAllotmentID");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntrantsNfcallotment",
                columns: table => new
                {
                    EntrantsID = table.Column<int>(type: "int", nullable: false),
                    NfcallotmentsNfcallotmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrantsNfcallotment", x => new { x.EntrantsID, x.NfcallotmentsNfcallotmentId });
                    table.ForeignKey(
                        name: "FK_EntrantsNfcallotment_Entrants_EntrantsID",
                        column: x => x.EntrantsID,
                        principalTable: "Entrants",
                        principalColumn: "EntrantsID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrantsNfcallotment_NFCAllotment_NfcallotmentsNfcallotmentId",
                        column: x => x.NfcallotmentsNfcallotmentId,
                        principalTable: "NFCAllotment",
                        principalColumn: "NFCAllotmentID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceRoomReservation_DeviceId",
                table: "ConferenceRoomReservation",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_DepartmentAddUserID",
                table: "Department",
                column: "DepartmentAddUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Department_DepartmentUpDateUserID",
                table: "Department",
                column: "DepartmentUpDateUserID");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOffice_OfficesOfficeId",
                table: "DepartmentOffice",
                column: "OfficesOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_DeviceAddUserID",
                table: "Device",
                column: "DeviceAddUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Device_DeviceUpdateUserID",
                table: "Device",
                column: "DeviceUpdateUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Device_DeviceUserID",
                table: "Device",
                column: "DeviceUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Device_RoomID",
                table: "Device",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Entrants_DeviceID",
                table: "Entrants",
                column: "DeviceID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntrantsNfcallotment_NfcallotmentsNfcallotmentId",
                table: "EntrantsNfcallotment",
                column: "NfcallotmentsNfcallotmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_RoomId",
                table: "Equipment",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_roomID",
                table: "Image",
                column: "roomID");

            migrationBuilder.CreateIndex(
                name: "IX_NFC_NfcAddUserID",
                table: "NFC",
                column: "NfcAddUserID");

            migrationBuilder.CreateIndex(
                name: "IX_NFC_NfcUpdateUserID",
                table: "NFC",
                column: "NfcUpdateUserID");

            migrationBuilder.CreateIndex(
                name: "IX_NFCAllotment_AddUserId",
                table: "NFCAllotment",
                column: "AddUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NFCAllotment_NfcID",
                table: "NFCAllotment",
                column: "NfcID");

            migrationBuilder.CreateIndex(
                name: "IX_NFCAllotment_NoReservationID",
                table: "NFCAllotment",
                column: "NoReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_NFCAllotment_ReservationID",
                table: "NFCAllotment",
                column: "ReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_NFCAllotment_UpdateUserId",
                table: "NFCAllotment",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NFCAllotment_UserId",
                table: "NFCAllotment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoReservation_NoReservationAddUserID",
                table: "NoReservation",
                column: "NoReservationAddUserID");

            migrationBuilder.CreateIndex(
                name: "IX_NoReservation_NoReservationUpdateUserID",
                table: "NoReservation",
                column: "NoReservationUpdateUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Office_OfficeAddUserID",
                table: "Office",
                column: "OfficeAddUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Office_OfficeUpDateUserID",
                table: "Office",
                column: "OfficeUpDateUserID");

            migrationBuilder.CreateIndex(
                name: "IX_OptOut_DeviceID",
                table: "OptOut",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_OptOut_NFCAllotmentID",
                table: "OptOut",
                column: "NFCAllotmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ReservationAddUserID",
                table: "Reservation",
                column: "ReservationAddUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ReservationUpdateUserID",
                table: "Reservation",
                column: "ReservationUpdateUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_SectionId",
                table: "Reservation",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_OfficeID",
                table: "Room",
                column: "OfficeID");

            migrationBuilder.CreateIndex(
                name: "IX_Room_RoomAddUserID",
                table: "Room",
                column: "RoomAddUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Room_RoompDateUserID",
                table: "Room",
                column: "RoompDateUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Room_SectionID",
                table: "Room",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Section_DepartmentID",
                table: "Section",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Section_SectionAddUserID",
                table: "Section",
                column: "SectionAddUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Section_SectionUpDateUserID",
                table: "Section",
                column: "SectionUpDateUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ConferenceRoomReservation");

            migrationBuilder.DropTable(
                name: "DepartmentOffice");

            migrationBuilder.DropTable(
                name: "EntrantsNfcallotment");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "OptOut");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Entrants");

            migrationBuilder.DropTable(
                name: "NFCAllotment");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "NFC");

            migrationBuilder.DropTable(
                name: "NoReservation");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Office");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
