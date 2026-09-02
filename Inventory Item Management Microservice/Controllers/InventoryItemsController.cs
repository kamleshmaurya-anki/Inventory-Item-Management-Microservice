using Inventory_Item_Management_Microservice.DTOs;
using Inventory_Item_Management_Microservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Item_Management_Microservice.Controllers;

[ApiController]
[Route("api/inventory-items")]
public class InventoryItemsController : ControllerBase
{
    private readonly IInventoryItemService _inventoryItemService;

    public InventoryItemsController(IInventoryItemService inventoryItemService)
    {
        _inventoryItemService = inventoryItemService;
    }

    /// <summary>
    /// Creates a new inventory item.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InventoryItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InventoryItemResponse>> Create(
        [FromBody] CreateInventoryItemRequest request,
        CancellationToken cancellationToken)
    {
        var item = await _inventoryItemService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, item);
    }

   
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InventoryItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InventoryItemResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var item = await _inventoryItemService.GetByIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>
    /// Lists inventory items with optional pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<InventoryItemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<InventoryItemResponse>>> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _inventoryItemService.ListAsync(page, pageSize, includeInactive, cancellationToken);
        return Ok(result);
    }

    /// <summary>
 
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(InventoryItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InventoryItemResponse>> Update(
        Guid id,
        [FromBody] UpdateInventoryItemRequest request,
        CancellationToken cancellationToken)
    {
        var item = await _inventoryItemService.UpdateAsync(id, request, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>
   
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SoftDelete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _inventoryItemService.SoftDeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
