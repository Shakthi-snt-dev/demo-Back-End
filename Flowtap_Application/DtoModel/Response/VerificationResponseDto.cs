namespace Flowtap_Application.DtoModel.Response;

public class VerifyEmailResponseDto
{
    public bool IsVerified { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? AppUserId { get; set; }
    public int OnboardingStep { get; set; } = 1;
}

public class OnboardingResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int CurrentStep { get; set; }
    public Guid? StoreId { get; set; }
    public bool TrialStarted { get; set; }
    public DateTime? TrialEndDate { get; set; }
}

