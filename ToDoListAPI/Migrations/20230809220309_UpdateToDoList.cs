using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListAPI.Migrations
{
    public partial class UpdateToDoList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_User_AdminId",
                table: "ToDoList");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "ToDoList",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoList_AdminId",
                table: "ToDoList",
                newName: "IX_ToDoList_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_User_UserId",
                table: "ToDoList",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_User_UserId",
                table: "ToDoList");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ToDoList",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoList_UserId",
                table: "ToDoList",
                newName: "IX_ToDoList_AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_User_AdminId",
                table: "ToDoList",
                column: "AdminId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
