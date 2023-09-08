using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ShopDB;

public partial class ShopDbContext : DbContext
{

    public ShopDbContext(
        DbContextOptions<ShopDbContext> options       
       
)
        : base(options)
    {
      
    }


    public virtual required DbSet<Postavchik> Postavchiks { get; set; }

    public virtual required DbSet<Catalog> Catalogs { get; set; } 

      public virtual required DbSet<SubKatalog> SubKatalogs { get; set; }    

    public virtual  DbSet<Article>?  Articles { get; set; }

    public virtual  DbSet<Brand>? Brands { get; set; }

    
    public virtual required DbSet<Color> Colors { get; set; }


    public virtual required DbSet<Product> Products { get; set; }

    public virtual required DbSet<ProductNomenclature> ProductNomenclatures { get; set; }


     public virtual required DbSet<Photo> Photos { get; set; }

  

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    //     => optionsBuilder.UseMySql(_configuration., Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");




         modelBuilder.Entity<Catalog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Catalog");

            entity.Property(e => e.Id).HasColumnName("id");
            

            entity.HasIndex(e => e.TypeProductId, "fk_Catalog_TypeProduct1_idx");

            entity.HasIndex(e => e.PostavchikId, "fk_Catalog_Postavchik1_idx");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

             entity.Property(e => e.PostavchikId)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Postavchik_id")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");    
            
            entity.Property(e => e.TypeProductId).HasColumnName("TypeProduct_id");
            
            entity.Property(p => p.Hidden).HasColumnType("tinyint(1)");
            
            entity.Property(e => e.DecriptSeo)
                .HasColumnName("decriptSEO")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
           
            
        });
    

        
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Article");

            entity.HasIndex(e => e.TypeProductId, "fk_ArticleN_TypeProduct1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TypeProductId).HasColumnName("TypeProduct_id");

            entity.Property(p => p.Hidden).HasColumnType("tinyint(1)");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Brand");

            entity.HasIndex(e => e.TypeProductId, "fk_BrandN_TypeProduct1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TypeProductId).HasColumnName("TypeProduct_id");
            entity.Property(p => p.Hidden).HasColumnType("tinyint(1)");
        });

       

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Color");

            entity.HasIndex(e => e.TypeProductId, "fk_ColorN_TypeProduct1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TypeProductId).HasColumnName("TypeProduct_id");
            entity.Property(p => p.Hidden).HasColumnType("tinyint(1)");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Photo");

            entity.HasIndex(e => e.ProductId, "fk_Photo_Product1_idx");

            entity.HasIndex(e => e.Guid, "name_UNIQUE5").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Guid)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("guid")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ImagePs)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ImageP_Product1");
        });

       
      
       

       

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.HasIndex(e => e.KatalogId, "fk_Nomenclature_Katalog1_idx");

            entity.HasIndex(e => e.CategoriaId, "fk_Product_Categoria2");

            entity.HasIndex(e => e.MaterialId, "fk_Product_Material2");

            entity.HasIndex(e => e.Name, "name_UNIQUE4").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .HasColumnName("image")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.KatalogId).HasColumnName("Katalog_id");
            entity.Property(e => e.Markup)
                .HasComment("торговая наценка")
                .HasColumnName("markup");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Product_Categoria2");

            entity.HasOne(d => d.Katalog).WithMany(p => p.Products)
                .HasForeignKey(d => d.KatalogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Nomenclature_Katalog1");

            entity.HasOne(d => d.Material).WithMany(p => p.Products)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Product_Material2");
        });

        modelBuilder.Entity<ProductNomenclature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProductNomenclature");

            entity.HasIndex(e => e.NomenclatureId, "fk_ProductNomenclature_Nomenclature1_idx");

            entity.HasIndex(e => e.ProductId, "fk_ProductNomenclature_Product1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NomenclatureId).HasColumnName("Nomenclature_id");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Nomenclature).WithMany(p => p.ProductNomenclature_Nomenclatures)
                .HasForeignKey(d => d.NomenclatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ProductNomenclature_Nomenclature1");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductNomenclature_Products)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ProductNomenclature_Product1");
        });

        modelBuilder.Entity<SubKatalog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("SubKatalog");

            entity.HasIndex(e => e.CatalogId, "fk_KatalogN_CategoriaN1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DecriptSeo)
                .HasColumnName("decriptSEO")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CatalogId).HasColumnName("katalog_id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.Catalog).WithMany(p => p.SubKatalogs)
                .HasForeignKey(d => d.CatalogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_KatalogN_CategoriaN1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
