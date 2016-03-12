using System.Data.Entity.ModelConfiguration;
using Blogfolio.Models.Blog;

namespace Blogfolio.Data.Configuration.Blog
{
    internal class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        /// <summary>
        /// Fluent API configuration for the Comment table.
        /// </summary>
        internal CommentConfiguration()
        {
            ToTable("Comment");

            HasKey(x => x.CommentId)
                .Property(x => x.CommentId)
                .HasColumnName("CommentId")
                .HasColumnType("uniqueidentifier")
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

            Property(x => x.Website)
                .HasColumnName("Website")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsOptional();

            Property(x => x.Content)
                .HasColumnName("Content")
                .HasColumnType("nvarchar")
                .HasMaxLength(1000)
                .IsRequired();

            Property(x => x.Status)
                .HasColumnName("Status")
                .IsRequired();

            Property(x => x.DateCreated)
                .HasColumnName("DateCreated")
                .HasColumnType("datetime2")
                .IsRequired();

            Property(x => x.DateModified)
                .HasColumnName("DateModified")
                .HasColumnType("datetime2")
                .IsOptional();

            HasRequired(x => x.Post)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.PostId);
        }
    }
}