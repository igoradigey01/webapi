using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class ImageP
{
    public int Id { get; set; }

    public string Guid { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; }
}
