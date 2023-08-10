namespace ToDoListAPI.Models
{
    public class ToDoListCollaborator
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
        public ToDoList? ToDoList { get; set; }
        public int ToDoListId { get; set; }
    }
}