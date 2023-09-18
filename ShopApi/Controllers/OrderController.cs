using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderDB;
using ShopApi.Model.Identity;
using ShopAPI.Model;
using OrderDB;
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
                                    OrderNumber = o.OrderNo,
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

             item.CustomerId=userId;
             item.CreatedAt=DateTime.Now;
             item.OrderNo=item.OwnerId+item.CreatedAt.ToString();// reset  on 


            _db.Orders.Add(item);
            await _db.SaveChangesAsync();



            var dto = new OrderDto()
            {
                Id = item.Id,
                OrderNumber = item.OrderNo,
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

            return CreatedAtRoute("GetItem", new { id = item.Id }, dto);
        }


    }
}