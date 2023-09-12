using System;
using System.Collections.Generic;

namespace ShopDB;

public partial class Product
{
    public int Id { get; set; }
    public required string Guid { get; set; } // set in DB - UUID()
   
    public required string OwnerId { get; set; }

    public required int Product_typeId { get; set; }
    public required string Title { get; set; }
     
    public int SubKatalogId { get; set; }
    public int ColorId { get; set; }
    public int BrandId { get; set; }
    public int ArticleId { get; set; }


    public int Position { get; set; } // for by Sort in list render

    public bool InStock { get; set; } // в наличие на складе
    public bool Sale { get; set; } // распродажа

    public float Price { get; set; }

    public float Markup { get; set; } /// торговая наценка
    public string? Description { get; set; }

    public string? DescriptionSeo { get; set; }

    public virtual SubCatalog SubCatalog { get; set; }= null!;
     public virtual Product_type Product_type { get; set; } = null!;
    public virtual Color Color { get; set; } = null!;
    public virtual Brand Brand { get; set; } = null!;
    public virtual Article Article { get; set; } = null!;

    public virtual ICollection<Photo> ImagePs { get; set; } = new List<Photo>();

    public virtual ICollection<ProductNomenclature> ProductNomenclature_Nomenclatures { get; set; } = new List<ProductNomenclature>();

    public virtual ICollection<ProductNomenclature> ProductNomenclature_Products { get; set; } = new List<ProductNomenclature>();
}
