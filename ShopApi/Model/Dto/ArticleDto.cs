namespace ShopAPI.Model
{
    public class ArticleDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool Hidden { get; set; }

        public int Product_typeId { get; set; }
       
    }
}