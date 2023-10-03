
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
    public class ProductPhotoController : ControllerBase // Соств Product
    {
          private readonly ShopDbContext _db;

        public ProductPhotoController(ShopDbContext db)
        {
            _db = db;
        }


       [HttpGet("{id}")]       
        public async Task<ActionResult<IEnumerable<PhotoDto>>> GetAll( int id)
        {
            var photos = await (from item in _db.Photos
                                 where item.Id==id
                                select new PhotoDto()
                                {
                                    Id=item.Id,
                                    Guid=item.Guid,
                                    ProductId=item.ProductId
                                   

                                }).ToListAsync();

            if (photos == null) return NotFound();

            return Ok(photos);


        }

            [HttpPost("{id}")]
        public async Task<ActionResult<PhotoDto>> Create(int id,)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            _db.Colors!.Add(item);
            await _db.SaveChangesAsync();



            var dto = new ColorDto()
            {
                Id = item.Id,
                 OwnerId=item.OwnerId,
                Name = item.Name,
                Product_type_id = item.Product_typeId,
                Hidden = item.Hidden
            };

            return CreatedAtRoute("GetItem", new { id = item.Id }, dto);

        }





    }