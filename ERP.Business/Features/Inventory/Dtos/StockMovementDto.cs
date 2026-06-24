namespace ERP.Business.Features.Inventory.Dtos;

public class StockMovementDto
{
    public int Id { get; set; }
    public string MovementNumber { get; set; } = string.Empty;
    public string MovementType { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int SourceWarehouseId { get; set; }
    public string SourceWarehouseName { get; set; } = string.Empty;
    public int? DestinationWarehouseId { get; set; }
    public string? DestinationWarehouseName { get; set; }
    public int? ProductBatchId { get; set; }
    public string? BatchNumber { get; set; }
    public decimal Quantity { get; set; }
    public DateTime CreatedDate { get; set; }
}
