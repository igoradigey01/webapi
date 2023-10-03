using Microsoft.AspNetCore.Mvc;
using ShopApi.Model.Identity;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ShopDB;

namespace ShopAPI.Controllers;



    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ProductСonsistController : ControllerBase // Соств Product
    {
          private readonly ShopDbContext _db;

        public ProductСonsistController(ShopDbContext db)
        {
            _db = db;
        }


        
       [HttpGet("{id}")]       
        public async Task<ActionResult<IEnumerable<ProductСonsistDto>>> GetAll( int id)
        {
            var nomenclatures = await (from item in _db.ProductNomenclatures
                                 where item.Id==id
                                select new ProductСonsistDto()
                                {
                                    Id=item.Id,
                                   NomenclatureId=item.NomenclatureId,
                                    ProductId=item.ProductId
                                   

                                }).ToListAsync();

            if (nomenclatures == null) return NotFound();

            return Ok(nomenclatures);


        }

    }