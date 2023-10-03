namespace ShopAPI.Model;

public class ProductRequestDto
{
      public int Id { get; set; }
      public  string? Guid { get; set; }
      public bool Hidden { get; set; }
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

      public required IFormFile File { get; set; }



}