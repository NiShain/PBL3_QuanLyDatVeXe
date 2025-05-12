using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3_QuanLyDatXe.Migrations
{
    /// <inheritdoc />
    public partial class updateDBD4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "giaVe",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "giaVe",
                table: "Trips");
        }
    }
}
