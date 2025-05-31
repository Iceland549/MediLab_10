using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatientMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Adress", "DateOfBirth", "FirstName", "Gender", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "1 Brookside St", new DateTime(1966, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "TestNone", "F", "Test", "100-222-3333" },
                    { 2, "2 High St", new DateTime(1945, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "TestBorderline", "M", "Test", "200-333-4444" },
                    { 3, "3 Club Road", new DateTime(2004, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "TestInDanger", "M", "Test", "300-444-5555" },
                    { 4, "4 Valley Dr", new DateTime(2002, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "TestEarlyOnSet", "F", "Test", "400-555-6666" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
