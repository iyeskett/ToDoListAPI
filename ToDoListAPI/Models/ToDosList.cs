namespace ToDoListAPI.Models
{
    public class ToDosList
    {
        public int Id { get; set; }
        public ToDo? ToDo { get; set; }
        public int ToDoId { get; set; }
        public ToDoList? ToDoList { get; set; }
        public int ToDoListId { get; set; }
    }
}