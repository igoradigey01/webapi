using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderDB;
using ShopApi.Model.Identity;
using ShopAPI.Model;
using OrderDB;



namespace ShopAPI.Controllers
{


    [ApiController]
    [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _db;

        public OrderController(OrderDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetAll()
        {



            var orders = await (from b in _db.Orders!
                                  select new OrderDto()
                                  {
                                      Id = b.Id,
                                      OrderDate = b.OrderDate,
                                      OrderNumber=b.OrderNumber
                                     
                                  }).ToListAsync();

            if (orders == null) return NotFound();

            return Ok(orders);


        }

    }
}