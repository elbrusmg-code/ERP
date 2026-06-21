namespace ERP.Core.Enums;

public enum PriceChangeReason
{
    InitialPrice = 1,
    CostChanged = 2,
    SalePriceChanged = 3,
    Promotion = 4,
    BranchOverride = 5,
    ManualCorrection = 6,
    Other = 99
}
