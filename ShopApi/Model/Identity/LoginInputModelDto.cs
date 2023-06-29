using System.ComponentModel.DataAnnotations;

namespace ShopApi.Model.Identity
{
    public class LoginInputModelDto
    {
     
        public string? ReturnUrl { get; set; }

        [Required(ErrorMessage = "Незадан Email")]       
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Незадан Password")]
        public string? Password { get; set; }

        
        public bool RememberMe { get; set; }
    }
}