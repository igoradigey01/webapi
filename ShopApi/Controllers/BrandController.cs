using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using ShopApi.Model.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<BrandDto>> GetAll()
        {
            var brands = await (from b in _db.Brands!
                                  select new BrandDto()
                                  {
                                      Id = b.Id,
                                      Name = b.Name,
                                      PostavchikId = b.PostavchikId,
                                      TypeProductId = b.TypeProductId,
                                      Hidden = b.Hidden
                                  }).ToListAsync();

            return brands;
        }

        [HttpGet("{idPostavchik}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetPostavchik(string idPostavchik)
        {
            // int i = 0;
           var barnds = await (from item in _db.Brands!
                                  where item.PostavchikId == idPostavchik
                                  select new BrandDto()
                                  {
                                      Id = item.Id,
                                      Name = item.Name,
                                      PostavchikId = item.PostavchikId,
                                      TypeProductId = item.TypeProductId,
                                      Hidden = item.Hidden
                                  }).ToListAsync();
            if (barnds == null) NotFound();

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
                PostavchikId = d.PostavchikId,
                TypeProductId = d.TypeProductId,
                Hidden = d.Hidden
            }
            )
            .SingleOrDefaultAsync(c => c.Id == id);

            if (item == null) NotFound();

            return Ok(item);
            //  throw new Exception("NOt Implimetn Exception");
        }

        // POST api/<CategoriaController>
        // api/Material (post) создать
        [HttpPost]
        public async Task<ActionResult<BrandDto>> Create(Brand item)
        {
              if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            _db.Brands!.Add(item);
            await _db.SaveChangesAsync();



            var dto = new ArticleDto()
            {
                Id = item.Id,
                Name = item.Name,
                PostavchikId = item.PostavchikId,
                TypeProductId = item.TypeProductId,
                Hidden = item.Hidden
            };

            return CreatedAtRoute("GetArticle", new { id = item.Id }, dto);
            
        }


        // public void Put(int id, [FromBody] string value)     

        // PUT api/material/3 (put) -изменить
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

        // DELETE api/<CategoriaController>/5       
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
