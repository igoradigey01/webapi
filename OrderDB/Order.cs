namespace OrderDB;

public partial class Order
{
    public int Id { get; set; }

    public bool OrderClosed{get;set;}=false;

    public required string OrderNumber {get;set;} 
    public required string  OrderStartedDate { get; set; }  //coздан

    public  string?  OrderClosedDate { get; set; }  // закрыт

    public string? Adress{get;set;} 

   public required string OwnerId { get; set; } // владелец clientId
    
   public required string OwnerPhone { get; set; }
   
   public required string Customer {get;set;}    

    public  string? CustomerId {get;set;} 

    public required string CustomerPhone {get;set;}

    public string?   CustomerMail {get;set;}

    public string?   CustomerAdress {get;set;}
 
    public  int PaymentStateId {get;set;} 
    public required float  Payment_total {get;set;} 

    public required float  Total {get;set;}

     public virtual ICollection<OrderDetails> OrderDetail { get; set; } = new List<OrderDetails>();

      public virtual PaymentState Payment {get;set;}=null!;

   
    
}
