using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KitAplication.Models
{
    public class SystemModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "System name is required")]
        [DisplayName("Namn på systemet")]
        public string SystemName { get; set; }

        [StringLength(25, MinimumLength = 5, ErrorMessage = "Role name must be between 5 and 25 characters")]
        public string RoleName { get; set; } = "system";

        [Required(ErrorMessage = "Model is required")]
        [DisplayName("Modell")]
        public string Model { get; set; } = "gpt-3.5-turbo";

        [DisplayName("Suffix")]
        public string? Prefix { get; set; } = "";

        [Required(ErrorMessage = "System content is required")]
        [MinLength(5, ErrorMessage = "System content must be at least 5 characters")]
        [DisplayName("Systemmeddelande")]
        public string SystemContent { get; set; }

        public bool IsActive { get; set; } = false;
    }
}
