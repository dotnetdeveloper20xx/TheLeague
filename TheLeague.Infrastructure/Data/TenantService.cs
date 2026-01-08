namespace TheLeague.Infrastructure.Data;

public class TenantService : ITenantService
{
    private Guid? _currentTenantId;

    public Guid? CurrentTenantId => _currentTenantId;

    public void SetCurrentTenant(Guid? tenantId)
    {
        _currentTenantId = tenantId;
    }
}
