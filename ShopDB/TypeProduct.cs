using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class TypeProduct  // корпус forniture  , мягкая fornityre  ,мягкая изделие ,корпус Изделие
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public bool Hidden { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Catalog> Catalogs { get; set; } = new List<Catalog>();

     public virtual ICollection<Color> Colors { get; set; } = new List<Color>();

     public virtual ICollection<Brand> Brands { get; set; } = new List<Brand>();

     public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
