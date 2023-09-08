using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class Photo
{
    public int Id { get; set; }

    public required string Guid { get; set; }

    public int ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
