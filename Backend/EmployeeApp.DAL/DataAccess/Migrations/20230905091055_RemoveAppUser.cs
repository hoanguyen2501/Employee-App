using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeApp.DAL.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "longblob", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "longblob", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Username = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "Id", "CreatedAt", "Description", "PasswordHash", "PasswordSalt", "UpdatedAt", "Username" },
                values: new object[] { new Guid("f5ef4239-676e-4cda-a370-c794fb5aea0e"), new DateTime(2023, 8, 11, 6, 19, 24, 404, DateTimeKind.Utc).AddTicks(9322), null, new byte[] { 245, 1, 83, 158, 90, 70, 47, 143, 215, 255, 85, 95, 1, 83, 25, 214, 170, 160, 213, 99, 131, 72, 184, 250, 249, 185, 236, 220, 91, 255, 154, 247, 17, 162, 97, 130, 25, 181, 208, 59, 35, 47, 5, 8, 120, 254, 231, 16, 30, 128, 58, 204, 68, 3, 165, 185, 120, 44, 2, 74, 214, 54, 62, 119 }, new byte[] { 140, 129, 209, 108, 109, 33, 20, 141, 156, 55, 252, 80, 254, 196, 156, 147, 3, 178, 158, 210, 192, 117, 102, 22, 235, 12, 18, 229, 56, 107, 190, 15, 188, 241, 71, 159, 236, 201, 92, 252, 125, 251, 146, 152, 101, 162, 15, 163, 138, 117, 41, 61, 80, 187, 251, 10, 242, 18, 142, 130, 158, 61, 13, 55, 92, 56, 187, 178, 48, 159, 183, 60, 2, 19, 36, 157, 107, 1, 151, 8, 245, 246, 223, 10, 215, 196, 222, 189, 170, 60, 11, 106, 109, 127, 51, 44, 154, 154, 208, 235, 154, 222, 31, 87, 201, 104, 24, 114, 112, 159, 23, 10, 69, 160, 39, 176, 134, 78, 128, 67, 227, 244, 97, 44, 29, 172, 56, 219 }, new DateTime(2023, 8, 11, 6, 19, 24, 404, DateTimeKind.Utc).AddTicks(9324), "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_Username",
                table: "AppUser",
                column: "Username",
                unique: true);
        }
    }
}
