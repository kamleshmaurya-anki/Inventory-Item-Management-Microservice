using Inventory_Item_Management_Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Item_Management_Microservice.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.ToTable("inventory_items");

            entity.HasKey(e => e.ItemId);

            entity.Property(e => e.ItemId)
                .HasColumnName("item_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.ItemName)
                .HasColumnName("item_name")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.Category)
                .HasColumnName("category")
                .HasMaxLength(100);

            entity.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            entity.HasIndex(e => e.ItemName)
                .HasDatabaseName("idx_inventory_item_name");

            entity.HasIndex(e => e.IsActive)
                .HasDatabaseName("idx_inventory_is_active");
        });
    }
}
