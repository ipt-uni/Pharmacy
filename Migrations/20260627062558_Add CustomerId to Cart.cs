using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmacy.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerIdtoCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "TotalQuantity",
                table: "Carts",
                newName: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_MyUsers_CustomerId",
                table: "Carts",
                column: "CustomerId",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_MyUsers_CustomerId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Carts",
                newName: "TotalQuantity");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
