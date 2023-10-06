namespace OrderDB;

public partial class OrderDetail
{
    public int Id { get; set; }

    public required int OrderId { get; set; }

    public required int NomenclatureId { get; set; }

    public required string NomenclatureGuid { get; set; }
    public required string NomenclatureName { get; set; }
    public required float NomenclaturePrace { get; set; }

    public required int NomenclatureQuantity { get; set; }

    public virtual Order? Order { get; set; }


}