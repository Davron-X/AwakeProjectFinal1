using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Awake_Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProdQuantityInInquiryDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProdQuantity",
                table: "InquiryDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdQuantity",
                table: "InquiryDetails");
        }
    }
}
