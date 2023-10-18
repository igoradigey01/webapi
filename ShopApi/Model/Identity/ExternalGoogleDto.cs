
namespace ShopApi.Model.Identity
{
    public class ExternalGoogleDto
    {
        public string Provider { get; set; }
       
        public string  SpaId {get;set;} =string.Empty;  //  x-01 ,чей пользователь?,id spa client
        public string IdUser { get; set; }
        public string IdToken { get; set; }
    }
}
