using TicketNow.Service.Business;
using TicketNow.Infra.Data.Context;

namespace Customer.Infra.Data.Context;

public static class SeedData
{
    public static void EnsureSeedData(this ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        if (context.AllMigrationsApplied())
        {
            SeedHistoryEvaluator.ApplySeedHistory(context, serviceProvider);
        }
    }
}
