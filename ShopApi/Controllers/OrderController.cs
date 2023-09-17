using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderDB;
using ShopApi.Model.Identity;
using ShopAPI.Model;
using OrderDB;



namespace ShopAPI.Controllers
{


    [ApiController]
    [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _db;

        public OrderController(OrderDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {



            var orders = await (from o in _db.Orders
                                  select new OrderDto()
                                  {
                                      Id = o.Id,
                                      OrderNumber=o.OrderNumber,
                                      OwnerId=o.OwnerId,
                                      OwnerPhone=o.OwnerPhone,                                      
                                      CreatedAt = o.CreatedAt,
                                      ClosedAt=o.ClosedAt,
                                      OrderAdress=o.OrderAdress,
                                      OrderPickup=o.OrderPickup,
                                      OrderNote=o.OrderNote,
                                      CustomerFullName=o.CustomerFullName,
                                      CustomerId=o.CustomerId,
                                      CustomerPhone=o.CustomerPhone,
                                      CustomerMail=o.CustomerMail,

                                      Payment_total=o.Payment_total,
                                      Total=o.Total,

                                      PaymentStateId=o.PaymentStateId,
                                      OrderStateId=o.OrderStateId
                                     

                                     
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
                                      StateName=p.StateName,
                                      SmallName=p.SmallName,
                                      Description=p.Description

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
                                      StateName=p.StateName,
                                      SmallName=p.SmallName,
                                      Description=p.Description
                                          }).ToListAsync();

            if (orderStates == null) return NotFound();

            return Ok(orderStates);
        }


    }
}