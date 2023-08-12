using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListAPI.Migrations
{
    public partial class UpdateToDo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ToDoListId",
                table: "ToDo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ToDo_ToDoListId",
                table: "ToDo",
                column: "ToDoListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_ToDoList_ToDoListId",
                table: "ToDo",
                column: "ToDoListId",
                principalTable: "ToDoList",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_ToDoList_ToDoListId",
                table: "ToDo");

            migrationBuilder.DropIndex(
                name: "IX_ToDo_ToDoListId",
                table: "ToDo");

            migrationBuilder.DropColumn(
                name: "ToDoListId",
                table: "ToDo");
        }
    }
}