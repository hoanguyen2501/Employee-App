using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeApp.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Username = table.Column<string>(type: "varchar(255)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "longblob", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "longblob", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    EstablishedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    Country = table.Column<string>(type: "longtext", nullable: true),
                    City = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(255)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: true),
                    LastName = table.Column<string>(type: "longtext", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Gender = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    City = table.Column<string>(type: "longtext", nullable: true),
                    Country = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(255)", nullable: true),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true),
                    HiredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "Id", "CreatedAt", "Description", "PasswordHash", "PasswordSalt", "UpdatedAt", "Username" },
                values: new object[] { new Guid("391a641b-16b9-4008-b56f-d315ab66ca28"), new DateTime(2023, 8, 2, 5, 4, 17, 259, DateTimeKind.Utc).AddTicks(5804), null, new byte[] { 22, 132, 162, 245, 219, 243, 238, 185, 62, 203, 157, 81, 195, 160, 138, 213, 135, 79, 78, 42, 204, 167, 242, 121, 114, 2, 124, 119, 243, 213, 155, 146, 118, 127, 133, 31, 93, 33, 39, 166, 233, 126, 227, 210, 39, 126, 18, 77, 57, 198, 30, 211, 68, 201, 200, 19, 30, 199, 34, 51, 103, 157, 135, 156 }, new byte[] { 199, 199, 185, 241, 228, 66, 64, 28, 157, 154, 52, 10, 142, 1, 32, 2, 102, 64, 80, 23, 14, 196, 52, 71, 92, 102, 5, 110, 215, 129, 30, 229, 138, 63, 120, 57, 237, 14, 94, 17, 30, 141, 83, 60, 162, 87, 172, 113, 190, 143, 44, 121, 48, 161, 187, 92, 62, 180, 236, 100, 77, 81, 131, 245, 156, 16, 43, 185, 26, 240, 146, 250, 146, 5, 176, 172, 185, 192, 6, 167, 62, 249, 75, 219, 182, 118, 153, 64, 154, 206, 1, 131, 227, 72, 140, 16, 99, 121, 183, 121, 0, 77, 193, 182, 161, 207, 58, 195, 25, 14, 124, 102, 84, 57, 103, 162, 29, 9, 38, 186, 60, 53, 199, 129, 253, 218, 3, 61 }, new DateTime(2023, 8, 2, 5, 4, 17, 259, DateTimeKind.Utc).AddTicks(5806), "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_Username",
                table: "AppUser",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_Email",
                table: "Company",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_PhoneNumber",
                table: "Company",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyId",
                table: "Employee",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Email",
                table: "Employee",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PhoneNumber",
                table: "Employee",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
