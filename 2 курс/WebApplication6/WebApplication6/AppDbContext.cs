using Microsoft.EntityFrameworkCore;


    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Purchase> Purchases => Set<Purchase>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Clients
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients", "store");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.Birthday).HasColumnName("birthday");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
                entity.Property(e => e.Address).HasColumnName("address");
            });

            // Products
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products", "store");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            // Purchases
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("purchases", "store");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ClientId).HasColumnName("client_id");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.PurchaseDate).HasColumnName("purchase_date");

                entity.HasOne(p => p.Client)
                      .WithMany()
                      .HasForeignKey(p => p.ClientId)
                      .HasPrincipalKey(c => c.Id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Product)
                      .WithMany()
                      .HasForeignKey(p => p.ProductId)
                      .HasPrincipalKey(pr => pr.Id)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
