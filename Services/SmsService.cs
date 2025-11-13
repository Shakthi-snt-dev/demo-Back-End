using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace FlowTap.Api.Services;

public class SmsService : ISmsService
{
    private readonly IConfiguration _configuration;

    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<bool> SendSmsAsync(string to, string message)
    {
        try
        {
            // Twilio implementation would go here
            // For now, return true as placeholder
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}

