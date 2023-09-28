
using Microsoft.AspNetCore.Mvc;
using ShopApi.Model.Identity;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ShopDB;


namespace ShopAPI.Controllers
{

    [ApiController]
    [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
    [Route("api/[controller]/[action]")]
    public class ColorController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public ColorController(ShopDbContext db)
        {
            _db = db;
        }

        [HttpGet("{owner_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ColorDto>>> GetForProduct_type(string  owner_id)
        {
            // int i = 0;
            var colors = await (from b in _db.Colors
                                 where b.OwnerId == owner_id
                                select new ColorDto()
                                {
                                    Id = b.Id,
                                     OwnerId=b.OwnerId,
                                    Name = b.Name,
                                   
                                    Product_type_id = b.Product_typeId,
                                    Hidden = b.Hidden
                                }).ToListAsync();

            if (colors == null)return NotFound();

            return Ok(colors);

        }

        [HttpGet("{owner_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ColorDto>>> GetAll( string  owner_id, int product_type_id)
        {
            // int i = 0;
            var colors = await (from item in _db.Colors
                                where item.OwnerId==owner_id && item.Product_typeId == product_type_id
                                select new ColorDto()
                                {
                                    Id = item.Id,
                                     OwnerId=item.OwnerId,
                                    Name = item.Name,                                   
                                    Product_type_id = item.Product_typeId,
                                    Hidden = item.Hidden
                                }).ToListAsync();
            if (colors == null) return NotFound();

            return Ok(colors);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ColorDto>> GetItem(int id)
        {
            var item = await _db.Colors!.Select(d => new ColorDto
            {
                Id = d.Id,
                 OwnerId=d.OwnerId,
                Name = d.Name,                
                Product_type_id = d.Product_typeId,
                Hidden = d.Hidden
            }
              )
              .SingleOrDefaultAsync(c => c.Id == id);

            if (item == null)return NotFound();

            return Ok(item);

        }


        //  (post) создать
        [HttpPost]
        public async Task<ActionResult<ColorDto>> Create(Color item)
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



        //  (put) -изменить
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Color item)
        {

              if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            _db.Entry(item).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); //204


        }

        // DELETE api/<CategoriaController>/5       
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            
            Color? item = await _db.Colors!.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.Colors.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }


           private bool ColordExists(int id)
        {
            return _db.Brands!.Count(e => e.Id == id) > 0;
        }
    }
}

