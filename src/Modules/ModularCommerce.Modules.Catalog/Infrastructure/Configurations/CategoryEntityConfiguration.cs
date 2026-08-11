using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularCommerce.Modules.Catalog.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Infrastructure.Configurations
{
    internal class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), CategoryName = "Auto", ParentCategoryId = (Guid?)null },
                new { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), CategoryName = "Sport", ParentCategoryId = (Guid?)null },
                new { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), CategoryName = "Beauty", ParentCategoryId = (Guid?)null }
            );
        }
    }
}
