
namespace ShopApi.Model.Identity
{
    public class ExternalGoogleDto
    {
        public required string Provider { get; set; }
       
        public required string  IdSpa {get;set;}   //  x-01 ,чей пользователь?,id spa client
        public required string IdUser { get; set; }
        public required string IdToken { get; set; }
    }
}
