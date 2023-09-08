
namespace ShopDB;

public partial class SubCatalog
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public bool Hidden { get; set; }

    public string? DecriptSeo { get; set; }

    public int CatalogId { get; set; }

    public virtual Catalog Catalog { get; set; }=null!;

    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}
