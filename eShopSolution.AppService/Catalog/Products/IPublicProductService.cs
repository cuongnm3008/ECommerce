using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.CommonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AppService.Catalog.Products
{
    
    public interface IPublicProductService
    {
       Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);
       
       Task<List<ProductViewModel>> GetAll(string languageId);
    }
}
