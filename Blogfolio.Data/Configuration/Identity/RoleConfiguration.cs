using System.Data.Entity.ModelConfiguration;
using Blogfolio.Models.Identity;

namespace Blogfolio.Data.Configuration.Identity
{
    internal class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        /// <summary>
        /// Fluent API configuration for the Role table
        /// </summary>
        internal RoleConfiguration()
        {
            ToTable("Role");

            HasKey(x => x.RoleId)
                .Property(x => x.RoleId)
                .HasColumnName("RoleId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            HasMany(x => x.Users)
                .WithMany(x => x.Roles)
                .Map(x =>
                {
                    x.ToTable("UserRole");
                    x.MapLeftKey("RoleId");
                    x.MapRightKey("UserId");
                });
        }
    }
}