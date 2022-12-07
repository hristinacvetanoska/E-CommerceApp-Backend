using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceApp_Backend.Migrations
{
    public partial class AddFilterBySellerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SellerName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerName",
                table: "NewProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c5c2b8db-5753-4e9b-a4fd-ca9bf9d2ddc7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a208d32c-ec7b-4a31-a869-68735bcd3c65");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a5c77320-7b40-4dca-980a-0e2d3c5a45c6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SellerName",
                table: "NewProducts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "98bef2fe-1667-44d2-8205-493e0941c2de");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "8fa43781-a216-4947-bd08-1869db1f6907");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "1792ead8-dc57-4f3c-8e67-5b12b3f8d7eb");
        }
    }
}
