﻿using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using ShopApi.Model.Identity;
using Microsoft.AspNetCore.Authorization;
using ShopDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

namespace ShopAPI.Controllers
{

    [ApiController]
    [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly ShopDbContext _db;
        private readonly ImageRepository _imageRepository;

        public ProductController(
             ShopDbContext db,
             ImageRepository imageRepository
            )
        {
            _db = db;
            _imageRepository = imageRepository;
        }


        [HttpGet("{owner_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(string owner_id)
        {
            // int i = 0;
            var products = await (from item in _db.Products
                                  where item.OwnerId == owner_id
                                  select new ProductDto()
                                  {
                                      Id = item.Id,
                                      Guid = item.Guid,
                                       Hidden=item.Hidden,
                                      OwnerId = item.OwnerId,
                                      Product_typeId = item.Product_typeId,
                                      Title = item.Title,

                                      SubCatalogId = item.SubCatalogId,
                                      ColorId = item.ColorId,
                                      BrandId = item.BrandId,
                                      ArticleId = item.ArticleId,

                                      Position = item.Position,

                                      InStock = item.InStock,
                                      Sale = item.Sale,

                                      Price = item.Price,

                                      Markup = item.Markup,

                                      Description = item.Description,
                                      DescriptionSeo = item.DescriptionSeo



                                  }).ToListAsync();

            if (products == null) return NotFound();

            return Ok(products);
        }

        [HttpGet("{owner_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetForProduct_type(string owner_id, int product_type_id)
        {
            var products = await (from item in _db.Products
                                  where item.OwnerId == owner_id && item.Product_typeId == product_type_id
                                  select new ProductDto()
                                  {
                                      Id = item.Id,
                                      Guid = item.Guid,
                                      OwnerId = item.OwnerId,
                                       Hidden=item.Hidden,
                                      Product_typeId = item.Product_typeId,
                                      Title = item.Title,

                                      SubCatalogId = item.SubCatalogId,
                                      ColorId = item.ColorId,
                                      BrandId = item.BrandId,
                                      ArticleId = item.ArticleId,

                                      Position = item.Position,

                                      InStock = item.InStock,
                                      Sale = item.Sale,

                                      Price = item.Price,

                                      Markup = item.Markup,

                                      Description = item.Description,
                                      DescriptionSeo = item.DescriptionSeo



                                  }).ToListAsync();

            if (products == null) return NotFound();

            return Ok(products);
        }

        [HttpGet("{owner_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetForSubCatalog(string owner_id, int idSubCatalog)
        {
            // int i = 0; 
            var products = await (from item in _db.Products
                                  where item.OwnerId == owner_id && item.SubCatalogId == idSubCatalog
                                  select new ProductDto()
                                  {
                                      Id = item.Id,
                                      Guid = item.Guid,
                                      Hidden=item.Hidden,
                                      OwnerId = item.OwnerId,
                                      Product_typeId = item.Product_typeId,
                                      Title = item.Title,

                                      SubCatalogId = item.SubCatalogId,
                                      ColorId = item.ColorId,
                                      BrandId = item.BrandId,
                                      ArticleId = item.ArticleId,

                                      Position = item.Position,

                                      InStock = item.InStock,
                                      Sale = item.Sale,

                                      Price = item.Price,

                                      Markup = item.Markup,

                                      Description = item.Description,
                                      DescriptionSeo = item.DescriptionSeo



                                  }).ToListAsync();

            if (products == null) return NotFound();

            return Ok(products);
        }



        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            var product = await (from item in _db.Products
                                 where item.Id == id
                                 select new ProductDto()
                                 {
                                     Id = item.Id,
                                     Guid = item.Guid!,
                                      Hidden=item.Hidden,
                                     OwnerId = item.OwnerId,
                                     Product_typeId = item.Product_typeId,
                                     Title = item.Title,

                                     SubCatalogId = item.SubCatalogId,
                                     ColorId = item.ColorId,
                                     BrandId = item.BrandId,
                                     ArticleId = item.ArticleId,

                                     Position = item.Position,

                                     InStock = item.InStock,
                                     Sale = item.Sale,

                                     Price = item.Price,

                                     Markup = item.Markup,

                                     Description = item.Description,
                                     DescriptionSeo = item.DescriptionSeo



                                 })
            .SingleOrDefaultAsync();

            if (product == null) return NotFound();

            return Ok(product);

        }



        [HttpPost] // (post) создать из [FromBody]        
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> Create()
        {
            // throw new NotImplementedException();

     
    
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            } 

          
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.
            Product product = new()
            {
                Id = 0,
                Guid = Guid.NewGuid().ToString(),
                Hidden=bool.Parse(form["hidden"]),
                OwnerId = form["ownerId"],
                Product_typeId = int.Parse(form["product_typeId"]),
                Title =  form["title"],

                SubCatalogId =int.Parse(form["subCatalogId"]),
                ColorId = int.Parse(form["colorId"]),
                BrandId = int.Parse(form["brandId"]),
                ArticleId =int.Parse(form["articleId"]),

                Position = int.Parse(form["position"]),

                InStock =bool.Parse(form["inStock"]),
                Sale =bool.Parse(form["sale"]),

                Price =int.Parse(form["price"]),

                Markup =int.Parse(form["markup"]),

                Description =form["description"],
                DescriptionSeo =    form["descriptionSeo"]



            };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8601 // Possible null reference assignment.




            if (form.Files.Count > 0)
            {
                var file = form.Files[0] as IFormFile;
                if (file == null) return BadRequest("Пустой файл Фото"); ;
                var guid = _imageRepository.RamdomName;

                _imageRepository.Save(guid, file.OpenReadStream());

               product.Guid = guid;
            }
            else
            {
              return BadRequest("form IFormFile ==null");

            }



            // Console.WriteLine("Task< ActionResult<Katalog>> Post(Katalog item)----"+item.Name +"-"+item.Id+"-"+item.Model);
          

      
       

#pragma warning disable CS8604 // Possible null reference argument.
            if (ProductNameExist(product.OwnerId,product.Title))
                return BadRequest("Такое имя товара уже существует!");
#pragma warning restore CS8604 // Possible null reference argument.

            _db.Products.Add(product);
            await _db.SaveChangesAsync();


         



            var dto = new ProductDto
            {

                Id = product.Id,
                Guid = product.Guid,
                 Hidden=product.Hidden,
                OwnerId = product.OwnerId,
                Product_typeId = product.Product_typeId,
                Title = product.Title,

                SubCatalogId = product.SubCatalogId,
                ColorId = product.ColorId,
                BrandId = product.BrandId,
                ArticleId = product.ArticleId,

                Position = product.Position,

                InStock = product.InStock,
                Sale = product.Sale,

                Price = product.Price,

                Markup = product.Markup,

                Description = product.Description,
                DescriptionSeo = product.DescriptionSeo



            };







            //-------------new ----------------------
            return CreatedAtRoute("GetItem", new { id = product.Id }, dto);
        }


        [HttpPut("{id}")] //  (put) -изменитьиз [FromBody]
        public async Task<ActionResult> Update(int id, [FromForm] ProductRequestDto item_c)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item_c.Id)
            {
                return BadRequest();
            }

            if (item_c.File.Length <= 0)
                return BadRequest("Пустой файл Фото");

            if(ProductNameExist(item_c.OwnerId,item_c.Title))
                return BadRequest("Такое имя товара уже существует!");    

             var guid = await (from puducts in _db.Products
                              where puducts.Id == id && puducts.Guid == item_c.Guid
                              select puducts.Guid).FirstOrDefaultAsync();

            if (String.IsNullOrEmpty(guid))
            {

                return BadRequest("guid неравны");

            }    

            Product product = new()
            {
                Id = item_c.Id,
                Guid = item_c.Guid!,
                Hidden=item_c.Hidden,
                OwnerId = item_c.OwnerId,
                Product_typeId = item_c.Product_typeId,
                Title = item_c.Title,

                SubCatalogId = item_c.SubCatalogId,
                ColorId = item_c.ColorId,
                BrandId = item_c.BrandId,
                ArticleId = item_c.ArticleId,

                Position = item_c.Position,

                InStock = item_c.InStock,
                Sale = item_c.Sale,

                Price = item_c.Price,

                Markup = item_c.Markup,

                Description = item_c.Description,
                DescriptionSeo = item_c.DescriptionSeo



            };



            _db.Entry(product).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            _imageRepository.Save(item_c.Guid!, item_c.File.OpenReadStream());





            return NoContent(); //204
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateIgnoreImg(int id, Product item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            if(ProductNameExist(item.OwnerId,item.Title))
                return BadRequest("Такое имя товара уже существует!");

            _db.Entry(item).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOnlyImg(int id, [FromForm] PhotoRequestDto item)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.IdProduct)
            {
                return BadRequest("id товара неравны");
            }

            var guid = await (from puducts in _db.Products
                              where puducts.Id == id && puducts.Guid == item.Guid
                              select puducts.Guid).FirstOrDefaultAsync();

            if (String.IsNullOrEmpty(guid))
            {

                return BadRequest("guid неравны");

            }

            _imageRepository.Save(item.Guid!, item.File.OpenReadStream());
            return NoContent(); //204

        }

        // DELETE api/<CategoriaController>/5       
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Product? item = await _db.Products.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.Products.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        private bool ProductExists(int id)
        {
            return _db.Products.Count(e => e.Id == id) > 0;
        }
        private bool ProductNameExist(string ownerId,string title){
             return _db.Products.Count(e => e.OwnerId == ownerId && e.Title ==title) > 0;

        }
    }
}
