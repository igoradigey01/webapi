using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OrderDB;
using ShopAPI.Model;
using ShopApi.Model.Identity;
using System.Data;
using System.Security.Claims;
using Microsoft.Data.SqlClient;



namespace ShopAPI.Controllers
{


    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
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


        [HttpGet("{owner_id}")]
        [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll(string owner_id, DateTime start)
        {

             var time_min=DateTime.Parse("1753-01-01T00:00:00") ;  // for mssql  min datetime
            if (  start< time_min)
            {
                            start=time_min;
                        }

            // int i = 0;
            var orders = await (from item in _db.Orders!
                                where item.OwnerId == owner_id && item.CreatedAt >= start
                                select new OrderDto()
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



                                }).ToListAsync();
            if (orders == null) return NotFound();

            return Ok(orders);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllForCustomer()
        {
            // int i = 0;
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await (from item in _db.Orders!
                                where item.CustomerId == userId
                                select new OrderDto()
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



                                }).ToListAsync();
            if (orders == null) return NotFound();

            return Ok(orders);
        }



        [HttpGet("{no_order}")]
        [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetForOrderNO(string no_order)
        {
            // int i = 0;
            //  string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await (from item in _db.Orders!
                                where item.OrderNo == no_order
                                select new OrderDto()
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



                                }).ToListAsync();
            if (orders == null) return NotFound();

            return Ok(orders);
        }

        [HttpGet("{no_order}")]
        [Authorize(Roles = X01Roles.Admin + "," + X01Roles.Manager)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetForCustomerPhone(string phone)
        {
            // int i = 0;
            // string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await (from item in _db.Orders!
                                where item.CustomerPhone == phone
                                select new OrderDto()
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



                                }).ToListAsync();
            if (orders == null) return NotFound();

            return Ok(orders);
        }


        [HttpGet("{id}", Name = nameof(GetItem))]
        public async Task<ActionResult<OrderDto>> GetItem(int id)
        {
            
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
        public async Task<ActionResult<OrderDto>> CreateOrder(Order item)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var cloneItemOrder = item.OrderDetails.ToArray().Clone();
            // item.OrderDetails.Clear();
            /* var products = item.OrderDetails;
            if (products.Count > 0)
            {
                int id_last = GetLastIdOrderDetails();
                foreach (var p in products)
                {
                    p.Id = ++id_last;
                }
            } */
            item.CustomerId = userId;
            item.CreatedAt = DateTime.Now;
            item.ClosedAt =     DateTime.Parse("1753-01-01T00:00:00");      // DateTime.MinValue;  for MsSql  min var  1753-1-1
            //  "closedAt": "0001-01-01T00:00:00",


            GetOrdrNO(item.OwnerId, out int count);
            item.OrderNo = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + "-" + count.ToString();
            Console.WriteLine("Order_NO : " + item.OrderNo);
        

               item.PaymentStateId=1; //Платеж не принят  pay_total равен нулю
               item.OrderStateId=1;//Ордер проверен на корректность, но еще не принят брокером

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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, Order item)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            _db.Entry(item).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); //204         
        }



        private void   GetOrdrNO(string owner_id, out int no_order)
        {
           
       string sqlExpression = "SELECT      MAX(no_order)   FROM  [dbo].[Sequence]  where    [dbo].[Sequence].[owner_id] = @owner_id  " ;
       var con = _db.Database.GetDbConnection();
            var cmd = con.CreateCommand();
                SqlParameter name_owner_id = new()
             {
                    ParameterName = "@owner_id",
                    Value =  owner_id
                };
                // добавляем параметр
                cmd.Parameters.Add(name_owner_id);
            cmd.CommandText = sqlExpression;
         /*     cmd.CommandType = CommandType.StoredProcedure;
             SqlParameter name_owner_id = new()
             {
                    ParameterName = "@owner_id",
                    Value =  owner_id
                };
                // добавляем параметр
                cmd.Parameters.Add(name_owner_id); */
        con.Open();
            var no_last_order = (int?) cmd.ExecuteScalar() ;
     
            var  no_new_order=0;
          if(no_last_order != null){ no_new_order = (int)(no_last_order + 1); }
          
         var item=new Sequence{NoOrder=no_new_order,OwnerId=owner_id};
         _db.Sequences.Add(item);
          _db.SaveChanges(); /// ???? error ???
           no_order = no_new_order;
            // cmd.Connection.Close();

        }

        private int GetLastIdOrderDetails()
        {

            int id_last;


            var con = _db.Database.GetDbConnection();
            var cmd = con.CreateCommand();
            cmd.CommandText =  "  EXEC  [dbo]. last_id_order_detail ;";                                      // "select OrderDB.last_id_order_detail();";  for  mysql

            if (cmd.Connection!.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            var i = cmd.ExecuteScalar() ;
            if(i==null) {
                return id_last=1;
            }
            else
            {
                return id_last=(int)i;
            }
        }


        private bool OrderExists(int id)
        {
            return _db.Orders.Count(e => e.Id == id) > 0;

        }

    }
}