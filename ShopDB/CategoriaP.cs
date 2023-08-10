﻿using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class CategoriaP
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool Hidden { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
