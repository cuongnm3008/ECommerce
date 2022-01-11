using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImage");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ImagePath).IsRequired(true).HasMaxLength(200);
            builder.Property(x => x.Caption).IsRequired(false).HasMaxLength(200);
            builder.HasOne(x => x.Product).WithMany(x => x.ProductImages).HasForeignKey(x=>x.ProductId);
        }   
    }
}
