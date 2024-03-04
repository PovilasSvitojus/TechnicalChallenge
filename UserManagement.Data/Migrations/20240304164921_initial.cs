using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Forename = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "Forename", "IsActive", "Surname" },
                values: new object[,]
                {
                    { 1L, new DateTime(1990, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "ploew@example.com", "Peter", true, "Loew" },
                    { 2L, new DateTime(1971, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "bfgates@example.com", "Benjamin Franklin", true, "Gates" },
                    { 3L, new DateTime(1956, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "ctroy@example.com", "Castor", false, "Troy" },
                    { 4L, new DateTime(1975, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "mraines@example.com", "Memphis", true, "Raines" },
                    { 5L, new DateTime(1999, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "sgodspeed@example.com", "Stanley", true, "Goodspeed" },
                    { 6L, new DateTime(1983, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "himcdunnough@example.com", "H.I.", true, "McDunnough" },
                    { 7L, new DateTime(2008, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "cpoe@example.com", "Cameron", false, "Poe" },
                    { 8L, new DateTime(2010, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "emalus@example.com", "Edward", false, "Malus" },
                    { 9L, new DateTime(1998, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "dmacready@example.com", "Damon", false, "Macready" },
                    { 10L, new DateTime(2000, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "jblaze@example.com", "Johnny", true, "Blaze" },
                    { 11L, new DateTime(1966, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "rfeld@example.com", "Robin", true, "Feld" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
