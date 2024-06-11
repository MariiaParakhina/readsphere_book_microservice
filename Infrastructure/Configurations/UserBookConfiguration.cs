 using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserBookConfiguration: IEntityTypeConfiguration<UserBook> 
{
     
    public void Configure(EntityTypeBuilder<UserBook> builder)
    {
        builder.ToTable("user_book");
        builder.Property(s=> s.ishidden)
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.HasKey(ub => new { UserId = ub.userid, ub.bookid });
    }
}