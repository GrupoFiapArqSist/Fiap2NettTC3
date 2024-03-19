using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Infra.Data.Mapping;

public class OrderMap : IEntityTypeConfiguration<Domain.Entities.Order>
{
	public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
	{
		builder.ToTable("Order");
		builder.HasKey(prop => prop.Id);

		builder.HasMany(e => e.OrderItens)
				.WithOne(e => e.Order)
				.HasForeignKey(e => e.OrderId)
				.HasPrincipalKey(e => e.Id);
	}
}