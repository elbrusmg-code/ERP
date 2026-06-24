namespace ERP.Business.Features.POS.Dtos;

public sealed class POSReceiptItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductNameSnapshot { get; set; } = string.Empty;
    public string SKUSnapshot { get; set; } = string.Empty;
    public string? BarcodeSnapshot { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal LineTotal { get; set; }
    public bool IsReturned { get; set; }
    public decimal ReturnedQuantity { get; set; }
}
