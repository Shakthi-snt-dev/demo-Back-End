using Microsoft.AspNetCore.Identity;

namespace FlowTap.Api.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public Dictionary<string, Dictionary<string, bool>> Permissions { get; set; } = new();
    public bool IsSuperUser { get; set; } = false;
}

