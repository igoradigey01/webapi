using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class KatalogP
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Hidden { get; set; }

    public bool FlagLink { get; set; }

    public bool FlagHref { get; set; }

    public string Link { get; set; }

    public string DecriptSeo { get; set; }

    public string KeywordsSeo { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
