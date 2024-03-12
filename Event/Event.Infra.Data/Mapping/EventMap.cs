using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Event.Domain.Enums;

namespace Event.Infra.Data.Mapping;

public class EventMap : IEntityTypeConfiguration<Domain.Entities.Event>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Event> builder)
    {
        builder.ToTable("Event");
        builder.HasKey(prop => prop.Id);

        #region Fields            
        builder.Property(prop => prop.Name)
               .HasConversion(prop => prop.ToString(), prop => prop)
               .IsRequired()
               .HasColumnName("Name")
               .HasColumnType("varchar(150)");

        builder.Property(prop => prop.Address)
               .HasConversion(prop => prop.ToString(), prop => prop)
               .IsRequired()
               .HasColumnName("Address")
               .HasColumnType("varchar(150)");

        builder.Property(prop => prop.City)
               .HasConversion(prop => prop.ToString(), prop => prop)
               .IsRequired()
               .HasColumnName("City")
               .HasColumnType("varchar(50)");

        builder.Property(prop => prop.UF)
               .HasConversion(prop => prop.ToString(), prop => prop)
               .IsRequired()
               .HasColumnName("UF")
               .HasColumnType("varchar(2)");

        builder.Property(prop => prop.Description)
               .HasConversion(prop => prop.ToString(), prop => prop)
               .IsRequired()
               .HasColumnName("Description")
               .HasColumnType("varchar(max)");

        builder.Property(prop => prop.EventTime)
               .HasConversion(prop => prop.ToString(), prop => prop)
               .IsRequired()
               .HasColumnName("EventTime")
               .HasColumnType("varchar(5)");

        builder.Property(e => e.Category)
               .HasMaxLength(50)
               .HasConversion(
                   v => v.ToString(),
                   v => (CategoryEnum)Enum.Parse(typeof(CategoryEnum), v))
                   .IsUnicode(false);

        builder.Property(p => p.TicketPrice)
               .IsRequired()
               .HasColumnType("decimal(18, 2)");

        builder.Property(prop => prop.Active)
               .HasConversion(prop => Convert.ToBoolean(prop), prop => prop)
               .IsRequired()
               .HasColumnName("Active")
               .HasColumnType("bit")
               .HasDefaultValue(false);

        builder.Property(prop => prop.Approved)
               .HasConversion(prop => Convert.ToBoolean(prop), prop => prop)
               .IsRequired()
               .HasColumnName("Approved")
               .HasColumnType("bit")
               .HasDefaultValue(false);
        #endregion


    }
}
