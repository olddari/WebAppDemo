using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppDemo.Data.Migrations
{
    public partial class AddRoleIDToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_RoleID",
                table: "Customers",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Roles_RoleID",
                table: "Customers",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Roles_RoleID",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_RoleID",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Customers");
        }
    }
}
