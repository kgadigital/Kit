using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace KitAplication.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vänligen fyll i lösenord")]
        [DisplayName("Lösenord")]
        //[BindProperty]
        public string Password { get; set; } = null!;
    }
}
