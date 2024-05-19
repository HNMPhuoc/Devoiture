using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Devoiture.Models;
using System.Linq;

namespace Devoiture.Helpers
{
    public class ManagerAuthorizeFilter : IAuthorizationFilter
    {
        private readonly Devoiture1Context _context;

        public ManagerAuthorizeFilter(Devoiture1Context context)
        {
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var email = user.FindFirstValue(ClaimTypes.Email);
            var taiKhoan = _context.Taikhoans
                    .Include(t => t.IdQuyenNavigation.Websites)
                    .FirstOrDefault(t => t.Email == email);

            if (taiKhoan != null && taiKhoan.Lock == true)
            {
                context.Result = new ViewResult { ViewName = "LockAcc" };
                return;
            }

            var quyen = taiKhoan?.IdQuyenNavigation;

            var action = context.RouteData.Values["action"]?.ToString();
            var controller = context.RouteData.Values["controller"]?.ToString();

            bool HasPermission(string websiteName, string permissionType)
            {
                var websitePermission = quyen.Websites.FirstOrDefault(w => w.TenWebsite == websiteName);
                if (websitePermission == null) return false;

                return permissionType switch
                {
                    "Quyentruycap" => websitePermission.Quyentruycap ?? false,
                    "Create" => websitePermission.Create ?? false,
                    "Read" => websitePermission.Read ?? false,
                    "Update" => websitePermission.Update ?? false,
                    "Delete" => websitePermission.Delete ?? false,
                    _ => false,
                };
            }
            string websiteQLTKKH = "Quản lí tài khoản khách hàng";
            string websiteQLTKNV = "Quản lí tài khoản nhân viên";
            string websiteKDX = "Kiểm duyệt xe";
            string websiteDKCTXTL = "Đăng kí cho thuê xe tự lái";
            string websiteQTV = "Trang chủ quản trị viên";
            switch (controller)
            {
                case "QLKhachhang":
                    if (action == "DanhsachKH" && !HasPermission(websiteQLTKKH, "Quyentruycap"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "LockAccount" && !HasPermission(websiteQLTKKH, "Update"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "ChiTietKhachHang" && !HasPermission(websiteQLTKKH, "Read"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    break;
                case "QLNhanvien":
                    if (action == "DanhsachNV" && !HasPermission(websiteQLTKNV, "Quyentruycap"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "LockAccount" && !HasPermission(websiteQLTKNV, "Update"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "XoaNhanvien" && !HasPermission(websiteQLTKNV, "Delete"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "ThemNhanVien" && !HasPermission(websiteQLTKNV, "Create"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "ChiTietNhanVien" && !HasPermission(websiteQLTKNV, "Read"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    break;
                case "DuyetXe":
                    if (action == "DanhsachxeCanDuyet" && !HasPermission(websiteKDX, "Quyentruycap"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "Duyet" && !HasPermission(websiteKDX, "Update"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "ChiTietXe" && !HasPermission(websiteKDX, "Read"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    break;
                case "XeKH":
                    if (action == "DanhsachxecuaKH" && !HasPermission(websiteDKCTXTL, "Quyentruycap"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "ThemXe" && !HasPermission(websiteDKCTXTL, "Create"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "SuaXe" && !HasPermission(websiteDKCTXTL, "Update"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "XoaXe" && !HasPermission(websiteDKCTXTL, "Delete"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    break;
                case "TrangchuAdmin":
                    if (action == "Index" && !HasPermission(websiteQTV, "Quyentruycap"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    break;
                case "PhanQuyen":
                    if (action == "PhanQuyenRole" && !HasPermission("Phân quyền tài khoản", "Quyentruycap"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "UpdatePermissions" && !HasPermission("Phân quyền tài khoản", "Update"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "ThemQuyenMoi" && !HasPermission("Phân quyền tài khoản", "Create"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "CapQuyenChoRole" && !HasPermission("Phân quyền tài khoản", "Update"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "ChangeRole" && !HasPermission("Chuyển quyền tài khoản", "Update"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    if (action == "ChangeRole" && !HasPermission("Chuyển quyền tài khoản", "Quyentruycap"))
                    {
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                        return;
                    }
                    break;
                
            }
        }
    }
}