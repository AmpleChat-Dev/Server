using System.ComponentModel.DataAnnotations;

namespace AmpleChatServer.Models {
    public class RegisterModel
    {
        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string UserName { get; set; }

        // Email minimumlength is 3 chars [0]name [1]@ [2]domain
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Email { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 8)]
        public string Password { get; set; }

        public bool IsValid()
        {
            var email = !string.IsNullOrEmpty(Email);
            var username = !string.IsNullOrEmpty(UserName);
            var password = !string.IsNullOrEmpty(Password);

            return email && username && password;
        }
    }
}
