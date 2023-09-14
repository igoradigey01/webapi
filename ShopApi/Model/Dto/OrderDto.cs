namespace ShopAPI.Model
{
    public class OrderDto
    {
        public int Id { get; set; }

        public required string OrderNumber { get; set; }
        public required string OrderDate { get; set; }


    }
}