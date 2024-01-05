using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_CustumerId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "CustumerId",
                table: "Order",
                newName: "customerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustumerId",
                table: "Order",
                newName: "IX_Order_customerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_customerId",
                table: "Order",
                column: "customerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_customerId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "Order",
                newName: "CustumerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_customerId",
                table: "Order",
                newName: "IX_Order_CustumerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_CustumerId",
                table: "Order",
                column: "CustumerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
