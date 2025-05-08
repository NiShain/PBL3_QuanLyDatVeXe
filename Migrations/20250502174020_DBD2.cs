using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3_QuanLyDatXe.Migrations
{
    /// <inheritdoc />
    public partial class DBD2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Users_UserId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Customer_Customerid",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Trip_Tripid",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Buses_Busid",
                table: "Trip");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Route_Routeid",
                table: "Trip");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trip",
                table: "Trip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Trip",
                newName: "Trips");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Trip_Routeid",
                table: "Trips",
                newName: "IX_Trips_Routeid");

            migrationBuilder.RenameIndex(
                name: "IX_Trip_Busid",
                table: "Trips",
                newName: "IX_Trips_Busid");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_UserId",
                table: "Customers",
                newName: "IX_Customers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trips",
                table: "Trips",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenTuyen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    diemDi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    diemDen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Account_UserId",
                table: "Customers",
                column: "UserId",
                principalTable: "Account",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Buses_Busid",
                table: "Trips",
                column: "Busid",
                principalTable: "Buses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Lines_Routeid",
                table: "Trips",
                column: "Routeid",
                principalTable: "Lines",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Account_UserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Customers_Customerid",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Trips_Tripid",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Buses_Busid",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Lines_Routeid",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trips",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Trips",
                newName: "Trip");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_Routeid",
                table: "Trip",
                newName: "IX_Trip_Routeid");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_Busid",
                table: "Trip",
                newName: "IX_Trip_Busid");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_UserId",
                table: "Customer",
                newName: "IX_Customer_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trip",
                table: "Trip",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    diemDen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    diemDi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tenTuyen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ten = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Users_UserId",
                table: "Customer",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Customer_Customerid",
                table: "Ticket",
                column: "Customerid",
                principalTable: "Customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Trip_Tripid",
                table: "Ticket",
                column: "Tripid",
                principalTable: "Trip",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Buses_Busid",
                table: "Trip",
                column: "Busid",
                principalTable: "Buses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Route_Routeid",
                table: "Trip",
                column: "Routeid",
                principalTable: "Route",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
