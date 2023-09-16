using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace OrderDB;

public partial class OrderDbContext : DbContext
{

    public virtual DbSet<Order> Orders { get; set; } = null!;

    public virtual DbSet<PaymentState> PaymentStates { get; set; } = null!;

    public virtual DbSet<OrderState> OrderStates { get; set; } = null!;

    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;

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



           entity.HasIndex(e => e.PaymentStateId, "fk_Order_PaymentState1_idx");

           entity.HasIndex(e => e.OrderStateId, "fk_Order_OrderState1_idx");



           entity.HasIndex(e => new { e.OrderNumber, e.OwnerId }, "unique_OrderNumber_OwnerId_idx").IsUnique();
           //entity.HasAlternateKey(e=>new {e.Name,e.PostavchikId} ); // is UNIQUE

           entity.Property(e => e.Id).HasColumnName("id");


           entity.Property(e => e.OrderNumber)
           .IsRequired()
           .HasMaxLength(20)
           .HasColumnName("order_number")
           .UseCollation("utf8mb3_general_ci")
           .HasCharSet("utf8mb3");

           entity.Property(e => e.OwnerId)
           .IsRequired()
           .HasMaxLength(20)
           .HasColumnName("owner_id")
           .UseCollation("utf8mb3_general_ci")
           .HasCharSet("utf8mb3");

           entity.Property(e => e.OwnerPhone)
           .IsRequired()
           .HasMaxLength(20)
           .HasColumnName("name_state")
           .UseCollation("utf8mb3_general_ci")
           .HasCharSet("utf8mb3");



           entity.Property(u => u.CreatedAt)
           .HasColumnName("create_at")
           .HasColumnType("datetime")
           .HasDefaultValueSql("'current_timestamp()'");


           entity.Property(u => u.ClosedAt)
           .HasColumnName("closed_at")
           .HasColumnType("datetime");


           entity.Property(e => e.OrderAdress)
          .IsRequired()
          .HasMaxLength(200)
          .HasColumnName("order_adress")
          .UseCollation("utf8mb3_general_ci")
          .HasCharSet("utf8mb3");


           entity.Property(p => p.OrderPickup)
           .HasColumnType("tinyint(1)")
           .HasColumnName("order_pickup")
           .HasDefaultValue(0);

           entity.Property(e => e.OrderNote)
          .IsRequired()
          .HasMaxLength(500)
          .HasColumnName("order_note")
          .UseCollation("utf8mb3_general_ci")
          .HasCharSet("utf8mb3");


           entity.Property(e => e.CustomerFullName)
          .IsRequired()
          .HasMaxLength(100)
          .HasColumnName("customer_full_name")
          .UseCollation("utf8mb3_general_ci")
          .HasCharSet("utf8mb3");

           entity.Property(e => e.CustomerId)
           .IsRequired()
           .HasMaxLength(50)
           .HasColumnName("customer_id")
           .UseCollation("utf8mb3_general_ci")
           .HasCharSet("utf8mb3");

           entity.Property(e => e.CustomerPhone)
           .IsRequired()
           .HasMaxLength(50)
           .HasColumnName("customer_phone")
           .UseCollation("utf8mb3_general_ci")
           .HasCharSet("utf8mb3");

           entity.Property(e => e.CustomerMail)
           .IsRequired()
           .HasMaxLength(50)
           .HasColumnName("customer_mail")
           .UseCollation("utf8mb3_general_ci")
           .HasCharSet("utf8mb3");

           entity.Property(e => e.Payment_total).HasColumnName("payment_total");

           entity.Property(e => e.Total).HasColumnName("total");

           entity.Property(e => e.PaymentStateId).HasColumnName("payment_state_id");


           entity.Property(e => e.OrderStateId).HasColumnName("order_state_id");

           entity.HasOne(d => d.PaymentState)
              .WithMany(p => p.Orders)
              .HasForeignKey(d => d.OrderStateId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("fk_Order_PaymentState1");

           entity.HasOne(d => d.OrderState)
          .WithMany(p => p.Orders)
          .HasForeignKey(d => d.OrderStateId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("fk_Order_OrderState1");

       }
        );


        modelBuilder.Entity<OrderDetail>(entity =>
     {


         entity.HasKey(e => e.Id).HasName("PRIMARY");

         entity.ToTable("OrderDetail");
         entity.Property(e => e.Id).HasColumnName("id");
         entity.Property(e => e.OrderId).HasColumnName("order_id");
         entity.Property(e => e.NomenclatureId).HasColumnName("nomenclature_id");
         entity.Property(e => e.NomenclatureName)
                     .IsRequired()
                      .HasMaxLength(200)
                      .HasColumnName("nomenclature_name")
                      .UseCollation("utf8mb3_general_ci")
                      .HasCharSet("utf8mb3");


         entity.Property(e => e.NomenclatureGuid)
                 .IsRequired()
                .HasMaxLength(36)
                .HasColumnName("title")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");




         entity.Property(e => e.NomenclaturePrace).HasColumnName("nomenclature_prace");
         entity.Property(e => e.NomenclatureQuantity).HasColumnName("nomenclature_quantity");


           entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OrderDetail_Order");


     });



    }




}

