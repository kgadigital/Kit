using Microsoft.Build.Framework;

namespace KitAplication.Models
{
    public class SystemActiveModel
    {
        public int SystemId { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
