using Hangfire;
using HangfireBasicAuthenticationFilter;

namespace EnhancedPensionSystem_WebAPP.Extensions;

public static class HangfireAuthorizationFilter
{
    public static DashboardOptions GetHangfireDashboardOptions(this IConfiguration configuration)
    {
        return new DashboardOptions
        {
            DashboardTitle = "EPS Reporting and Monitoring Jobs Dashboard",
            Authorization = new[]
            {
             new HangfireCustomBasicAuthenticationFilter()
             {
                 Pass = configuration["HANGFIRE_PASSWORD"],
                 User = configuration["HANGFIRE_USER"]
             }
        }
        };
    }
}
