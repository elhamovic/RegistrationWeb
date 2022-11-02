using System.ComponentModel.DataAnnotations;

namespace RegistrationWeb.Models
{
    /// <summary>
    /// The model contains all the data needed to create a user opject.
    /// </summary>
    public class UserModel
    {
        [Required(ErrorMessage = "Please enter the Username.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter the Password.")]
        public string Password { get; set; }
        public string Role { get; set; }
        public string Track { get; set; }

    }
}
