
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
    public class SubCatalogController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public SubCatalogController(ShopDbContext db)
        {
            _db = db;
        }

        [HttpGet("{owner_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SubCatalogDto>>> GetAll(string owner_id, int catalog_id)
        {
            var Subcatalogs = await (from b in _db.SubCatalogs!
                                     where b.OwnerId == owner_id && b.CatalogId == catalog_id
                                     select new SubCatalogDto()
                                     {
                                         Id = b.Id,
                                         OwnerId = b.OwnerId,
                                         CatalogId = b.CatalogId,
                                         GoogleTypeId = b.GoogleTypeId,

                                         Name = b.Name,
                                         Hidden = b.Hidden,
                                         DecriptSeo = b.DecriptSeo
                                     }).ToListAsync();
            if (Subcatalogs == null) return NotFound();


            return Ok(Subcatalogs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<SubCatalogDto>> GetItem(int id)
        {
            var item = await _db.SubCatalogs.Select(d => new SubCatalogDto()
            {
                Id = d.Id,
                OwnerId = d.OwnerId,
                CatalogId = d.CatalogId,
                GoogleTypeId = d.GoogleTypeId,

                Name = d.Name,
                Hidden = d.Hidden,
                DecriptSeo = d.DecriptSeo
            }
            )
            .SingleOrDefaultAsync(c => c.Id == id);

            if (item == null) return NotFound();

            return Ok(item);
        }

         // (post) создать
        [HttpPost]
        public async Task<ActionResult<SubCatalogDto>> Create(SubCatalog item)
        {
            
              if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            _db.SubCatalogs.Add(item);
            await _db.SaveChangesAsync();



            var dto = new SubCatalogDto()
            {
                Id = item.Id,
                OwnerId = item.OwnerId,
                CatalogId = item.CatalogId,
                GoogleTypeId = item.GoogleTypeId,

                Name = item.Name,
                Hidden = item.Hidden,
                DecriptSeo = item.DecriptSeo
            };

            return CreatedAtRoute("GetItem", new { id = item.Id }, dto);   
        }

         [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, SubCatalog item)
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
                if (!SubCatalogdExists(id))
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
          
            SubCatalog? item = await _db.SubCatalogs.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.SubCatalogs.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }


       
        private bool SubCatalogdExists(int id){
             return _db.SubCatalogs.Count(e => e.Id == id) > 0;

        }


    }
}