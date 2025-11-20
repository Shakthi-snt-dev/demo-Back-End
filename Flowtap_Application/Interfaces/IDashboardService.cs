using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponseDto> GetDashboardDataAsync(Guid? storeId = null, DateTime? startDate = null, DateTime? endDate = null);
    Task<DashboardStatsResponseDto> GetDashboardStatsAsync(Guid? storeId = null);
    Task<List<SalesChartDataDto>> GetSalesChartDataAsync(Guid? storeId = null, int days = 7);
    Task<List<TopProductDto>> GetTopProductsAsync(Guid? storeId = null, int limit = 5);
    Task<List<RecentOrderDto>> GetRecentOrdersAsync(Guid? storeId = null, int limit = 10);
}

