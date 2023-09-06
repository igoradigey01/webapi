using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopDB;

namespace ShopAPI.Model.DB_Repositorys
{
    public class ArticleRepository
    {
        private readonly ShopDbContext _db;

        public ArticleRepository(ShopDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Article>> Get()
        {
            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.Articles!.ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetPostavchik(string idPostavchik )
        {
            return await _db.Articles!.Where(a=>a.PostavchikId == idPostavchik).ToListAsync(); 
        }

        public async Task<Article?> Item(int idArticle)
        {

            return await _db.Articles!.SingleOrDefaultAsync(c => c.Id == idArticle);
        }

        public async Task<DtoRepositoryResponse> Create(Article item)
        {
            DtoRepositoryResponse flag = new (){ Flag = false, Message = null };
            await _db.Articles!.AddAsync(item);
            int i = await _db.SaveChangesAsync();

            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

            if (i != 0)
            {
                flag.Flag = true;
                flag.Message = "БД add ok!";
                flag.Item = item;
                return flag;
            }
            else
            {
                flag.Flag = false;
                flag.Message = "БД add not!";
                return flag;
            }

        }

        public async Task<DtoRepositoryResponse> Update(Article item)
        {
            var flag = new DtoRepositoryResponse { Flag = false, Message = null, Item = null };
            Article? selectItem = await _db.Articles!.FirstOrDefaultAsync(c => c.Id == item.Id);
            if (selectItem == null)
            {

                flag.Message = "Товар с таким id  в БД ненайден";
                return flag;

            }
            selectItem.Name = item.Name.Trim();//23.02.22
            selectItem.PostavchikId = item.PostavchikId;

            selectItem.Hidden = item.Hidden;

            _db.Articles!.Update(selectItem);

            int i = await _db.SaveChangesAsync();
            if (i != 0)
            {
                flag.Message = "БД update ok!";
                flag.Flag = true;
                flag.Item = selectItem;
                return flag;
            }
            else
            {
                flag.Message = "БД update not(false)!";
                flag.Flag = false;
                flag.Item = selectItem;
                return flag;
            }




        }

        public async Task<DtoRepositoryResponse> Delete(int id)
        {
            DtoRepositoryResponse flagValid = new() { Flag = false, Message = "" };
            Article? item = await _db.Articles!.FirstOrDefaultAsync(c => c.Id == id);
            if (item == null)
            {
                flagValid.Message = "Каталога с таким id не существует!";
                flagValid.Flag = false;
                return flagValid;
            }
            _db.Articles!.Remove(item);
            int i = await _db.SaveChangesAsync();
            if (i != 0)
            {
                flagValid.Flag = true;
                flagValid.Message = "БД delete ok";
                return flagValid;
            }
            else
            {
                flagValid.Message = "БД delete not";
                flagValid.Flag = false;
                return flagValid;
            }

        }
    }
}
