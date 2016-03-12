using System.Data.Entity.ModelConfiguration;
using Blogfolio.Models.Identity;

namespace Blogfolio.Data.Configuration.Identity
{
    internal class ExternalLoginConfiguration : EntityTypeConfiguration<ExternalLogin>
    {
        /// <summary>
        ///     Fluent API configuration for the ExternalLogin table.
        /// </summary>
        internal ExternalLoginConfiguration()
        {
            ToTable("ExternalLogin");

            HasKey(x => new {x.LoginProvider, x.ProviderKey, x.UserId});

            Property(x => x.LoginProvider)
                .HasColumnName("LoginProvider")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(x => x.ProviderKey)
                .HasColumnName("ProviderKey")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(x => x.UserId)
                .HasColumnName("UserId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            HasRequired(x => x.User)
                .WithMany(x => x.Logins)
                .HasForeignKey(x => x.UserId);
        }
    }
}