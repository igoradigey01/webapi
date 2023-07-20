using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public float? Price { get; set; }

    /// <summary>
    /// торговая наценка
    /// </summary>
    public float? Markup { get; set; }

    public string Description { get; set; }

    public string Image { get; set; }

    public int KatalogId { get; set; }

    public int MaterialId { get; set; }

    public int CategoriaId { get; set; }

    public virtual CategoriaP Categoria { get; set; }

    public virtual ICollection<ImageP> ImagePs { get; set; } = new List<ImageP>();

    public virtual KatalogP Katalog { get; set; }

    public virtual MaterialP Material { get; set; }

    public virtual ICollection<ProductNomenclature> ProductNomenclatureNomenclatures { get; set; } = new List<ProductNomenclature>();

    public virtual ICollection<ProductNomenclature> ProductNomenclatureProducts { get; set; } = new List<ProductNomenclature>();
}
