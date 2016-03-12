using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Blogfolio.Models.Blog;

namespace Blogfolio.Data.Configuration.Blog
{
    internal class PostConfiguration : EntityTypeConfiguration<Post>
    {
        /// <summary>
        /// Fluent API configuration for the Post table.
        /// </summary>
        internal PostConfiguration()
        {
            ToTable("Post");

            HasKey(x => x.PostId)
                .Property(x => x.PostId)
                .HasColumnName("PostId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.Title)
                .HasColumnName("Title")
                .HasColumnType("nvarchar")
                .HasMaxLength(64)
                .IsRequired();

            Property(x => x.Summary)
                .HasColumnName("Summary")
                .HasColumnType("nvarchar")
                .HasMaxLength(320)
                .IsRequired();

            Property(x => x.Content)
                .HasColumnName("Content")
                .HasColumnType("text")
                .IsMaxLength()
                .IsRequired();

            Property(x => x.Slug)
                .HasColumnName("Slug")
                .HasColumnType("nvarchar")
                .HasMaxLength(64)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Slug") {IsUnique = true}))
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

            HasMany(x => x.Categories)
                .WithMany(x => x.Posts)
                .Map(x =>
                {
                    x.ToTable("PostCategory");
                    x.MapLeftKey("PostId");
                    x.MapRightKey("CategoryId");
                });

            HasMany(x => x.Comments)
                .WithRequired(x => x.Post)
                .HasForeignKey(x => x.PostId);

            HasRequired(x => x.User)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.UserId);
        }
    }
}