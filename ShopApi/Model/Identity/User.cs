using Microsoft.AspNetCore.Identity;

namespace ShopApi.Model.Identity
{
     public class User: IdentityUser{
        public string FirstName { get; set; }=String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty; //delete               
        public string RefreshToken { get; set; } = String.Empty;

        public DateTime RefreshTokenExpiryTime{get;set;}
    }
}
