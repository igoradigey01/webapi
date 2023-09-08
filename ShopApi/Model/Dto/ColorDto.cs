namespace ShopAPI.Model
{
    public class ColorDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool Hidden { get; set; }

        public int TypeProductId { get; set; }

        public required string PostavchikId { get; set; }
    }
}