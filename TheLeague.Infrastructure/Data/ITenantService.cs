namespace TheLeague.Infrastructure.Data;

public interface ITenantService
{
    Guid? CurrentTenantId { get; }
    void SetCurrentTenant(Guid? tenantId);
}
