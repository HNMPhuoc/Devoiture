using AutoMapper;
using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devoiture.Areas.Admin.Controllers
{
    [ManagerAuthorize]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly Devoiture1Context _context;
        public ProfileController(Devoiture1Context context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public IActionResult EditprofileAD()
        {
            var model = new ProfileAD_VM();
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
            var editkh = _context.Taikhoans.FirstOrDefault(p => p.Email == kh);
            if (editkh != null)
            {
                model.Email = editkh.Email;
                model.Sdt = editkh.Sdt;
                model.Username = editkh.Username;
                model.HoTen = editkh.HoTen;
                model.Ngsinh = editkh.Ngsinh;
                model.Gioitinh = editkh.Gioitinh;
                model.HinhDaiDien = editkh.HinhDaiDien;
                model.SoCccd = editkh.SoCccd;
                model.HinhCccd = editkh.HinhCccd;
            }
            return View("~/Areas/Admin/Views/Profile/EditprofileAD.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditprofileAD(ProfileAD_VM model, IFormFile Hinh1, IFormFile Hinh2)
        {
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
            var editkh = _context.Taikhoans.FirstOrDefault(p => p.Email == kh);
            if (editkh != null)
            {
                editkh.Email = model.Email;
                editkh.Sdt = model.Sdt;
                editkh.Username = model.Username;
                editkh.HoTen = model.HoTen;
                editkh.Ngsinh = model.Ngsinh;
                editkh.Gioitinh = model.Gioitinh;
                editkh.SoCccd = model.SoCccd;
                // Upload images if provided
                if (Hinh1 != null)
                {
                    model.HinhDaiDien = MyUtil.UploadHinh("AnhDaiDien", Hinh1);
                    editkh.HinhDaiDien = model.HinhDaiDien;
                }
                if (Hinh2 != null)
                {
                    model.HinhCccd = MyUtil.UploadHinh("AnhCCCD", Hinh2);
                    editkh.HinhCccd = model.HinhCccd;
                }
                _context.Update(editkh);
                _context.SaveChanges();

                // Update session variables if needed
                if (editkh.HinhDaiDien != null)
                {
                    HttpContext.Session.SetString(MySettings.ACCOUNT_AVATAR, editkh.HinhDaiDien);
                }
                if (editkh.Username != null)
                {
                    HttpContext.Session.SetString(MySettings.ACCOUNT_NAME, editkh.Username);
                }
            }
            return View("~/Areas/Admin/Views/TrangchuAdmin/Index.cshtml");
        }
    }
}
