namespace ERP.Business.Features.POS.Dtos;

public sealed class SalesReturnItemDto
{
    public int Id { get; set; }
    public int POSReceiptItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ReturnAmount { get; set; }
    public bool RestockToInventory { get; set; }
    public bool MarkAsDamaged { get; set; }
    public string? Reason { get; set; }
}
