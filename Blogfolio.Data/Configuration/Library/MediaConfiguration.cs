using System.Data.Entity.ModelConfiguration;
using Blogfolio.Models.Library;

namespace Blogfolio.Data.Configuration.Library
{
    internal class MediaConfiguration : EntityTypeConfiguration<Media>
    {
        /// <summary>
        /// Fluent API configuration for the Media table
        /// </summary>
        internal MediaConfiguration()
        {
            ToTable("Media");

            HasKey(x => x.MediaId)
                .Property(x => x.MediaId)
                .HasColumnName("MediaId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            Property(x => x.Path)
                .HasColumnName("Path")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsRequired();

            Property(x => x.ThumbPath)
                .HasColumnName("ThumbPath")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsRequired();

            Property(x => x.Type)
                .HasColumnName("Type")
                .HasColumnType("nvarchar")
                .HasMaxLength(64)
                .IsOptional();

            Property(x => x.Size)
                .HasColumnName("Size")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsOptional();

            Property(x => x.DateCreated)
                .HasColumnName("DateCreated")
                .HasColumnType("datetime2")
                .IsRequired();

            Property(x => x.DateModified)
                .HasColumnName("DateModified")
                .HasColumnType("datetime2")
                .IsOptional();
        }
    }
}