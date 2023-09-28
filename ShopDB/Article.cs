

namespace ShopDB;

public partial class Article
{
    public int Id { get; set; }

    public required string OwnerId { get; set; }  

    public int Product_typeId { get; set; }

    public required string Name { get; set; }

    public bool Hidden { get; set; }
   

    public virtual Product_type? Product_Type { get; set; }
     public virtual ICollection<Product> Product { get; set; } = new List<Product>();
    
}
