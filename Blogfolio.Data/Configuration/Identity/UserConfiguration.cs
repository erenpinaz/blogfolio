using System.Data.Entity.ModelConfiguration;
using Blogfolio.Models.Identity;

namespace Blogfolio.Data.Configuration.Identity
{
    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        /// <summary>
        /// Fluent API configuration for the User table.
        /// </summary>
        internal UserConfiguration()
        {
            ToTable("User");

            HasKey(x => x.UserId)
                .Property(x => x.UserId)
                .HasColumnName("UserId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(x => x.SecurityStamp)
                .HasColumnName("SecurityStamp")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(x => x.UserName)
                .HasColumnName("UserName")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();

            Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("nvarchar")
                .HasMaxLength(320)
                .IsRequired();

            HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .Map(x =>
                {
                    x.ToTable("UserRole");
                    x.MapLeftKey("UserId");
                    x.MapRightKey("RoleId");
                });

            HasMany(x => x.Claims)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);

            HasMany(x => x.Logins)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);

            HasMany(x => x.Posts)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}