using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");
        builder.HasKey(s => s.id);

        builder.Property(s => s.id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.title)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(s => s.coverid)
            .IsUnique();
        builder.Property(s => s.author)
            .HasMaxLength(100)
            .IsRequired();
    }
}