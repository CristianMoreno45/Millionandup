using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Millionandup.MsProperty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Owner",
                schema: "dbo",
                columns: table => new
                {
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Owner ID"),
                    Name = table.Column<string>(type: "nvarchar(80)", nullable: false, comment: "Owner name"),
                    Address = table.Column<string>(type: "nvarchar(125)", nullable: false, comment: "Owner Address"),
                    Photo = table.Column<string>(type: "nvarchar(2048)", nullable: false, comment: "Owner Photo URL"),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false, comment: "Owner Birthday")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.OwnerId);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                schema: "dbo",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Property ID"),
                    Name = table.Column<string>(type: "nvarchar(80)", nullable: false, comment: "Property name"),
                    Address = table.Column<string>(type: "nvarchar(125)", nullable: false, comment: "Property Address"),
                    Price = table.Column<decimal>(type: "money", nullable: false, comment: "Property Price"),
                    CodeInternal = table.Column<string>(type: "nvarchar(50)", nullable: false, comment: "Code Internal"),
                    Year = table.Column<int>(type: "int", nullable: false, comment: "Property Age in years")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.PropertyId);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImage",
                schema: "dbo",
                columns: table => new
                {
                    PropertyImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Property image ID"),
                    File = table.Column<string>(type: "nvarchar(2048)", nullable: false, comment: "Date of the sale"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, comment: "Log is eneable")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImage", x => x.PropertyImageId);
                });

            migrationBuilder.CreateTable(
                name: "OwnerByProperty",
                schema: "dbo",
                columns: table => new
                {
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerByProperty", x => new { x.OwnerId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_OwnerByProperty_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "dbo",
                        principalTable: "Owner",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnerByProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "dbo",
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTrace",
                schema: "dbo",
                columns: table => new
                {
                    PropertyTraceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Trace identifier"),
                    DateSale = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Date of the sale"),
                    Name = table.Column<string>(type: "nvarchar(80)", nullable: false, comment: "Property name"),
                    Value = table.Column<decimal>(type: "money", nullable: false, comment: "Property value"),
                    Tax = table.Column<decimal>(type: "money", nullable: false, comment: "Taxes value"),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Property ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTrace", x => x.PropertyTraceId);
                    table.ForeignKey(
                        name: "FK_PropertyTrace_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "dbo",
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImageByProperty",
                schema: "dbo",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImageByProperty", x => new { x.PropertyId, x.PropertyImageId });
                    table.ForeignKey(
                        name: "FK_PropertyImageByProperty_PropertyImage_PropertyImageId",
                        column: x => x.PropertyImageId,
                        principalSchema: "dbo",
                        principalTable: "PropertyImage",
                        principalColumn: "PropertyImageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyImageByProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "dbo",
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Owner_OwnerId",
                schema: "dbo",
                table: "Owner",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerByProperty_PropertyId",
                schema: "dbo",
                table: "OwnerByProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Property_PropertyId",
                schema: "dbo",
                table: "Property",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImage_PropertyImageId",
                schema: "dbo",
                table: "PropertyImage",
                column: "PropertyImageId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImageByProperty_PropertyImageId",
                schema: "dbo",
                table: "PropertyImageByProperty",
                column: "PropertyImageId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTrace_DateSale",
                schema: "dbo",
                table: "PropertyTrace",
                column: "DateSale",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTrace_PropertyId",
                schema: "dbo",
                table: "PropertyTrace",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTrace_PropertyTraceId",
                schema: "dbo",
                table: "PropertyTrace",
                column: "PropertyTraceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OwnerByProperty",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PropertyImageByProperty",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PropertyTrace",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Owner",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PropertyImage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Property",
                schema: "dbo");
        }
    }
}
