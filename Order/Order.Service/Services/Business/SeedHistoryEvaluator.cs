using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TicketNow.Domain.Entities;

namespace Order.Service.Services.Business;

public static class SeedHistoryEvaluator
{
    public static void ApplySeedHistory(DbContext context, IServiceProvider serviceProvider)
    {
        var configurationType = context.GetType();
        var seedTypes = configurationType.Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Seed)) && t.Namespace == $"{configurationType.Namespace.Replace(".Context", "")}.Seeds._SeedHistory").OrderBy(t => t.Name);
        var setSeedHistory = context.Set<SeedHistory>().ToList();

        foreach (var seedType in seedTypes)
        {
            var validateSeed = ValidateSeed(seedType);

            if (!validateSeed)
                throw new ApplicationException($"Invalid seed history class name ({seedType.Name}). Format must be: 'Seed_yyyyMMddHHmmss_[SeedName]', where 'yyyyMMddHHmmss must represents a past date.");

            Seed seed = Seed.CreateInstance(seedType, context, serviceProvider);
            var seedHistory = new SeedHistory(seed);

            if (setSeedHistory.All(s => s.SeedId != seedHistory.SeedId))
            {
                seed.Up();
                context.SaveChanges();

                context.Set<SeedHistory>().Add(seedHistory);
                context.SaveChanges();
            }
        }
    }

    private static bool ValidateSeed(Type seedType)
    {
        var seedName = seedType.Name;
        if (!seedName.StartsWith("Seed_"))
            return false;

        var seedValues = seedName.Split('_');
        if (!ValidateSeedDateOnName(seedValues[1]))
            return false;

        return true;
    }

    private static bool ValidateSeedDateOnName(string seedDate)
    {
        var dateTime = new DateTime();
        if (!string.IsNullOrEmpty(seedDate))
            return DateTime.TryParseExact(seedDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) && dateTime < DateTime.UtcNow;

        return false;
    }
}
