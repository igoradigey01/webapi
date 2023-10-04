
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
public class ProductPhotosController : ControllerBase // Соств Product
{
    private readonly ShopDbContext _db;
    private readonly ImageRepository _imageRepository;

    public ProductPhotosController(
        ShopDbContext db,
         ImageRepository imageRepository
        )
    {
        _db = db;
        _imageRepository = imageRepository;

    }


    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<PhotoDto>>> GetAll(int id)
    {
        var photos = await (from item in _db.Photos
                            where item.Id == id
                            select new PhotoDto()
                            {
                                Id = item.Id,
                                Guid = item.Guid,
                                ProductId = item.ProductId


                            }).ToListAsync();

        if (photos == null) return NotFound();

        return Ok(photos);


    }


    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<PhotoDto>> GetItem(int id)
    {
        var item = await _db.Photos.Select(d => new PhotoDto
        {
            Id = d.Id,
            Guid = d.Guid,
            ProductId = d.ProductId
        }
          )
          .SingleOrDefaultAsync(c => c.Id == id);

        if (item == null) return NotFound();

        return Ok(item);

    }



    [HttpPost("{id}")]
    public async Task<ActionResult<PhotoDto>> Create(int id, [FromForm] PhotoRequestDto item)
    {


        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != item.IdProduct)
        {
            return BadRequest("id товара неравны");
        }

        if (item.File!.Length <= 0)
            return BadRequest("Пустой файл Фото");


        Photo photo = new()
        {
            Id = 0,
            Guid = Guid.NewGuid().ToString(),
            ProductId = item.IdProduct
        };



        _db.Photos.Add(photo);
        await _db.SaveChangesAsync();

        _imageRepository.Save(photo.Guid, item.File.OpenReadStream());


        var dto = new PhotoDto()
        {
            Id = photo.Id,
            Guid = photo.Guid,
            ProductId = photo.ProductId

        };

        return CreatedAtRoute("GetItem", new { id = photo.Id }, dto);

    }


    //  (put) -изменить
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromForm] PhotoRequestDto item)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != item.Id)
        {
            return BadRequest();
        }

        if (item.File.Length <= 0)
            return BadRequest("Пустой файл Фото");


        var guid = await (from d in _db.Photos
                          where d.Id == id && d.Guid == item.Guid
                          select d.Guid).FirstOrDefaultAsync();

        if (String.IsNullOrEmpty(guid))
        {

            return BadRequest("guid неравны");

        }

        Photo photo = new()
        {
            Id = item.Id,
            Guid = item.Guid!,
            ProductId = item.IdProduct
        };



        _db.Entry(item).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PhotoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        _imageRepository.Save(photo.Guid, item.File.OpenReadStream());

        return NoContent(); //204


    }

    //  (put) -изменить
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOnlyImg(int id, [FromForm] PhotoRequestDto item_c)
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


        var guid = await (from d in _db.Photos
                          where d.Id == id && d.Guid == item_c.Guid
                          select d.Guid).FirstOrDefaultAsync();

        if (String.IsNullOrEmpty(guid))
        {

            return BadRequest("guid неравны");

        }


        _imageRepository.Save(item_c.Guid!, item_c.File.OpenReadStream());

        return NoContent(); //204


    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        Photo? item = await _db.Photos.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _db.Photos.Remove(item);
        await _db.SaveChangesAsync();
        return Ok(item);
    }


    private bool PhotoExists(int id)
    {
        return _db.Photos.Count(e => e.Id == id) > 0;
    }




}