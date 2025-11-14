using Flowtap_Domain.BoundedContexts.Identity.Entities;

namespace Flowtap_Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserAccount userAccount);
    string GenerateToken(UserAccount userAccount, Guid? appUserId);
}

