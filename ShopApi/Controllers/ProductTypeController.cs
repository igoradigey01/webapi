using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.Model.Identity;
using ShopAPI.Model;
using ShopDB;



namespace ShopAPI.Controllers
{


    [ApiController]
    [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
    [Route("api/[controller]/[action]")]
    public class ProductTypeController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public ProductTypeController(ShopDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Product_TypeDto>>> GetAll()
        {



            var product_type = await (from b in _db.Product_Types
                                
                                  select new Product_TypeDto()
                                  {
                                      Id = b.Id,                                       
                                      Name = b.Name, 
                                      Hidden = b.Hidden
                                  }).ToListAsync();

            if (product_type == null) return NotFound();

            return Ok(product_type);


        }

    

        

   
    }
}
