using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class Color
{
    public int Id { get; set; }

    public int Product_typeId { get; set; }

   
    public required string Name { get; set; }

    public bool Hidden { get; set; }

    public virtual Product_type Product_Type { get; set; }=null!;
    
     public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}
