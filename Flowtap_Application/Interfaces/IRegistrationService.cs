using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IRegistrationService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<VerifyEmailResponseDto> VerifyEmailAsync(string token);
    Task<OnboardingResponseDto> CompleteOnboardingStep1Async(Guid appUserId, OnboardingStep1RequestDto request);
    Task<OnboardingResponseDto> CompleteOnboardingStep2Async(Guid appUserId, OnboardingStep2RequestDto request);
    Task<OnboardingResponseDto> CompleteOnboardingStep3Async(Guid appUserId, OnboardingStep3RequestDto request);
    Task<UpgradeToSubscriptionResponseDto> UpgradeToSubscriptionAsync(Guid appUserId, UpgradeToSubscriptionRequestDto request);
}

