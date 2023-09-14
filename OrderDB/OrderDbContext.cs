using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace OrderDB;

public partial class OrderDbContext : DbContext
{

    public virtual required DbSet<Order> Orders { get; set; }

    public virtual required DbSet<PaymentState> PaymentStates { get; set; }
    public virtual required DbSet<OrderState> OrderStates { get; set; }

    public virtual required DbSet<OrderDetails> OrderDetails { get; set; }

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {

        Console.WriteLine("MyshopContext ---------------------- statr60.05.21");
        Database.SetCommandTimeout(300);
        //Database.EnsureDeleted();  //03.13.20
        Database.EnsureCreated();

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");


        modelBuilder.Entity<OrderState>(entity =>
    {
        entity.HasKey(e => e.Id).HasName("PRIMARY");

        entity.ToTable("OrderState");

        entity.Property(e => e.Id).HasColumnName("id");


        entity.Property(e => e.StateName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("name_state")
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");


        entity.Property(e => e.SmallName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("small_name")
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        entity.Property(e => e.Description)
           .IsRequired()
           .HasMaxLength(50)
           .HasColumnName("description")
           .UseCollation("utf8mb3_general_ci")
           .HasCharSet("utf8mb3");

    });


        modelBuilder.Entity<PaymentState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PaymentState");

            entity.Property(e => e.Id).HasColumnName("id");


            entity.Property(e => e.StateName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name_state")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");


            entity.Property(e => e.SmallName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("small_name")
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

            entity.Property(e => e.Description)
           .IsRequired()
           .HasMaxLength(50)
           .HasColumnName("description")
           .UseCollation("utf8mb3_general_ci")
           .HasCharSet("utf8mb3");

        });


        modelBuilder.Entity<Order>(entity =>
       {
           entity.HasKey(e => e.Id).HasName("PRIMARY");

           entity.ToTable("Order");

       }
        );

    }




}

