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
public class ProductСonsistController : ControllerBase // Соств ProductNomenclatura
{
    private readonly ShopDbContext _db;

    public ProductСonsistController(ShopDbContext db)
    {
        _db = db;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<ProductNomenclatureDto>>> GetAll(int id)
    {
        var nomenclatures = await (from item in _db.ProductNomenclatures
                                   where item.Id == id
                                   select new ProductNomenclatureDto()
                                   {
                                       Id = item.Id,
                                       NomenclatureId = item.NomenclatureId,
                                       ProductId = item.ProductId


                                   }).ToListAsync();

        if (nomenclatures == null) return NotFound();

        return Ok(nomenclatures);


    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductNomenclatureDto>> GetItem(int id)
    {
        var item = await _db.ProductNomenclatures.Select(item => new ProductNomenclatureDto
        {
            Id = item.Id,
            NomenclatureId = item.NomenclatureId,
            ProductId = item.ProductId
        }
          )
          .SingleOrDefaultAsync(c => c.Id == id);

        if (item == null) return NotFound();

        return Ok(item);

    }



    [HttpPost] //  (post) создать
    public async Task<ActionResult<ProductNomenclatureDto>> Create(ProductNomenclature item)
    {


        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }



        _db.ProductNomenclatures.Add(item);
        await _db.SaveChangesAsync();



        var dto = new ProductNomenclatureDto()
        {
            Id = item.Id,
            ProductId = item.ProductId,
            NomenclatureId = item.NomenclatureId
        };

        return CreatedAtRoute("GetItem", new { id = item.Id }, dto);

    }


    //  (put) -изменить
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, ProductNomenclature item)
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
            if (!ProductNomenclatureExists(id))
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
        ProductNomenclature? item = await _db.ProductNomenclatures.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _db.ProductNomenclatures.Remove(item);
        await _db.SaveChangesAsync();
        return Ok(item);
    }



    private bool ProductNomenclatureExists(int id)
    {
        return _db.ProductNomenclatures.Count(e => e.Id == id) > 0;
    }





}