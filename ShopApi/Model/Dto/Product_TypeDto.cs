namespace ShopAPI.Model
{
    public class Product_TypeDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool Hidden { get; set; }

    }
}