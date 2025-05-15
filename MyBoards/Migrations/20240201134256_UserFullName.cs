using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards.Migrations
{
    public partial class UserFullName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql(@"
            UPDATE Users
            SET FullName = FirstName + ' ' + LastName
            ");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            //update new columns
            migrationBuilder.Sql(@"
            UPDATE Users
            SET FirstName = LEFT(FullName, CHARINDEX(' ', FullName) - 1), LastName = RIGHT(FullName, CHARINDEX(' ', REVERSE(FullName) - 1)
            ");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Users");
        }
    }
}
