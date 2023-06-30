using Microsoft.AspNetCore.Identity;

namespace ShopApi.Model.Identity
{
     public class UserIdentityX01: IdentityUser{
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
       
    }
}
