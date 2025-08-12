using Hangfire.Dashboard;

namespace AccountService.Api.ObjectStorage.Objects;

public class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}