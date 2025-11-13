using FlowTap.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntegrationDto = FlowTap.Api.Services.IntegrationDto;
using ConnectShopifyRequest = FlowTap.Api.Services.ConnectShopifyRequest;
using ConnectWooCommerceRequest = FlowTap.Api.Services.ConnectWooCommerceRequest;
using ConnectQuickBooksRequest = FlowTap.Api.Services.ConnectQuickBooksRequest;

namespace FlowTap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IntegrationsController : ControllerBase
{
    private readonly IIntegrationService _integrationService;

    public IntegrationsController(IIntegrationService integrationService)
    {
        _integrationService = integrationService;
    }

    [HttpPost("shopify/connect")]
    public async Task<ActionResult<IntegrationDto>> ConnectShopify([FromBody] ConnectShopifyRequest request)
    {
        try
        {
            var integration = await _integrationService.ConnectShopifyAsync(request);
            return Ok(new IntegrationDto
            {
                Id = integration.Id,
                Type = integration.Type.ToString(),
                Status = integration.Status.ToString(),
                LastSync = integration.LastSync,
                ErrorLog = integration.ErrorLog
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("shopify/sync")]
    public async Task<ActionResult> SyncShopify()
    {
        try
        {
            await _integrationService.SyncShopifyAsync();
            return Ok(new { message = "Sync initiated" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("woocommerce/connect")]
    public async Task<ActionResult<IntegrationDto>> ConnectWooCommerce([FromBody] ConnectWooCommerceRequest request)
    {
        try
        {
            var integration = await _integrationService.ConnectWooCommerceAsync(request);
            return Ok(new IntegrationDto
            {
                Id = integration.Id,
                Type = integration.Type.ToString(),
                Status = integration.Status.ToString(),
                LastSync = integration.LastSync,
                ErrorLog = integration.ErrorLog
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("quickbooks/connect")]
    public async Task<ActionResult<IntegrationDto>> ConnectQuickBooks([FromBody] ConnectQuickBooksRequest request)
    {
        try
        {
            var integration = await _integrationService.ConnectQuickBooksAsync(request);
            return Ok(new IntegrationDto
            {
                Id = integration.Id,
                Type = integration.Type.ToString(),
                Status = integration.Status.ToString(),
                LastSync = integration.LastSync,
                ErrorLog = integration.ErrorLog
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("status")]
    public async Task<ActionResult<List<IntegrationDto>>> GetIntegrationStatus()
    {
        var integrations = await _integrationService.GetIntegrationStatusAsync();
        return Ok(integrations);
    }

    [HttpPost("api-key/reset")]
    public async Task<ActionResult> ResetApiKey()
    {
        try
        {
            await _integrationService.ResetApiKeyAsync();
            return Ok(new { message = "API key reset successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

