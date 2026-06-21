namespace ERP.Core.Enums;

public enum StockMovementType
{
    PurchaseReceived = 1,
    PosSale = 2,
    SalesReturn = 3,
    SupplierReturn = 4,
    TransferIn = 5,
    TransferOut = 6,
    AdjustmentIncrease = 7,
    AdjustmentDecrease = 8,
    Damaged = 9,
    Expired = 10,
    StockTakeCorrection = 11,
    Other = 99
}
