using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListAPI.Migrations
{
    public partial class AddToDoList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ToDoListId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ToDoList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoList_User_AdminId",
                        column: x => x.AdminId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ToDoListId",
                table: "User",
                column: "ToDoListId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_AdminId",
                table: "ToDoList",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ToDoList_ToDoListId",
                table: "User",
                column: "ToDoListId",
                principalTable: "ToDoList",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_ToDoList_ToDoListId",
                table: "User");

            migrationBuilder.DropTable(
                name: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_User_ToDoListId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ToDoListId",
                table: "User");
        }
    }
}
