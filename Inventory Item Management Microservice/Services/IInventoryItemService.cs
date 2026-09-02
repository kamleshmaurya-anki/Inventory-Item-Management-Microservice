using Inventory_Item_Management_Microservice.DTOs;

namespace Inventory_Item_Management_Microservice.Services;

public interface IInventoryItemService
{
    Task<InventoryItemResponse> CreateAsync(CreateInventoryItemRequest request, CancellationToken cancellationToken = default);

    Task<InventoryItemResponse?> GetByIdAsync(Guid itemId, CancellationToken cancellationToken = default);

    Task<PagedResult<InventoryItemResponse>> ListAsync(
        int page,
        int pageSize,
        bool includeInactive,
        CancellationToken cancellationToken = default);

    Task<InventoryItemResponse?> UpdateAsync(
        Guid itemId,
        UpdateInventoryItemRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> SoftDeleteAsync(Guid itemId, CancellationToken cancellationToken = default);
}
