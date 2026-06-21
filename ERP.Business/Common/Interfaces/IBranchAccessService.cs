namespace ERP.Business.Common.Interfaces;

public interface IBranchAccessService
{
    Task<bool> CanAccessBranchAsync(
        string userId,
        int branchId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<int>> GetAccessibleBranchIdsAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<bool> HasAllBranchesAccessAsync(
        string userId,
        CancellationToken cancellationToken = default);
}
