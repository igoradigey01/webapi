namespace ShopAPI.Model
{
    public class OrderStateDto
    {
        public int Id { get; set; }

        public required string StateName{get;set;}

        public required string SmallName{get;set;}

        public required string Description {get;set;}
    }
}  