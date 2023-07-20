using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class Katalog
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool FlagLink { get; set; }

    public bool FlagHref { get; set; }

    public string Link { get; set; }

    public bool Hidden { get; set; }

    public string DecriptSeo { get; set; }

    public virtual ICollection<SubKatalog> SubKatalogs { get; set; } = new List<SubKatalog>();
}
