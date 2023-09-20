using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderDB;
using ShopApi.Model.Identity;
using ShopAPI.Model;

using System.Security.Claims;



namespace ShopAPI.Controllers
{


    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _db;

        public OrderController(OrderDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        //[Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {



            var orders = await (from o in _db.Orders
                                select new OrderDto()
                                {
                                    Id = o.Id,
                                    OrderNo = o.OrderNo,
                                    OwnerId = o.OwnerId,
                                    OwnerPhone = o.OwnerPhone,
                                    CreatedAt = o.CreatedAt,
                                    ClosedAt = o.ClosedAt,
                                    OrderAdress = o.OrderAdress,
                                    OrderPickup = o.OrderPickup,
                                    OrderNote = o.OrderNote,
                                    CustomerFullName = o.CustomerFullName,
                                    CustomerId = o.CustomerId,
                                    CustomerPhone = o.CustomerPhone,
                                    CustomerMail = o.CustomerMail,

                                    Payment_total = o.Payment_total,
                                    Total = o.Total,

                                    PaymentStateId = o.PaymentStateId,
                                    OrderStateId = o.OrderStateId



                                }).ToListAsync();

            if (orders == null) return NotFound();

            return Ok(orders);


        }

        [HttpGet("{id}", Name = nameof(GetItem))]
        [AllowAnonymous]
        public async Task<ActionResult<OrderDto>> GetItem(int id)
        {


            int v = id;
            var item = await _db.Orders.Select(d => new OrderDto
            {
                Id = d.Id,
                OrderNo = d.OrderNo,
                OwnerId = d.OwnerId,
                OwnerPhone = d.OwnerPhone,
                CreatedAt = d.CreatedAt,
                ClosedAt = d.ClosedAt,
                OrderAdress = d.OrderAdress,
                OrderPickup = d.OrderPickup,
                OrderNote = d.OrderNote,
                CustomerFullName = d.CustomerFullName,
                CustomerId = d.CustomerId,
                CustomerPhone = d.CustomerPhone,
                CustomerMail = d.CustomerMail,

                Payment_total = d.Payment_total,
                Total = d.Total,

                PaymentStateId = d.PaymentStateId,
                OrderStateId = d.OrderStateId
            }
            )
            .SingleOrDefaultAsync(c => c.Id == id);

            if (item == null) return NotFound();

            return Ok(item);



        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PaymentStateDto>>> GetPaymentStateAll()
        {

            var paymentStates = await (from p in _db.PaymentStates
                                       select new PaymentStateDto()
                                       {
                                           Id = p.Id,
                                           StateName = p.StateName,
                                           SmallName = p.SmallName,
                                           Description = p.Description

                                       }).ToListAsync();

            if (paymentStates == null) return NotFound();

            return Ok(paymentStates);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PaymentStateDto>>> GetOrderStateAll()
        {

            var orderStates = await (from p in _db.OrderStates
                                     select new OrderStateDto()
                                     {
                                         Id = p.Id,
                                         StateName = p.StateName,
                                         SmallName = p.SmallName,
                                         Description = p.Description
                                     }).ToListAsync();

            if (orderStates == null) return NotFound();

            return Ok(orderStates);
        }




        [HttpPost]
        public async Task<ActionResult<CatalogDto>> CreateOrder(Order item)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cloneItemOrder = item.OrderDetails.ToArray().Clone();
            // item.OrderDetails.Clear();
            item.OrderDetails = new List<OrderDetail>();
            item.CustomerId = userId;
            item.CreatedAt = DateTime.Now;
            item.ClosedAt = DateTime.MinValue;
            //  "closedAt": "0001-01-01T00:00:00",
            

            /*  Script  SqlProcedure  : CREATE DEFINER=`root`@`%` PROCEDURE `next_order_no`(
in  owner_id  VARCHAR(50),
out order_no int
)
BEGIN
declare  order_new int;
SELECT     `Sequence`.`no_order`   
FROM `OrderDB`.`Sequence`
 where    `Sequence`.`owner_id`=owner_id
  ORDER BY id DESC LIMIT 1
  INTO order_no  ;
  set  order_new = order_no+1;
  
  INSERT INTO `OrderDB`.`Sequence`
(`id`,
`owner_id`,
`no_order`)
VALUES
(0,
owner_id,
order_new);


END
            */

            // item.OrderNo=item.OwnerId+item.CreatedAt.ToString();// reset  on 

            //  SqliteParameter param = new SqliteParameter("@name", "%Tom%");

            // var no_owner = _db.Sequences.FromSqlRaw("SELECT     `Sequence`.`no_order`" +
            //                                         "FROM `OrderDB`.`Sequence`" +
            //                                       "where    `Sequence`.`owner_id`='x-01'" +
            //                                        " ORDER BY id DESC LIMIT 1;");
            //Console.WriteLine("no_owner--" + no_owner[0]);

            _db.Orders.Add(item);
            await _db.SaveChangesAsync();





            var dto = new OrderDto()
            {
                Id = item.Id,
                OrderNo = item.OrderNo,
                OwnerId = item.OwnerId,
                OwnerPhone = item.OwnerPhone,
                CreatedAt = item.CreatedAt,
                ClosedAt = item.ClosedAt,
                OrderAdress = item.OrderAdress,
                OrderPickup = item.OrderPickup,
                OrderNote = item.OrderNote,
                CustomerFullName = item.CustomerFullName,
                CustomerId = item.CustomerId,
                CustomerPhone = item.CustomerPhone,
                CustomerMail = item.CustomerMail,

                Payment_total = item.Payment_total,
                Total = item.Total,

                PaymentStateId = item.PaymentStateId,
                OrderStateId = item.OrderStateId

            };

            return CreatedAtRoute(nameof(GetItem), new { id = item.Id }, dto);
        }


    }
}