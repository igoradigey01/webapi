using System.ComponentModel.DataAnnotations;

namespace ShopApi.Model.Identity
{
    public class LoginInputModelDto
    {

        public string? ReturnUrl { get; set; }

        // [Required(ErrorMessage = "Незадан Email")]       
        
        public string? Email { get; set; }

        public string? Phone { get; set; }

        [Required(ErrorMessage = "Незадан Password")]
        public string Password { get; set; } = String.Empty;

        [Required(ErrorMessage = "SpaClientId is required")]
        public string SpaId { get; set; } = string.Empty;  //  x-01 ,чей пользователь?,id spa client


        public bool RememberMe { get; set; }
    }
}