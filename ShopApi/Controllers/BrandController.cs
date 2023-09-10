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
    public class BrandController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public BrandController(
            ShopDbContext db
            )
        {
          _db = db;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task< ActionResult< IEnumerable<BrandDto>>> GetAll()
        {
            var brands = await (from b in _db.Brands!
                                  select new BrandDto()
                                  {
                                      Id = b.Id,
                                      Name = b.Name,
                                     // PostavchikId = b.PostavchikId,
                                      Product_type_id = b.Product_typeId,
                                      Hidden = b.Hidden
                                  }).ToListAsync();
             if (brands == null)return NotFound();
                      

            return Ok( brands);
        }

        [HttpGet("{product_type_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAll(int product_type_id)
        {
            // int i = 0;
           var barnds = await (from item in _db.Brands!
                                  where item.Product_typeId == product_type_id
                                  select new BrandDto()
                                  {
                                      Id = item.Id,
                                      Name = item.Name,                                     
                                      Product_type_id = item.Product_typeId,
                                      Hidden = item.Hidden
                                  }).ToListAsync();
            if (barnds == null)return NotFound();

            return Ok(barnds);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult< BrandDto>> GetItem(int id)
        {
             var item = await _db.Brands!.Select(d => new BrandDto
            {
                Id = d.Id,
                Name = d.Name,
                
                Product_type_id = d.Product_typeId,
                Hidden = d.Hidden
            }
            )
            .SingleOrDefaultAsync(c => c.Id == id);

            if (item == null) return NotFound();

            return Ok(item);
            //  throw new Exception("NOt Implimetn Exception");
        }

      
        //  (post) создать
        [HttpPost]
        public async Task<ActionResult<BrandDto>> Create(Brand item)
        {
              if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            _db.Brands!.Add(item);
            await _db.SaveChangesAsync();



            var dto = new BrandDto()
            {
                Id = item.Id,
                Name = item.Name,               
                Product_type_id = item.Product_typeId,
                Hidden = item.Hidden
            };

            return CreatedAtRoute("GetItem", new { id = item.Id }, dto);
            
        }


        //  (put) -изменить
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Brand item)
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
                if (!BrandExists(id))
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

           
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
          
            Brand? item = await _db.Brands!.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.Brands.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }


        private bool BrandExists(int id)
        {
            return _db.Brands!.Count(e => e.Id == id) > 0;
        }
    }
}
