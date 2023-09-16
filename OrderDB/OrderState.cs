namespace OrderDB;

public partial class OrderState
{
    public int Id { get; set; }

    public required string StateName{get;set;}

    public required string SmallName{get;set;}

    public required string Description {get;set;}

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

}

/*
 ORDER_STATE_STARTED

Ордер проверен на корректность, но еще не принят брокером

ORDER_STATE_PLACED

Ордер принят

ORDER_STATE_CANCELED

Ордер снят клиентом

ORDER_STATE_PARTIAL

Ордер выполнен частично

ORDER_STATE_FILLED

Ордер выполнен полностью

ORDER_STATE_REJECTED

Ордер отклонен

ORDER_STATE_EXPIRED

Ордер снят по истечении срока его действия

ORDER_STATE_REQUEST_ADD

Ордер в состоянии регистрации (выставление в торговую систему)

ORDER_STATE_REQUEST_MODIFY

Ордер в состоянии модификации (изменение его параметров)

ORDER_STATE_REQUEST_CANCEL

Ордер в состоянии удаления (удаление из торговой системы)

Order Closed 
*/