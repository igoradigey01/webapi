using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Model.Identity
{
     public class UserIdentityX01: IdentityUser{
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "SpaClientId is required")]
        public string  SpaId {get;set;} =string.Empty;  //  x-01 ,чей пользователь?,id spa client
        public string? Address { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
       
    }
}
