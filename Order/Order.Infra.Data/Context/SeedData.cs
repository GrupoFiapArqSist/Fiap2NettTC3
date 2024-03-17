using Order.Service.Services.Business;

namespace Order.Infra.Data.Context
{
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
}
