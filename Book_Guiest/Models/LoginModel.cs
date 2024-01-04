using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Book_Guiest.Models
{
    public class LoginModel
    {
        [Required]
        [DisplayName("Почтовый ящик")]
        public string? Email { get; set; }
        [Required]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
