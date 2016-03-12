using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Blogfolio.Models.Blog;

namespace Blogfolio.Data.Configuration.Blog
{
    internal class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        /// <summary>
        /// Fluent API configuration for the Category table.
        /// </summary>
        internal CategoryConfiguration()
        {
            ToTable("Category");

            HasKey(x => x.CategoryId)
                .Property(x => x.CategoryId)
                .HasColumnName("CategoryId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();

            Property(x => x.Slug)
                .HasColumnName("Slug")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Slug") {IsUnique = true}))
                .IsRequired();

            Property(x => x.DateCreated)
                .HasColumnName("DateCreated")
                .HasColumnType("datetime2")
                .IsRequired();

            Property(x => x.DateModified)
                .HasColumnName("DateModified")
                .HasColumnType("datetime2")
                .IsOptional();

            HasMany(x => x.Posts)
                .WithMany(x => x.Categories)
                .Map(x =>
                {
                    x.ToTable("PostCategory");
                    x.MapLeftKey("PostId");
                    x.MapRightKey("CategoryId");
                });
        }
    }
}