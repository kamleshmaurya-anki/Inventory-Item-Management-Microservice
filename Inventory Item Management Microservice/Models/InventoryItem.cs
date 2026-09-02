namespace Inventory_Item_Management_Microservice.Models;

public class InventoryItem
{
    public Guid ItemId { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public string? Category { get; set; }

    public int Quantity { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
