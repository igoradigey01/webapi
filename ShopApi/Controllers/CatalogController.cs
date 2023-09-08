﻿
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<CatalogDto>> GetAll()
        {
            var catalogs = await (from b in _db.K!
                                  select new CatalogDto()
                                  {
                                      Id = b.Id,
                                      Name = b.Name,
                                      PostavchikId = b.PostavchikId,
                                      TypeProductId = b.TypeProductId,
                                      Hidden = b.Hidden
                                  }).ToListAsync();
             if (catalogs == null) NotFound();
                      

            return Ok( catalogs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IEnumerable<CategoriaN>> GetPostavchik(int id)
        {
            // int i = 0;
            return await _repository.GetPostavchik(id);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<CategoriaN> Item(int id)
        {
            return await _repository.Item(id);
            //  throw new Exception("NOt Implimetn Exception");
        }

        // POST api/<CategoriaController>
        // api/Material (post) создать
        [HttpPost]
        public async Task<ActionResult<CategoriaN>> Create()
        {


            CategoriaN item = new CategoriaN();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }

            item.Id = int.Parse(form["id"]);


            item.Name = form["name"];
            item.Name = item.Name.Trim();

            item.Hidden = bool.Parse(form["hidden"]);
            item.PostavchikId = int.Parse(form["postavchikId"]);
            item.DecriptSEO = form["decriptSEO"];
            




            // Console.WriteLine("Task< ActionResult<Katalog>> Post(Katalog item)----"+item.Name +"-"+item.Id+"-"+item.Model);
            var flag = await _repository.Create(item);
            if (flag.Flag)
            {
                return Ok(flag.Item as CategoriaN);
            }
            else
            {
                return BadRequest(flag.Message);
            }
        }


        // public void Put(int id, [FromBody] string value)     

        // PUT api/material/3 (put) -изменить
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id)
        {

            CategoriaN item = new CategoriaN();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }

            item.Id = int.Parse(form["id"]);
            if (item.Id != id)
            {
                return BadRequest("Неверный Id");
            }

            item.Name = form["name"];
            item.Name = item.Name.Trim();

            item.Hidden = bool.Parse(form["hidden"]);
            item.PostavchikId = int.Parse(form["postavchikId"]);
            item.DecriptSEO = form["decriptSEO"];





            // if(id!=item.Id) return BadRequest();
            var flag = await _repository.Update(item);
            if (flag.Flag)
            {
                // Katalog katalog = flag.Item as Katalog;
                //  Console.WriteLine(katalog.Name + "-----" + katalog.Id);
                return Ok();

            }


            return BadRequest(flag.Message);
        }

        // DELETE api/<CategoriaController>/5       
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var flagValid = await _repository.Delete(id);
            if (!flagValid.Flag)
            {
                return BadRequest(flagValid.Message);
            }
            return Ok();
        }
    }
}
