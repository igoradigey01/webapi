namespace ShopAPI.Model;

public class PhotoRequestDto
{     
      public int Id {get;set;}
      public int IdProduct { get; set; }
      public  string? Guid { get; set; } 

      public required IFormFile File { get; set; }
}     