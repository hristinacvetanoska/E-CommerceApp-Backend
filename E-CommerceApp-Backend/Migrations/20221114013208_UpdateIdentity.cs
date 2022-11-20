using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceApp_Backend.Migrations
{
    public partial class UpdateIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "adaf0579-d5a9-417e-826f-44eea918dcdc", "9c3b3a5e-b4eb-4852-96a8-01d82129004d", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d81b3aa0-e3b7-492a-913d-5c3cb61b8bfa", "e9e3645a-9ece-41a3-99ff-fae1ca0c1069", "Member", "MEMBER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adaf0579-d5a9-417e-826f-44eea918dcdc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d81b3aa0-e3b7-492a-913d-5c3cb61b8bfa");
        }
    }
}
