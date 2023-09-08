using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class Catalog
{
    public int Id { get; set; }
    public int TypeProductId { get; set; }

     public required string PostavchikId { get; set; }

    public required string Name { get; set; }

    public bool Hidden { get; set; }

    public string? DecriptSeo { get; set; }

    public virtual ICollection<SubKatalog> SubKatalogs { get; set; } = new List<SubKatalog>();
}
