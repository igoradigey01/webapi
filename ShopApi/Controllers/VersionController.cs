
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers
{

    public class VersionInfo
    {
        public string Version { get; set; } = "";
        public string Description { get; set; } = "";
    }

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        string _version = "b2.06.23";
        string _description = "Api shop- вторая редакция ( aspnetcore -net7.0)(16.07.23)";


       



        [HttpGet]
        public VersionInfo Info()
        {
            return new VersionInfo { Version = _version, Description = _description }; // отправка в формате json  (-error parsing angular response)

        }

    }
}
