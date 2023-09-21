using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OrderDB;
using ShopAPI.Model;
using ShopApi.Model.Identity;
using System.Data;
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
        [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
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
            var products = item.OrderDetails;
            if (products.Count > 0)
            {
                int id_last = GetLastIdOrderDetails();
                foreach (var p in products)
                {
                    p.Id = ++id_last;
                }
            }
            item.CustomerId = userId;
            item.CreatedAt = DateTime.Now;
            item.ClosedAt = DateTime.MinValue;
            //  "closedAt": "0001-01-01T00:00:00",


            GetOrdrNO(item.OwnerId, out int count);
            item.OrderNo = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + "-" + count.ToString();
            Console.WriteLine("Order_NO : " + item.OrderNo);



            //Console.WriteLine("id_last : "+id_last);

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

        private void GetOrdrNO(string owner_id, out int no_order)
        {
            // if use using on con -- con.close() end error for ef

            var con = _db.Database.GetDbConnection();
            var cmd = con.CreateCommand();
            cmd.CommandText = "select  OrderDB.new_order_no('x-01');";
            if (cmd.Connection!.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            no_order = (int)cmd.ExecuteScalar()!;
            // cmd.Connection.Close();

        }

        private int GetLastIdOrderDetails()
        {

            int id_last;


            var con = _db.Database.GetDbConnection();
            var cmd = con.CreateCommand();
            cmd.CommandText = "select OrderDB.last_id_order_detail();";

            if (cmd.Connection!.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            id_last = (int)cmd.ExecuteScalar()!;
            // cmd.Connection.Close();
            return id_last;
        }


    }
}