using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Model.Identity;
using System.Collections.Generic;
//-----------------
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopAPI.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<UserIdentityX01> userManager;

        public RoleController(
            UserManager<UserIdentityX01> userManager
            )
        {
            this.userManager = userManager;
        }

        // GET api/role
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IEnumerable<string>> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            if(!String.IsNullOrEmpty(userId)){
            var user = await userManager.FindByIdAsync(userId);
            if(user!=null){
            var role = await userManager.GetRolesAsync(user);
            return role;
            }
            }
             return new List<string>();
        }
    }
}