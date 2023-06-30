namespace ShopApi.Model.Identity
{
    public class UserTelegramDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "SpaClientId is required")]
        public string  SpaId {get;set;} =string.Empty;  //  x-01 ,чей пользователь?,id spa client
        public string? FirstName { get; set; }
        public string? UserName { get; set; }
        public string? AuthDate { get; set; }
        public string? Hash { get; set; }
    }
}
