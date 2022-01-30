using eShopSolution.AppService.Catalog.Products;
using eShopSolution.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ViewModel.Catalog.ProductImages;
using Microsoft.AspNetCore.Authorization;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        //http://localhost:port/product
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var products = await _productService.GetAll(languageId);
            return Ok(products);
        }

        //http://localhost:port/product/public-paging
        [HttpGet("public-paging/{languageId}")]
        public async Task<IActionResult> Get(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _productService.GetAllByCategoryId(languageId, request);
            return Ok(products);
        }

        //http://localhost:port/product/1
        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById(int id, string languageId)
        {
            var product = await _productService.GetById(id, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }

        [HttpPost]
        [Route("create-product")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _productService.Create(request);
            if (productId == 0)
            {
                return BadRequest(); // status : 400
            }
            //return NotFound(); // status : 404
            // return OK(); // status : 200

            var product = await _productService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product); // status : 201
        }

        [HttpPut]
        [Route("update-product")]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _productService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{productid}")]
        public async Task<IActionResult> Delete(int productid)
        {
            var affectedResult = await _productService.Delete(productid);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPatch("{productid}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productid, decimal newPrice)
        {
            var isSuccessful = await _productService.UpdatePrice(productid, newPrice);
            if (isSuccessful)
                return Ok();
            return BadRequest();
        }

        //Images
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _productService.AddImage(productId, request);
            if (imageId == 0)
                return BadRequest();

            var image = await _productService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpPut("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.UpdateImage(imageId, request);
            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.RemoveImage(imageId);
            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _productService.GetImageById(imageId);
            if (image == null)
                return BadRequest("Cannot find product");
            return Ok(image);
        }

        //[HttpPut("{id}/categories")]
        //[Authorize]
        //public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var result = await _productService.CategoryAssign(id, request);
        //    if (!result.IsSuccessed)
        //    {
        //        return BadRequest(result);
        //    }
        //    return Ok(result);
        //}
    }
}