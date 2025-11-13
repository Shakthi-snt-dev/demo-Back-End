using FlowTap.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductDto = FlowTap.Api.Services.ProductDto;
using CreateProductRequest = FlowTap.Api.Services.CreateProductRequest;
using UpdateProductRequest = FlowTap.Api.Services.UpdateProductRequest;

namespace FlowTap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("products")]
    public async Task<ActionResult<List<ProductDto>>> GetProducts(
        [FromQuery] string? category,
        [FromQuery] string? search)
    {
        var products = await _inventoryService.GetProductsAsync(category, search);
        return Ok(products);
    }

    [HttpPost("products")]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductRequest request)
    {
        try
        {
            var product = await _inventoryService.CreateProductAsync(request);
            return Ok(new ProductDto
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Cost = product.Cost,
                RetailPrice = product.RetailPrice,
                Stock = product.Stock,
                LowStockAlert = product.LowStockAlert,
                Supplier = product.Supplier,
                SyncShopify = product.SyncShopify,
                SyncWooCommerce = product.SyncWooCommerce,
                CreatedAt = product.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("products/{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
    {
        try
        {
            var product = await _inventoryService.UpdateProductAsync(id, request);
            return Ok(new ProductDto
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Cost = product.Cost,
                RetailPrice = product.RetailPrice,
                Stock = product.Stock,
                LowStockAlert = product.LowStockAlert,
                Supplier = product.Supplier,
                SyncShopify = product.SyncShopify,
                SyncWooCommerce = product.SyncWooCommerce,
                CreatedAt = product.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("products/{id}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        try
        {
            await _inventoryService.DeleteProductAsync(id);
            return Ok(new { message = "Product deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

