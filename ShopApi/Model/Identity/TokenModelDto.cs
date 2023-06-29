namespace ShopApi.Model.Identity
{
    public class TokenModelDto
    {
        public string? Access_token { get; set; }  // хранить в переменной на клиенте - like ( const accessToken = XYZ)
       // public string? Refresh_token { get; set; } - хранить в cookies  + httpOnly из соображений безопатности
       //Store the refresh token in httpOnly cookie: safe from CSRF, a bit better in terms of exposure to XSS.
    }
}