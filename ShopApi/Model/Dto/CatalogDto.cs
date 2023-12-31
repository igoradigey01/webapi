namespace ShopAPI.Model
{
    public class CatalogDto
    {
      
        public int Id { get; set; }
        public required string Name { get; set; }     
        
        public bool Hidden { get; set; }
        public string? DecriptSEO { get; set; }
        public required string   OwnerId { get; set; }       // владелец 
       
    }
}