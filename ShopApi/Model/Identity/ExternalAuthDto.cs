﻿namespace ShopApi.Model.Identity
{
    public class ExternalAuthDto
    {
        public string? Provider { get; set; }
        public string? IdUser { get; set; }
        public string? IdToken { get; set; }
    }
}
