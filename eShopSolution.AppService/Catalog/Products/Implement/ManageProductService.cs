using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.CommonDtos;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopSolution.AppService.Common;

namespace eShopSolution.AppService.Catalog.Products.Implement
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _dbcontext;
        private readonly IStorageService _storageService;

        public ManageProductService(EShopDbContext dbcontext, IStorageService storageService)
        {
            _dbcontext = dbcontext;
            _storageService = storageService;
        }

    

        public async Task AddViewCount(int productId)
        {
            var product = await _dbcontext.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _dbcontext.SaveChangesAsync();
        }


        // Create product
        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation(){
                       Name = request.Name,
                       Description = request.Description,
                       Details = request.Details,
                       SeoDescription = request.SeoDescription,
                       SeoTitle = request.SeoTitle,
                       SeoAlias = request.SeoAlias,
                       LanguageId = request.LanguageId
                    }
                }
            };
            //Save image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            _dbcontext.Products.Add(product);
            return await _dbcontext.SaveChangesAsync();
        }





        public async Task<int> Delete(int productId)
        {
            var product = await _dbcontext.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            var images = _dbcontext.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                // xóa cả bản ghi trong db nữa 
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
            _dbcontext.Products.Remove(product);
            return await _dbcontext.SaveChangesAsync(); // return total record was delete
        }

        // Get list product with to paging
        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            
            // 1. Select join
            var query = from p in _dbcontext.Products
                        join pt in _dbcontext.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _dbcontext.ProductInCategories on p.Id equals pic.ProductId
                        join c in _dbcontext.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            // 2. Filter
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                query = query.Where(x => x.pt.Name.Contains(request.KeyWord));
            }
            if (request.CategoryIds.Count>0)
            {
                query = query.Where(p=>request.CategoryIds.Contains(p.pic.CategoryId));
            }
            //3. Paging
            int totalRow =await query.CountAsync();

            //This is agorithm paging
            var data = query.Skip((request.PageIndex-1)*request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel() {
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

      


        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _dbcontext.Products.FindAsync(request.Id);
            var productTranslation = await _dbcontext.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId==request.Id
            &&x.LanguageId==request.LanguageId);
            if (product==null)
            {
                throw new EShopException($"Cannot find a product: {request.Id}");
            }
            else
            {
                productTranslation.Name = request.Name;
                productTranslation.Description = request.Description;
                productTranslation.Details = request.Details;
                productTranslation.SeoDescription = request.SeoDescription;
                productTranslation.SeoTitle = request.SeoTitle;
                productTranslation.SeoAlias = request.SeoAlias;
                //Save image
                if (request.ThumbnailImage != null)
                {
                    var thumbnailImage = await _dbcontext.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                    if (thumbnailImage != null)
                    {
                        thumbnailImage.FileSize = request.ThumbnailImage.Length;
                        thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                        _dbcontext.ProductImages.Update(thumbnailImage);
                    }
                }
                return await  _dbcontext.SaveChangesAsync();
            }
        }

     

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _dbcontext.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EShopException($"Cannot find a product: {productId}");
            }
            else
            {
                product.Price = newPrice;
                return await _dbcontext.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _dbcontext.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EShopException($"Cannot find a product: {productId}");
            }
            else
            {
                product.Stock += addedQuantity;
                return await _dbcontext.SaveChangesAsync() > 0;
            }
        }


        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        /*
            Service for image of product
        */
        public Task<int> AddImages(int productId, List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateImage(int imageId, string caption, bool isDefault)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveImages(int imageId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            throw new NotImplementedException();
        }


    }
}
