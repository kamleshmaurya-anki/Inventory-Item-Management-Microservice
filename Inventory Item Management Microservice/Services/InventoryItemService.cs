using Inventory_Item_Management_Microservice.Data;
using Inventory_Item_Management_Microservice.DTOs;
using Inventory_Item_Management_Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Item_Management_Microservice.Services;

public class InventoryItemService : IInventoryItemService
{
    private readonly InventoryDbContext _dbContext;

    public InventoryItemService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InventoryItemResponse> CreateAsync(
        CreateInventoryItemRequest request,
        CancellationToken cancellationToken = default)
    {
        var item = new InventoryItem
        {
            ItemName = request.ItemName,
            Category = request.Category,
            Quantity = request.Quantity,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.InventoryItems.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(item);
    }

    public async Task<InventoryItemResponse?> GetByIdAsync(
        Guid itemId,
        CancellationToken cancellationToken = default)
    {
        var item = await _dbContext.InventoryItems
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.ItemId == itemId, cancellationToken);

        return item is null ? null : MapToResponse(item);
    }

    public async Task<PagedResult<InventoryItemResponse>> ListAsync(
        int page,
        int pageSize,
        bool includeInactive,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : Math.Min(pageSize, 100);

        var query = _dbContext.InventoryItems.AsNoTracking();

        if (!includeInactive)
        {
            query = query.Where(i => i.IsActive);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<InventoryItemResponse>
        {
            Items = items.Select(MapToResponse).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<InventoryItemResponse?> UpdateAsync(
        Guid itemId,
        UpdateInventoryItemRequest request,
        CancellationToken cancellationToken = default)
    {
        var item = await _dbContext.InventoryItems
            .FirstOrDefaultAsync(i => i.ItemId == itemId, cancellationToken);

        if (item is null)
        {
            return null;
        }

        item.ItemName = request.ItemName;
        item.Category = request.Category;
        item.Quantity = request.Quantity;
        item.IsActive = request.IsActive;
        item.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(item);
    }

    public async Task<bool> SoftDeleteAsync(Guid itemId, CancellationToken cancellationToken = default)
    {
        var item = await _dbContext.InventoryItems
            .FirstOrDefaultAsync(i => i.ItemId == itemId && i.IsActive, cancellationToken);

        if (item is null)
        {
            return false;
        }

        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static InventoryItemResponse MapToResponse(InventoryItem item) =>
        new()
        {
            ItemId = item.ItemId,
            ItemName = item.ItemName,
            Category = item.Category,
            Quantity = item.Quantity,
            IsActive = item.IsActive,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
}
