using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListAPI.Migrations
{
    public partial class UpdateToDoToDoListIdNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_ToDoList_ToDoListId",
                table: "ToDo");

            migrationBuilder.AlterColumn<int>(
                name: "ToDoListId",
                table: "ToDo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_ToDoList_ToDoListId",
                table: "ToDo",
                column: "ToDoListId",
                principalTable: "ToDoList",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_ToDoList_ToDoListId",
                table: "ToDo");

            migrationBuilder.AlterColumn<int>(
                name: "ToDoListId",
                table: "ToDo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_ToDoList_ToDoListId",
                table: "ToDo",
                column: "ToDoListId",
                principalTable: "ToDoList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
