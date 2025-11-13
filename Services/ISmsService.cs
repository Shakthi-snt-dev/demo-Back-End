namespace FlowTap.Api.Services;

public interface ISmsService
{
    Task<bool> SendSmsAsync(string to, string message);
}

