using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.CommonDtos;
using eShopSolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AppService.Catalog.Products.Implement
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDbContext _dbcontext;

        public PublicProductService(EShopDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        // get product filter by category when user choose catecory ID 
        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            var query = from p in _dbcontext.Products
                        join pt in _dbcontext.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _dbcontext.ProductInCategories on p.Id equals pic.ProductId
                        join c in _dbcontext.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            // 2. Filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            //This is agorithm paging
            var data = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //4. Select and projection
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = await data
            };
            return pageResult;
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            var query = from p in _dbcontext.Products
                        join pt in _dbcontext.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _dbcontext.ProductInCategories on p.Id equals pic.ProductId
                        join c in _dbcontext.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            var data = await query.Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                Name = x.pt.Name,
                DateCreated = x.p.DateCreated,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = x.pt.LanguageId,
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount
            }).ToListAsync();
            return data;

        }
    }
}
