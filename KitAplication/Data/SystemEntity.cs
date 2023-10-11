using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitAplication.Data
{
    public class SystemEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SystemName { get; set; } 
        [MinLength(5)]
        [MaxLength(25)]
        public string RoleName { get; set; } = "system";
        [Required]
        public string Model { get; set; }
        public string Prefix { get; set; } = "";
        [Required]
        [MinLength(5)]
        public string SystemContent { get; set; } 
        public bool IsActive { get; set; } = false;
        public virtual ICollection<MessageEntity>? messages { get; set; }
    }
    public class MessageEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string RoleName { get; set; } 
        [Required]
        [MinLength(3)]
        public string Content { get; set; } 

        [ForeignKey("SystemId")]
        public int SystemId { get; set; }
        public virtual SystemEntity System { get; set; } //navigation property


    }
    public class LinkEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [Url]
        [MinLength(5)]
        public string Url { get; set; }
    }
    public class ChatSettings
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string IsActiveMessage { get; set; } = "Hej. Vad kan jag hjälpa dig med?";

        [StringLength(200)]
        public string IsNotActiveMessage { get; set; } = "Chatten är tyvärr stängd";

        [StringLength(200)]
        public string RequestFailMessage { get; set; } = "Någonting gick fel, försök igen.";
    }
}
