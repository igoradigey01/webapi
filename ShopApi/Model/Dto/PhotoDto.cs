namespace ShopAPI.Model;

public class PhotoDto
{

    public int Id { get; set; }

    public required string Guid { get; set; }

    public int ProductId { get; set; }

}