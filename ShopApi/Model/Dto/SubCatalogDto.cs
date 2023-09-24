namespace ShopAPI.Model;

public class SubCatalogDto
{

    public int Id { get; set; }
    public required string OwnerId { get; set; }
    public int CatalogId { get; set; }
    public string? GoogleTypeId { get; set; }
    
    public required string Name { get; set; }
    public bool Hidden { get; set; }
    public string? DecriptSeo { get; set; }




    // владелец 

}