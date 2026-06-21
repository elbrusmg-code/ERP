using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Security;

public class UserBranchAssignment : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public BranchAccessScope AccessScope { get; set; }
    public bool IsPrimaryBranch { get; set; }
    public bool IsActive { get; set; }
}
