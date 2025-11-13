using FlowTap.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleDto = FlowTap.Api.Services.SaleDto;
using CreateSaleRequest = FlowTap.Api.Services.CreateSaleRequest;

namespace FlowTap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PosController : ControllerBase
{
    private readonly IPosService _posService;

    public PosController(IPosService posService)
    {
        _posService = posService;
    }

    [HttpPost("sale")]
    public async Task<ActionResult<SaleDto>> CreateSale([FromBody] CreateSaleRequest request)
    {
        try
        {
            var sale = await _posService.CreateSaleAsync(request);
            var saleDto = await _posService.GetSaleByIdAsync(sale.Id);
            return Ok(saleDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("sales")]
    public async Task<ActionResult<List<SaleDto>>> GetSales(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var sales = await _posService.GetSalesAsync(startDate, endDate);
        return Ok(sales);
    }

    [HttpGet("sales/{id}")]
    public async Task<ActionResult<SaleDto>> GetSale(Guid id)
    {
        try
        {
            var sale = await _posService.GetSaleByIdAsync(id);
            return Ok(sale);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("refund/{id}")]
    public async Task<ActionResult> ProcessRefund(Guid id)
    {
        try
        {
            await _posService.ProcessRefundAsync(id);
            return Ok(new { message = "Refund processed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

