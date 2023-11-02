using System.ComponentModel.DataAnnotations;
namespace ShopApi.Model.Identity
{
    public class ResetPasswordProfileDto
    {
        [Required(ErrorMessage = "OldPassword is required")]
        public required string OldPassword { get; set; }

        [Required(ErrorMessage = "NewPassword is required")]
        public required string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

              
        public  string? Email { get; set; }
        public  string? Phone { get; set; }
        
    }
}
