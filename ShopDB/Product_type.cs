using System;
using System.Collections.Generic;

namespace ShopDB;


// https://support.google.com/merchants/answer/6324406

public partial class Product_type  // корпус forniture  , мягкая fornityre  ,мягкая изделие ,корпус Изделие
{
    public int Id { get; set; }

    public required string Name { get; set; }
    

    public bool Hidden { get; set; }

     public virtual ICollection<Product> Products { get; set; } = new List<Product>();

     public virtual ICollection<Color> Colors { get; set; } = new List<Color>();

     public virtual ICollection<Brand> Brands { get; set; } = new List<Brand>();

     public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
