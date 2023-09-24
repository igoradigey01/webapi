
using Microsoft.AspNetCore.Mvc;
using ShopApi.Model.Identity;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using ShopDB;
using Microsoft.EntityFrameworkCore;

namespace ShopAPI.Controllers
{
   
    [ApiController]
    [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
    [Route("api/[controller]/[action]")]
    public class CatalogController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public CatalogController( ShopDbContext db )
        {
           _db = db;
        }

        [HttpGet("{owner_id}")]
        [AllowAnonymous]
        public async Task<ActionResult< IEnumerable<CatalogDto>>> GetAll(string  owner_id )
        {
            var catalogs = await (from b in _db.Catalogs!
                                    where b.OwnerId == owner_id
                                  select new CatalogDto()
                                  {
                                      Id = b.Id,
                                      Name = b.Name,
                                      OwnerId = b.OwnerId,                                     
                                      Hidden = b.Hidden,
                                      DecriptSEO=b.DecriptSeo
                                  }).ToListAsync();
             if (catalogs == null) return NotFound();
                      

            return Ok( catalogs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult< CatalogDto>> GetItem(int id)
        {
            var item = await _db.Catalogs.Select(d => new CatalogDto
            {
                Id = d.Id,
                Name = d.Name,                
                OwnerId = d.OwnerId,
                DecriptSEO=d.DecriptSeo,
                Hidden = d.Hidden
            }
            )
            .SingleOrDefaultAsync(c => c.Id == id);

            if (item == null) return NotFound();

            return Ok(item);
        }

        // (post) создать
        [HttpPost]
        public async Task<ActionResult<CatalogDto>> Create(Catalog item)
        {
            
              if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            _db.Catalogs.Add(item);
            await _db.SaveChangesAsync();



            var dto = new CatalogDto()
            {
                Id = item.Id,
                Name = item.Name,               
                OwnerId =item.OwnerId,
                DecriptSEO=item.DecriptSeo,
                Hidden =item.Hidden
            };

            return CreatedAtRoute("GetItem", new { id = item.Id }, dto);   
        }


        //  (put) -изменить
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Catalog item)
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
                if (!CatalogdExists(id))
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
          
            Catalog? item = await _db.Catalogs.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.Catalogs.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }


        private bool CatalogdExists(int id){
             return _db.Catalogs.Count(e => e.Id == id) > 0;

        }
    }
}
