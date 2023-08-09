using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Title { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Description { get; set; }

        public User? User { get; set; }

        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public int UserId { get; set; }

        public bool Done { get; set; }
    }
}