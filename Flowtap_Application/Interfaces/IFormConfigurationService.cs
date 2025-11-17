using Flowtap_Domain.DtoModel;

namespace Flowtap_Application.Interfaces;

public interface IFormConfigurationService
{
    Task<FormConfigurationDto> GetFormConfigurationAsync(string formId, Guid? appUserId = null, Guid? storeId = null);
    Task<FormConfigurationDto> GetUserProfileFormConfigurationAsync(Guid appUserId);
    Task<FormConfigurationDto> GetStoreSettingsFormConfigurationAsync(Guid storeId);
    Task<FormConfigurationDto> GetEmployeeFormConfigurationAsync(Guid? storeId = null);
}

