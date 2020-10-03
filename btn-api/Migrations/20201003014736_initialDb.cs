using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EC_API.Migrations
{
    public partial class initialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    ParentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LineInfos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PO = table.Column<string>(nullable: true),
                    Batch = table.Column<string>(nullable: true),
                    ModelName = table.Column<string>(nullable: true),
                    ModelNO = table.Column<string>(nullable: true),
                    ArticleNO = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LineID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineInfos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LineInfos_Buildings_LineID",
                        column: x => x.LineID,
                        principalTable: "Buildings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: true),
                    WorkerCode = table.Column<string>(nullable: true),
                    Operation = table.Column<string>(nullable: true),
                    BuildingID = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Workers_Buildings_BuildingID",
                        column: x => x.BuildingID,
                        principalTable: "Buildings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Buttons",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Standard = table.Column<int>(nullable: false),
                    TaktTime = table.Column<int>(nullable: false),
                    WorkerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buttons", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Buttons_Workers_WorkerID",
                        column: x => x.WorkerID,
                        principalTable: "Workers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buttons_WorkerID",
                table: "Buttons",
                column: "WorkerID");

            migrationBuilder.CreateIndex(
                name: "IX_LineInfos_LineID",
                table: "LineInfos",
                column: "LineID");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_BuildingID",
                table: "Workers",
                column: "BuildingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buttons");

            migrationBuilder.DropTable(
                name: "LineInfos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
