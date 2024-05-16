using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Devoiture.Helpers
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Kiểm tra quyền của tài khoản
            if (!user.HasClaim(c => c.Type == ClaimTypes.Role && (c.Value == "1" || c.Value == "2")))
            {
                // Chuyển hướng đến trang "access denied" nếu không có quyền truy cập
                context.Result = new ViewResult { ViewName = "AccessDenied" };
            }
        }
    }
}
