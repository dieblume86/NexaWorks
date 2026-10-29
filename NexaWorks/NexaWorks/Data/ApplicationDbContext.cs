using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexaWorks.Models.Entities;

namespace NexaWorks.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Os> Os { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVersion> ProductVersions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SchoolDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasOne(t => t.ProductVersionOs)
                       .WithMany(pos => pos.Tickets)
                       .HasForeignKey(t => t.ProductVersionOsId);

                entity.Property(t => t.Issue)
                    .IsRequired();

                entity.Property(t => t.Resolution)
                    .IsRequired(false);

                entity.Property(t => t.Status)
                    .HasConversion<int>();

                entity.HasIndex(t => t.Status);
                entity.HasIndex(t => t.CreationDate);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasMany(p => p.ProductVersions)
                    .WithOne(pv => pv.Product)
                    .HasForeignKey(pv => pv.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.Name)
                    .IsRequired();
            });

            modelBuilder.Entity<ProductVersion>(entity =>
            {
                entity.HasMany(pv => pv.ProductVersionOs)
                    .WithOne(pvo => pvo.ProductVersion)
                    .HasForeignKey(pvo => pvo.ProductVersionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(pv => pv.VersionNumber)
                    .IsRequired();
            });

            modelBuilder.Entity<ProductVersionOs>(entity =>
            {
                entity.HasMany(pvo => pvo.Tickets)
                    .WithOne(t => t.ProductVersionOs)
                    .HasForeignKey(t => t.ProductVersionOsId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Os>(entity =>
            {
                entity.HasMany(os => os.ProductVersionOs)
                    .WithOne(pvo => pvo.Os)
                    .HasForeignKey(pvo => pvo.OsId);
            });
        }
    }
}
