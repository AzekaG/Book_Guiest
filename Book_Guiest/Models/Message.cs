using System.ComponentModel.DataAnnotations;

namespace Book_Guiest.Models
{
    public class Message
    {
        public int id {  get; set; }

        [Required(ErrorMessage="Отзыв не может быть пустым")]

        public string? msg { get; set; }

        public DateTime? date { get; set; }

        public Users User { get; set; }

        
    }
}
