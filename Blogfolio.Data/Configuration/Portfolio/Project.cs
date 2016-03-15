using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Blogfolio.Models.Portfolio;

namespace Blogfolio.Data.Configuration.Portfolio
{
    internal class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        /// <summary>
        /// Fluent API configuration for the Project table
        /// </summary>
        internal ProjectConfiguration()
        {
            ToTable("Project");

            HasKey(x => x.ProjectId)
                .Property(x => x.ProjectId)
                .HasColumnName("ProjectId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .HasMaxLength(64)
                .IsRequired();

            Property(x => x.Image)
                .HasColumnName("Image")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsRequired();

            Property(x => x.Description)
                .HasColumnName("Description")
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
        }
    }
}