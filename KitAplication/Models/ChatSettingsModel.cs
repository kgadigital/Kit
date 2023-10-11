using System.ComponentModel.DataAnnotations;

namespace KitAplication.Models
{
    public class ChatSettingsModel
    {
        public int Id { get; set; }
        [StringLength(200)]
        [MinLength(5,ErrorMessage ="Måste innehålla minst 5 tecken")]
        public string IsActiveMessage { get; set; } = "Hej. Vad kan jag hjälpa dig med?";

        [StringLength(200)]
        [MinLength(5, ErrorMessage = "Måste innehålla minst 5 tecken")]
        public string IsNotActiveMessage { get; set; } = "Chatten är tyvärr stängd";

        [StringLength(200)]
        [MinLength(5, ErrorMessage = "Måste innehålla minst 5 tecken")]
        public string RequestFailMessage { get; set; } = "Någonting gick fel, försök igen.";
    }
}
