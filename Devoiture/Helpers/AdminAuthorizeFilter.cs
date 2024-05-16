using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Devoiture.Helpers
{
    public class AdminAuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.HasClaim(c => c.Type == ClaimTypes.Role && (c.Value == "1")))
            {
                // Chuyển hướng đến trang "access denied" nếu IdQuyen không phù hợp
                context.Result = new ViewResult { ViewName = "AccessDenied" };
                return;
            }
        }
    }
}
