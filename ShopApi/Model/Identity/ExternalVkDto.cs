namespace ShopApi.Model.Identity
{
    public class ExternalVkDto
    {
        public string? Provider { get; set; }
        public string IdSpa {get;set;} =string.Empty; // idspa: x-01, idspa:xl-01

        public string? IdUser { get; set; }
        public string? IdApp { get; set; }
        public string? IdToken { get; set; }
        public string? Photo_rec_url { get; set; }
        public string? First_name { get; set; }
        public string? Last_name { get; set; }
        public string? Hash { get; set; }
    }
 
}
