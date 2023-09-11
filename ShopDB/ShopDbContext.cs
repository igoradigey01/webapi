
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ShopDB;

public partial class ShopDbContext : DbContext
{

    public ShopDbContext( DbContextOptions<ShopDbContext> options ) : base(options)
    {

         Console.WriteLine("MyshopContext ---------------------- statr60.05.21");
            Database.SetCommandTimeout(300);
            //Database.EnsureDeleted();  //03.13.20
            Database.EnsureCreated();
      
    }




    public virtual required DbSet<Catalog> Catalogs { get; set; } 

      public virtual required DbSet<SubCatalog> SubCatalogs { get; set; }    

    public virtual  DbSet<Article>  Articles { get; set; }

    public virtual  DbSet<Brand> Brands { get; set; }

    
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
            

          

            entity.HasIndex(e => e.OwnerId, "fk_Catalog_Postavchik1_idx");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

             entity.Property(e => e.OwnerId)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Postavchik_id")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");    
            
          
            
            entity.Property(p => p.Hidden).HasColumnType("tinyint(1)").HasDefaultValue(1);
            
            entity.Property(e => e.DecriptSeo)
                .HasColumnName("decriptSEO")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
           
            
        });
    
         modelBuilder.Entity<SubCatalog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("SubKatalog");

            entity.HasIndex(e => e.CatalogId, "fk_KatalogN_CategoriaN1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DecriptSeo)
                .HasColumnName("decriptSEO")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.GoogleTypeId)
                .HasMaxLength(20)
                .HasColumnName("google_type_id")
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
        
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Article");

            entity.HasIndex(e => e.Product_typeId, "fk_ArticleN_TypeProduct1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Product_typeId).HasColumnName("TypeProduct_id");

            entity.Property(p => p.Hidden).HasColumnType("tinyint(1)").HasDefaultValue(1);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Brand");

            entity.HasIndex(e => e.Product_typeId, "fk_BrandN_TypeProduct1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Product_typeId).HasColumnName("TypeProduct_id");
            entity.Property(p => p.Hidden).HasColumnType("tinyint(1)").HasDefaultValue(1);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Color");

            entity.HasIndex(e => e.Product_typeId, "fk_ColorN_TypeProduct1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Product_typeId).HasColumnName("TypeProduct_id");
            entity.Property(p => p.Hidden).HasColumnType("tinyint(1)").HasDefaultValue(1);
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

            entity.HasIndex(e => e.ColorId, "fk_Product_ColorId1_idx");

            entity.HasIndex(e => e.BrandId, "fk_Product_BrandId1_idx");

            entity.HasIndex(e => e.ArticleId, "fk_Product_ArticleId1_idx");

            entity.HasIndex(e => new {e.Title,e.OwnerId}, "unique_name_postavchikId_idx").IsUnique();
            //entity.HasAlternateKey(e=>new {e.Name,e.PostavchikId} ); // is UNIQUE

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Guid).HasDefaultValueSql("UUId()");

               entity.Property(e => e.OwnerId)
                .HasMaxLength(20)
                .HasColumnName("owner_id")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            

                

            entity.Property(e => e.SubKatalogId).HasColumnName("sub_katalog_id"); 

            entity.Property(p => p.ColorId).HasColumnName("color_id");
            entity.Property(p => p.ArticleId).HasColumnName("article_id");
            entity.Property(p => p.BrandId).HasColumnName("brand_id");
                
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("title")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3"); 

           
           
            

             
           
            entity.HasOne(d => d.SubCatalog)
                   .WithMany(p => p.Product)
                   .HasForeignKey(d => d.SubKatalogId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("fk_Product_SubCatolgN1");

            entity.HasOne(d => d.Color)
                 .WithMany(p => p.Product)
                 .HasForeignKey(d => d.ColorId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("fk_Product_Color1");

            entity.HasOne(d => d.Article)
                .WithMany(p => p.Product)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Product_Article1");

            entity.HasOne(d => d.Brand)
              .WithMany(p => p.Product)
              .HasForeignKey(d => d.BrandId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("fk_Product_Brand1");
                
            
            entity.Property(p => p.Sale).HasColumnName("sale").HasColumnType("tinyint(1)").HasDefaultValue(0);
              entity.Property(p => p.InStock).HasColumnName("inStock").HasColumnType("tinyint(1)").HasDefaultValue(1);

            entity.Property(e => e.Markup)
                .HasComment("торговая наценка")
                .HasColumnName("markup");
          
           
            entity.Property(e => e.Price).HasColumnName("price");


             entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

              entity.Property(e => e.DescriptionSeo)
                .HasMaxLength(500)
                .HasColumnName("description_seo")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");   

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

       

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


     private void OnModelCatalogCreating(ModelBuilder modelBuilder)
        {
           var catalogs = new Catalog[]{
           new Catalog {Id=0,Name="Base",Hidden=true,OwnerId="x-01",DecriptSeo=""}
           };
            modelBuilder.Entity<Catalog>().HasData(catalogs);
            base.OnModelCreating(modelBuilder);
        }
       private void OnModelSubCatalogCreating(ModelBuilder modelBuilder)
        {
               var subCatalogs = new SubCatalog[]{
          new SubCatalog {Id=1,Name="Комод",Hidden=false ,DecriptSeo="комод стандарт  | комод комби | комод ЛДСП | комод МДФ",CatalogId=0,GoogleTypeId="4205"},
          new SubCatalog{Id=2,Name="Кровать",Hidden=false,DecriptSeo="Кровать 800 | Кровать 900 | Кровать 1400 | Кровать 1600 | Кровать с ящиками|Кровать ЛДСП",CatalogId=0,GoogleTypeId="505764"},
          new SubCatalog{Id=3,Name="Шкаф",Hidden=false,DecriptSeo="Шкаф ДвухДверный | Шкаф ТрехДверный | Шкаф Купе | Шкаф для одежды| Шкаф Ламинат|Шкаф с ящиками|",CatalogId=0,GoogleTypeId="6356"},
          new SubCatalog{Id=4,Name="Кухонный Уголок",DecriptSeo="  предложение от производителя",CatalogId=0,GoogleTypeId="6850"},
          new SubCatalog{Id=5,Name="Стол Обеденный",Hidden=false,DecriptSeo="Стол Обеденный в каталоге x-01 -это предложение от производителя",CatalogId=0,GoogleTypeId="4355"} ,
          new SubCatalog{Id=6,Name="Стол Писменный",Hidden=false,DecriptSeo="Стол Писменный в каталоге x-01 -это предложение от производителя",CatalogId=0,GoogleTypeId="4191"},
          new SubCatalog{Id=7,Name="Стол Журнальный",Hidden=false,DecriptSeo="Стол Журнальный в каталоге x-01 -это низкие цены от производителя",CatalogId=0,GoogleTypeId="6392"},
          new SubCatalog{Id=8,Name="Стол Маникюрный",Hidden=false,DecriptSeo="Стол Маникюрный в каталоге x-01 -это предложение от производителя",CatalogId=0,GoogleTypeId="6363"},
          new SubCatalog{Id=9,Name="Стол Тумба",Hidden=false,DecriptSeo="Стол-Тумба в каталоге x-01 -это низкие цены от производителя",CatalogId=0,GoogleTypeId="4080"},
          new SubCatalog{Id=10,Name="Кухня",Hidden=false,DecriptSeo="Кухня в каталоге x-01 - это низкие цены от производителя",CatalogId=0,GoogleTypeId="6934"},
          new SubCatalog{Id=11,Name="Комплектующие",Hidden=false,DecriptSeo="Форнитура для корпусной и мягкой мебели : петли | ручки | подлокотник ... ",CatalogId=0,GoogleTypeId="503765"}

            };
            modelBuilder.Entity<SubCatalog>().HasData(subCatalogs);
            base.OnModelCreating(modelBuilder);

        }   
}
