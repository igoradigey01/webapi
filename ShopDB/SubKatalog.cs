using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class SubKatalog
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Hidden { get; set; }

    public string DecriptSeo { get; set; }

    public int KatalogId { get; set; }

    public virtual Katalog Katalog { get; set; }
}
