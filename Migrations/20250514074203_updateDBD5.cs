using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3_QuanLyDatXe.Migrations
{
    /// <inheritdoc />
    public partial class updateDBD5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Buses_Busid",
                table: "Trips");

            migrationBuilder.AlterColumn<int>(
                name: "Busid",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Buses_Busid",
                table: "Trips",
                column: "Busid",
                principalTable: "Buses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Buses_Busid",
                table: "Trips");

            migrationBuilder.AlterColumn<int>(
                name: "Busid",
                table: "Trips",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Buses_Busid",
                table: "Trips",
                column: "Busid",
                principalTable: "Buses",
                principalColumn: "id");
        }
    }
}
