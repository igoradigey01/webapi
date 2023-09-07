using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ShopApi.Model.Identity;
using Microsoft.EntityFrameworkCore;
using ShopDB;


namespace ShopAPI.Controllers
{


    [ApiController]
    [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
    [Route("api/[controller]/[action]")]
    public class ArticleController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public ArticleController(ShopDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<ArticleDto>> GetAll()
        {



            var articles = await (from b in _db.Articles!
                                  select new ArticleDto()
                                  {
                                      Id = b.Id,
                                      Name = b.Name,
                                      PostavchikId = b.PostavchikId,
                                      TypeProductId = b.TypeProductId,
                                      Hidden = b.Hidden
                                  }).ToListAsync();

            return articles;


        }

        [HttpGet("{idPostavchik}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetPostavchik(string idPostavchik)
        {
            // int i = 0;
            var articles = await (from item in _db.Articles!
                                  where item.PostavchikId == idPostavchik
                                  select new ArticleDto()
                                  {
                                      Id = item.Id,
                                      Name = item.Name,
                                      PostavchikId = item.PostavchikId,
                                      TypeProductId = item.TypeProductId,
                                      Hidden = item.Hidden
                                  }).ToListAsync();
            if (articles == null) NotFound();

            return Ok(articles);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArticleDto>> GetItem(int id)
        {
            var item = await _db.Articles!.Select(d => new ArticleDto
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
        public async Task<ActionResult<ArticleDto>> Create(Article item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            _db.Articles!.Add(item);
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
        public async Task<ActionResult> Update(int id, Article item)
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
                if (!ArticleExists(id))
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
        public async Task<ActionResult<Article>> Delete(int id)
        {


            Article? item = await _db.Articles!.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.Articles.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);

        }

        private bool ArticleExists(int id)
        {
            return _db.Articles!.Count(e => e.Id == id) > 0;
        }
    }
}
