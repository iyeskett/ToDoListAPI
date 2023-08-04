using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "{0} deve estar entre {2} e {1} caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}