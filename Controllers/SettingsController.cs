using FlowTap.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreDto = FlowTap.Api.Services.StoreDto;
using UpdateStoreRequest = FlowTap.Api.Services.UpdateStoreRequest;
using BusinessSettingsDto = FlowTap.Api.Services.BusinessSettingsDto;
using UpdateBusinessSettingsRequest = FlowTap.Api.Services.UpdateBusinessSettingsRequest;

namespace FlowTap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpGet("store")]
    public async Task<ActionResult<StoreDto>> GetStoreSettings()
    {
        var store = await _settingsService.GetStoreSettingsAsync();
        return Ok(store);
    }

    [HttpPut("store")]
    public async Task<ActionResult<StoreDto>> UpdateStoreSettings([FromBody] UpdateStoreRequest request)
    {
        try
        {
            var store = await _settingsService.UpdateStoreSettingsAsync(request);
            return Ok(new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Email = store.Email,
                Phone = store.Phone,
                Address = store.Address,
                City = store.City,
                State = store.State,
                Country = store.Country,
                Zip = store.Zip,
                TimeZone = store.TimeZone,
                Currency = store.Currency,
                TaxRate = store.TaxRate
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("business")]
    public async Task<ActionResult<BusinessSettingsDto>> GetBusinessSettings()
    {
        var settings = await _settingsService.GetBusinessSettingsAsync();
        return Ok(settings);
    }

    [HttpPut("business")]
    public async Task<ActionResult<BusinessSettingsDto>> UpdateBusinessSettings([FromBody] UpdateBusinessSettingsRequest request)
    {
        try
        {
            var settings = await _settingsService.UpdateBusinessSettingsAsync(request);
            return Ok(new BusinessSettingsDto
            {
                Id = settings.Id,
                BusinessName = settings.BusinessName,
                LogoUrl = settings.LogoUrl,
                DefaultStoreId = settings.DefaultStoreId,
                TimeFormat = settings.TimeFormat,
                Language = settings.Language,
                DefaultCurrency = settings.DefaultCurrency,
                TaxIncluded = settings.TaxIncluded,
                AccountingMethod = settings.AccountingMethod
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

