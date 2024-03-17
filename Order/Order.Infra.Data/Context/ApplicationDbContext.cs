using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using Order.Infra.Data.Mapping;

namespace Order.Infra.Data.Context
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Domain.Entities.Order> Orders { get; set; }
		public DbSet<SeedHistory> SeedHistories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Domain.Entities.Order>(new OrderMap().Configure);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}
	}
}
