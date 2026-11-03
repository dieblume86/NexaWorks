using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexaWorks.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Os",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Os", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VersionNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVersions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVersionOs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductVersionId = table.Column<int>(type: "int", nullable: false),
                    OsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersionOs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVersionOs_Os_OsId",
                        column: x => x.OsId,
                        principalTable: "Os",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVersionOs_ProductVersions_ProductVersionId",
                        column: x => x.ProductVersionId,
                        principalTable: "ProductVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ResolutionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Issue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductVersionOsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_ProductVersionOs_ProductVersionOsId",
                        column: x => x.ProductVersionOsId,
                        principalTable: "ProductVersionOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Os_Name",
                table: "Os",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersionOs_OsId",
                table: "ProductVersionOs",
                column: "OsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersionOs_ProductVersionId_OsId",
                table: "ProductVersionOs",
                columns: new[] { "ProductVersionId", "OsId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersions_ProductId_VersionNumber",
                table: "ProductVersions",
                columns: new[] { "ProductId", "VersionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreationDate",
                table: "Tickets",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProductVersionOsId",
                table: "Tickets",
                column: "ProductVersionOsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status",
                table: "Tickets",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "ProductVersionOs");

            migrationBuilder.DropTable(
                name: "Os");

            migrationBuilder.DropTable(
                name: "ProductVersions");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
