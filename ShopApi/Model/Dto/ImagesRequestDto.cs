namespace ShopAPI.Model;

public class ImageRequestDto
{
      public int Id { get; set; }
      public  string? Guid { get; set; } 

      public required IFormFile File { get; set; }
}     