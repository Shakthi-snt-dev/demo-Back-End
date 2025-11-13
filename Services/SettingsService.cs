using FlowTap.Api.Data;
using FlowTap.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowTap.Api.Services;

public class SettingsService : ISettingsService
{
    private readonly ApplicationDbContext _context;

    public SettingsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StoreDto> GetStoreSettingsAsync()
    {
        var store = await _context.Stores.FirstOrDefaultAsync();
        if (store == null)
        {
            // Create default store
            store = new Store
            {
                Name = "Default Store",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
        }

        return new StoreDto
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
        };
    }

    public async Task<Store> UpdateStoreSettingsAsync(UpdateStoreRequest request)
    {
        var store = await _context.Stores.FirstOrDefaultAsync();
        if (store == null)
        {
            store = new Store { CreatedAt = DateTime.UtcNow };
            _context.Stores.Add(store);
        }

        if (!string.IsNullOrEmpty(request.Name))
            store.Name = request.Name;
        if (request.Email != null)
            store.Email = request.Email;
        if (request.Phone != null)
            store.Phone = request.Phone;
        if (request.Address != null)
            store.Address = request.Address;
        if (request.City != null)
            store.City = request.City;
        if (request.State != null)
            store.State = request.State;
        if (request.Country != null)
            store.Country = request.Country;
        if (request.Zip != null)
            store.Zip = request.Zip;
        if (request.TimeZone != null)
            store.TimeZone = request.TimeZone;
        if (request.Currency != null)
            store.Currency = request.Currency;
        if (request.TaxRate.HasValue)
            store.TaxRate = request.TaxRate.Value;

        store.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return store;
    }

    public async Task<BusinessSettingsDto> GetBusinessSettingsAsync()
    {
        var settings = await _context.BusinessSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new BusinessSettings
            {
                BusinessName = "FlowTap",
                TimeFormat = "12h",
                Language = "en",
                DefaultCurrency = "USD",
                AccountingMethod = "Cash",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.BusinessSettings.Add(settings);
            await _context.SaveChangesAsync();
        }

        return new BusinessSettingsDto
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
        };
    }

    public async Task<BusinessSettings> UpdateBusinessSettingsAsync(UpdateBusinessSettingsRequest request)
    {
        var settings = await _context.BusinessSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new BusinessSettings { CreatedAt = DateTime.UtcNow };
            _context.BusinessSettings.Add(settings);
        }

        if (!string.IsNullOrEmpty(request.BusinessName))
            settings.BusinessName = request.BusinessName;
        if (request.LogoUrl != null)
            settings.LogoUrl = request.LogoUrl;
        if (request.DefaultStoreId.HasValue)
            settings.DefaultStoreId = request.DefaultStoreId;
        if (request.TimeFormat != null)
            settings.TimeFormat = request.TimeFormat;
        if (request.Language != null)
            settings.Language = request.Language;
        if (request.DefaultCurrency != null)
            settings.DefaultCurrency = request.DefaultCurrency;
        if (request.TaxIncluded.HasValue)
            settings.TaxIncluded = request.TaxIncluded.Value;
        if (request.AccountingMethod != null)
            settings.AccountingMethod = request.AccountingMethod;

        settings.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return settings;
    }
}

