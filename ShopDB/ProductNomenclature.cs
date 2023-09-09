using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class ProductNomenclature
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int NomenclatureId { get; set; }

    public virtual Product Nomenclature { get; set; }=null!;

    public virtual Product Product { get; set; }=null!;
}
