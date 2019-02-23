using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityReservation.DataAccess.Migrations
{
    public partial class ReservationInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tabBlockType",
                columns: table => new
                {
                    TypeId = table.Column<Guid>(nullable: false),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabBlockType", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "tabDisabledPeriod",
                columns: table => new
                {
                    PeriodId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    RepeatYearly = table.Column<bool>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabDisabledPeriod", x => x.PeriodId);
                });

            migrationBuilder.CreateTable(
                name: "tabNotice",
                columns: table => new
                {
                    NoticeId = table.Column<Guid>(nullable: false),
                    NoticeTitle = table.Column<string>(nullable: true),
                    NoticeDesc = table.Column<string>(nullable: true),
                    NoticeContent = table.Column<string>(nullable: true),
                    NoticePath = table.Column<string>(nullable: true),
                    NoticeExternalLink = table.Column<string>(nullable: true),
                    NoticeImagePath = table.Column<string>(nullable: true),
                    NoticeCustomPath = table.Column<string>(nullable: true),
                    NoticeVisitCount = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    UpdateBy = table.Column<string>(nullable: true),
                    NoticePublishTime = table.Column<DateTime>(nullable: false),
                    NoticePublisher = table.Column<string>(nullable: true),
                    CheckStatus = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabNotice", x => x.NoticeId);
                });

            migrationBuilder.CreateTable(
                name: "tabOperationLog",
                columns: table => new
                {
                    LogId = table.Column<Guid>(nullable: false),
                    OperTime = table.Column<DateTime>(nullable: false),
                    LogContent = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    OperBy = table.Column<string>(nullable: true),
                    LogModule = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabOperationLog", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "tabReservationPlace",
                columns: table => new
                {
                    PlaceId = table.Column<Guid>(nullable: false),
                    PlaceName = table.Column<string>(nullable: true),
                    PlaceIndex = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    UpdateBy = table.Column<string>(nullable: true),
                    MaxReservationPeriodNum = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabReservationPlace", x => x.PlaceId);
                });

            migrationBuilder.CreateTable(
                name: "tabSystemSettings",
                columns: table => new
                {
                    SettingId = table.Column<Guid>(nullable: false),
                    SettingName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    SettingValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabSystemSettings", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "tabUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    UserPassword = table.Column<string>(nullable: true),
                    IsSuper = table.Column<bool>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    UserMail = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabUser", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "tabWechatMenuConfig",
                columns: table => new
                {
                    ConfigId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    ButtonKey = table.Column<string>(nullable: true),
                    ButtonType = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabWechatMenuConfig", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "tabBlockEntity",
                columns: table => new
                {
                    BlockId = table.Column<Guid>(nullable: false),
                    BlockTypeId = table.Column<Guid>(nullable: false),
                    BlockValue = table.Column<string>(nullable: true),
                    BlockTime = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabBlockEntity", x => x.BlockId);
                    table.ForeignKey(
                        name: "FK_tabBlockEntity_tabBlockType_BlockTypeId",
                        column: x => x.BlockTypeId,
                        principalTable: "tabBlockType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tabReservation",
                columns: table => new
                {
                    ReservationId = table.Column<Guid>(nullable: false),
                    ReservationPersonName = table.Column<string>(nullable: true),
                    ReservationPersonPhone = table.Column<string>(nullable: true),
                    ReservationTime = table.Column<DateTime>(nullable: false),
                    ReservationForTime = table.Column<string>(nullable: true),
                    ReservationUnit = table.Column<string>(nullable: true),
                    ReservationActivityContent = table.Column<string>(nullable: true),
                    ReservationPeriod = table.Column<int>(nullable: false),
                    ReservationStatus = table.Column<int>(nullable: false),
                    UpdateBy = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    ReservationForDate = table.Column<DateTime>(nullable: false),
                    ReservationFromIp = table.Column<string>(nullable: true),
                    ReservationPlaceId = table.Column<Guid>(nullable: false),
                    UpdateMemo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabReservation", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_tabReservation_tabReservationPlace_ReservationPlaceId",
                        column: x => x.ReservationPlaceId,
                        principalTable: "tabReservationPlace",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tabReservationPeriod",
                columns: table => new
                {
                    PeriodId = table.Column<Guid>(nullable: false),
                    PeriodTitle = table.Column<string>(nullable: true),
                    PeriodDescription = table.Column<string>(nullable: true),
                    PeriodIndex = table.Column<int>(nullable: false),
                    PlaceId = table.Column<Guid>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateBy = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabReservationPeriod", x => x.PeriodId);
                    table.ForeignKey(
                        name: "FK_tabReservationPeriod_tabReservationPlace_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "tabReservationPlace",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tabBlockEntity_BlockTypeId",
                table: "tabBlockEntity",
                column: "BlockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tabReservation_ReservationPlaceId",
                table: "tabReservation",
                column: "ReservationPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_tabReservationPeriod_PlaceId",
                table: "tabReservationPeriod",
                column: "PlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tabBlockEntity");

            migrationBuilder.DropTable(
                name: "tabDisabledPeriod");

            migrationBuilder.DropTable(
                name: "tabNotice");

            migrationBuilder.DropTable(
                name: "tabOperationLog");

            migrationBuilder.DropTable(
                name: "tabReservation");

            migrationBuilder.DropTable(
                name: "tabReservationPeriod");

            migrationBuilder.DropTable(
                name: "tabSystemSettings");

            migrationBuilder.DropTable(
                name: "tabUser");

            migrationBuilder.DropTable(
                name: "tabWechatMenuConfig");

            migrationBuilder.DropTable(
                name: "tabBlockType");

            migrationBuilder.DropTable(
                name: "tabReservationPlace");
        }
    }
}
