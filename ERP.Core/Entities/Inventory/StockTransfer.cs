using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Inventory;

public class StockTransfer : SoftDeleteEntity
{
    public string TransferNumber { get; set; } = string.Empty;
    public int FromBranchId { get; set; }
    public Branch? FromBranch { get; set; }
    public int FromWarehouseId { get; set; }
    public Warehouse? FromWarehouse { get; set; }
    public int ToBranchId { get; set; }
    public Branch? ToBranch { get; set; }
    public int ToWarehouseId { get; set; }
    public Warehouse? ToWarehouse { get; set; }
    public StockTransferStatus Status { get; set; }
    public DateTime TransferDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public string? Notes { get; set; }
    public ICollection<StockTransferItem> Items { get; set; } = new List<StockTransferItem>();
}
