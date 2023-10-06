

namespace ShopDB;


public partial class Catalog
{
    public int Id { get; set; }
   

     public required string OwnerId { get; set; } // владелец

    public required string Name { get; set; }

    public bool Hidden { get; set; }

    public string? DecriptSeo { get; set; }

    public virtual ICollection<SubCatalog> SubCatalogs { get; set; } = new List<SubCatalog>();
}

