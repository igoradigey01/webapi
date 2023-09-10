namespace ShopAPI.Model
{
    public class BrandDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool Hidden { get; set; }

        public int Product_type_id { get; set; }

        
    }
}