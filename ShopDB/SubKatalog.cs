using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class SubKatalog
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public bool Hidden { get; set; }

    public string? DecriptSeo { get; set; }

    public int CatalogId { get; set; }

    public virtual Catalog? Catalog { get; set; }
}
