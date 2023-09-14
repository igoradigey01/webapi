namespace OrderDB;

public partial class OrderDetails
{
    public int Id { get; set; }

    public required string OrderId {get;set;} 

     public required string  NomenclatureId{ get; set; }

    public required string  NomenclatureGuid{ get; set; }
    public required string  NomenclatureName { get; set; }
    public required string  NomenclaturePrace { get; set; }



   
    
}