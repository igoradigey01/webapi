namespace OrderDB;

public partial class Order
{
    public int Id { get; set; }


    public required string OrderNo { get; set; }

    public required string OwnerId { get; set; } // владелец clientId

    public required string OwnerPhone { get; set; }

    public DateTime? CreatedAt { get; set; }  //coздан
    public DateTime? ClosedAt { get; set; }  // закрыт
    public string? OrderAdress { get; set; }
    public bool OrderPickup { get; set; } // самовывоз
    public string? OrderNote { get; set; }

    public required string CustomerFullName { get; set; }

    public string? CustomerId { get; set; }

    public string? CustomerPhone { get; set; }

    public string? CustomerMail { get; set; }


    public required float Payment_total { get; set; }
    public required float Total { get; set; }


    public int PaymentStateId { get; set; }
    public int OrderStateId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual PaymentState?  PaymentState { get; set; } 

    public virtual OrderState? OrderState { get; set; } 



}
