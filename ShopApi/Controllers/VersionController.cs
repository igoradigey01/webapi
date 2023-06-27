
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
        string _version = "b2.03.23";
        string _description = "Api shop- вторая редакция ( aspnetcore -net5.0)(24.03.23)";


       



        [HttpGet]
        public VersionInfo Info()
        {
            return new VersionInfo { Version = _version, Description = _description }; // отправка в формате json  (-error parsing angular response)

        }

    }
}
