using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Awake_Data.Migrations
{
    /// <inheritdoc />
    public partial class updateInquiryHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "InquiryHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "InquiryHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "InquiryHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "InquiryHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "InquiryHeaders");

            migrationBuilder.DropColumn(
                name: "State",
                table: "InquiryHeaders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "InquiryHeaders");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "InquiryHeaders");
        }
    }
}
