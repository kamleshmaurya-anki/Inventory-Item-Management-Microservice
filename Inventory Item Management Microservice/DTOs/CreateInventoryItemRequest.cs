using System.ComponentModel.DataAnnotations;

namespace Inventory_Item_Management_Microservice.DTOs;

public class CreateInventoryItemRequest
{
    [Required]
    [MaxLength(150)]
    public string ItemName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Category { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public bool IsActive { get; set; } = true;
}
