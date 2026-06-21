namespace ERP.Core.Enums;

public enum PurchaseOrderStatus
{
    Draft = 1,
    PendingApproval = 2,
    Approved = 3,
    Sent = 4,
    PartiallyReceived = 5,
    FullyReceived = 6,
    Cancelled = 7,
    Closed = 8
}
