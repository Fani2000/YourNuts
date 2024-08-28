using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOutOfStockToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isOutOfStock",
                table: "OrderProducts");

            migrationBuilder.AddColumn<bool>(
                name: "isOutOfStock",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isOutOfStock",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "isOutOfStock",
                table: "OrderProducts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
