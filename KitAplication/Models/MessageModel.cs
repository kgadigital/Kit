using System.ComponentModel.DataAnnotations;

namespace KitAplication.Models
{
    public class MessageModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Role name must be between 3 and 20 characters")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(3, ErrorMessage = "Content must be at least 3 characters")]
        public string Content { get; set; }

        public int SystemId { get; set; }
    }
}
