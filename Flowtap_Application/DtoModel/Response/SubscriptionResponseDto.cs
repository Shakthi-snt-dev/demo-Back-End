namespace Flowtap_Application.DtoModel.Response;

public class UpgradeToSubscriptionResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid SubscriptionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string PlanName { get; set; } = string.Empty;
}

