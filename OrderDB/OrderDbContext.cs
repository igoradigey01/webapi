using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace OrderDB ;

   public partial class OrderDbContext : DbContext
{

    public virtual required DbSet<Order> Orders { get; set; }

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {

        Console.WriteLine("MyshopContext ---------------------- statr60.05.21");
        Database.SetCommandTimeout(300);
        //Database.EnsureDeleted();  //03.13.20
        Database.EnsureCreated();

    }

    


}

