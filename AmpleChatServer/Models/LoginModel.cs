using System.ComponentModel.DataAnnotations;

namespace AmpleChatServer.Models {
    public class LoginModel
    {
        [Required]
        public string UserNameOrEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
