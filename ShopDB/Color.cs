﻿using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class Color
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public bool Hidden { get; set; }

    public int TypeProductId { get; set; }
     public required string PostavchikId {get;set;}
}
