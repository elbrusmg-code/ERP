using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.POS;

public class CashRegister : SoftDeleteEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CashRegisterStatus Status { get; set; }
    public bool IsActive { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public ICollection<CashShift> CashShifts { get; set; } = new List<CashShift>();
    public ICollection<POSReceipt> Receipts { get; set; } = new List<POSReceipt>();
}
