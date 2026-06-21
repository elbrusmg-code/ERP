using ERP.Core.Common;
using ERP.Core.Entities.HR;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Finance;

public class SalaryPayment : SoftDeleteEntity
{
    public string PaymentNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public SalaryPaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal DeductionAmount { get; set; }
    public decimal NetAmount { get; set; }
    public DateTime SalaryMonth { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
    public int? FinancialTransactionId { get; set; }
    public FinancialTransaction? FinancialTransaction { get; set; }
}
