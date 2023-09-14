namespace OrderDB;

public partial class PaymentState
{
     public int Id { get; set; }

    public required string StateName{get;set;}

    public required string SmallName{get;set;}

    public required string Description {get;set;}
}


/*
не подтвержден STARTED
подтвержден (Задаток) balance_due Payment_total меньше общей суммы
failed  платеж находится в состоянии сбоя 
paid оплачен оплачено - pay_total равен итогу

void - заказ отменен и pay_total равен нулю

*/