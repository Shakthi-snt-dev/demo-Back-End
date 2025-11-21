using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Flowtap_Application.Interfaces;

namespace Flowtap_Presentation.Attributes;

/// <summary>
/// Authorization attribute that allows access only for Employees with Role = Owner
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AppUserOrOwnerAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Get required services
        var httpAccessorService = context.HttpContext.RequestServices.GetRequiredService<IHttpAccessorService>();
        var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

        // Check if user is authenticated
        if (!httpAccessorService.IsAuthenticated())
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                success = false,
                message = "User is not authenticated"
            });
            return;
        }

        var userAccountId = httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                success = false,
                message = "Unable to identify user account"
            });
            return;
        }

        // Check if user is an Employee with Owner role using the authorization service
        var isAuthorized = await authorizationService.IsEmployeeOwnerAsync(userAccountId.Value);
        
        if (!isAuthorized)
        {
            // If we reach here, user is not authorized
            context.Result = new ForbidResult();
        }
    }
}

