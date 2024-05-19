using Devoiture.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Devoiture.Helpers
{
    public class CustomerAuthorizeFilter : IAuthorizationFilter
    {
        private readonly Devoiture1Context _context;
        public CustomerAuthorizeFilter(Devoiture1Context context)
        {
            _context = context;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Kiểm tra quyền của tài khoản
            if (!user.HasClaim(c => c.Type == ClaimTypes.Role && (c.Value == "3")))
            {
                // Chuyển hướng đến trang "access denied" nếu không có quyền truy cập
                context.Result = new ViewResult { ViewName = "AccessDenied" };
            }
            var email = user.FindFirstValue(ClaimTypes.Email);
            var dbContext = context.HttpContext.RequestServices.GetService<Devoiture1Context>();
            var taiKhoan = dbContext.Taikhoans
                .Include(t => t.IdQuyenNavigation)
                .FirstOrDefault(t => t.Email == email);

            if (taiKhoan == null)
            {
                context.Result = new ViewResult { ViewName = "AccessDenied" };
                return;
            }
            if (taiKhoan.Lock == true)
            {
                context.Result = new ViewResult { ViewName = "LockAcc" };
                return;
            }
            var quyen = taiKhoan.IdQuyenNavigation;

            // Lấy tên hành động và controller hiện tại
            var action = context.RouteData.Values["action"]?.ToString();
            var controller = context.RouteData.Values["controller"]?.ToString();

            // Kiểm tra quyền dựa trên tên hành động và controller
            if (controller == "XeKH")
            {
                if (action == "DanhsachxecuaKH" && quyen.Websites.FirstOrDefault()?.Quyentruycap != true)
                {
                    context.Result = new ViewResult { ViewName = "AccessDenied" };
                }
                // Thêm các kiểm tra quyền khác nếu cần
                if (action == "ThemXe" && quyen.Websites.FirstOrDefault()?.Create != true)
                {
                    context.Result = new ViewResult { ViewName = "AccessDenied" };
                }
                if (action == "SuaXe" && quyen.Websites.FirstOrDefault()?.Update != true)
                {
                    context.Result = new ViewResult { ViewName = "AccessDenied" };
                }
                if (action == "XoaXe" && quyen.Websites.FirstOrDefault()?.Delete != true)
                {
                    context.Result = new ViewResult { ViewName = "AccessDenied" };
                }
            }
        }
    }
}
