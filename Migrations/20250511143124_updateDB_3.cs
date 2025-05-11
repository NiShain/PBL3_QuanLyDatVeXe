using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3_QuanLyDatXe.Migrations
{
    /// <inheritdoc />
    public partial class updateDB_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Customers_Customerid",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Trips_Tripid",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.RenameTable(
                name: "Ticket",
                newName: "Tickets");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_Tripid",
                table: "Tickets",
                newName: "IX_Tickets_Tripid");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_Customerid",
                table: "Tickets",
                newName: "IX_Tickets_Customerid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Customers_Customerid",
                table: "Tickets",
                column: "Customerid",
                principalTable: "Customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Trips_Tripid",
                table: "Tickets",
                column: "Tripid",
                principalTable: "Trips",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Customers_Customerid",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Trips_Tripid",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "Ticket");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_Tripid",
                table: "Ticket",
                newName: "IX_Ticket_Tripid");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_Customerid",
                table: "Ticket",
                newName: "IX_Ticket_Customerid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Customers_Customerid",
                table: "Ticket",
                column: "Customerid",
                principalTable: "Customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Trips_Tripid",
                table: "Ticket",
                column: "Tripid",
                principalTable: "Trips",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
