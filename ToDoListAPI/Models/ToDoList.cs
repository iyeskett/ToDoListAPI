using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ToDoListAPI.Models
{
    public class ToDoList
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Name { get; set; }

        public User? User { get; set; }

        [Display(Name = "Administrador")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public List<User>? Collaborators { get; set; }
        public bool Closed { get; set; }
    }
}