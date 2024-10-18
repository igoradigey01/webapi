
namespace ShopDB;

public partial class Brand
{
    public int Id { get; set; }

    public required string OwnerId { get; set; }  

    public int Product_typeId { get; set; }   // fornityre mm korpus-mebel

    public required  string Name { get; set; }

    public bool Hidden { get; set; }=false;
    public virtual Product_type? Product_Type { get; set; }
    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
    
}
