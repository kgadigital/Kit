using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KitAplication.Models
{
    public class LinkModel
    {
        public int Id { get; set; }
        [DisplayName("Länk namn")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "URL is required")]
        [Url(ErrorMessage = "Invalid URL")]
        [MinLength(5, ErrorMessage = "URL must be at least 5 characters")]
        [DisplayName("Url")]
        public string Url { get; set; }
    }
}
