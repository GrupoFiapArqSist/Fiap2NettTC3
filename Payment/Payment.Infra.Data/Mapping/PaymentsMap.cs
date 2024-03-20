using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Payment.Infra.Data.Mapping
{
    public class PaymentsMap : IEntityTypeConfiguration<Payments>
    {
        public void Configure(EntityTypeBuilder<Payments> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(prop => prop.Id);
        }
    }
}
