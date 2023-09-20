namespace ShopAPI.Model
{
    public class OrderDto
    {
        public int Id { get; set; }

        public required string OrderNo { get; set; }
        public required string OwnerId { get; set; } // владелец clientId
        public required string OwnerPhone { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; } 
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



    }
}