using AutoMapper;
using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devoiture.Areas.Admin.Controllers
{
    [CustomAuthorize]
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
            var model = new ProfileKH_VM();
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
            var editkh = _context.Taikhoans.FirstOrDefault(p => p.Email == kh);
            model.Email = editkh.Email;
            model.Sdt = editkh.Sdt;
            model.Username = editkh.Username;
            model.SoCccd = editkh.SoCccd;
            model.HoTen = editkh.HoTen;
            model.SoGplxA4 = editkh.SoGplxB2;
            model.Ngsinh = editkh.Ngsinh;
            model.Gioitinh = editkh.Gioitinh;
            model.HinhDaiDien = editkh.HinhDaiDien;
            model.HinhCccd = editkh.HinhCccd;
            model.HinhGplx = editkh.HinhGplxb2;
            return View("~/Areas/Admin/Views/Profile/EditprofileAD.cshtml", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditprofileAD(ProfileKH_VM model, IFormFile Hinh1, IFormFile Hinh2, IFormFile Hinh3)
        {
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
            var editkh = _context.Taikhoans.FirstOrDefault(p => p.Email == kh);
            editkh.Email = model.Email;
            editkh.Sdt = model.Sdt;
            editkh.Username = model.Username;
            editkh.SoCccd = model.SoCccd;
            editkh.HoTen = model.HoTen;
            editkh.SoGplxB2 = model.SoGplxA4;
            editkh.Ngsinh = model.Ngsinh;
            editkh.Gioitinh = model.Gioitinh;
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
            if (Hinh3 != null)
            {
                model.HinhGplx = MyUtil.UploadHinh("AnhGPLX", Hinh3);
                editkh.HinhGplxb2 = model.HinhGplx;
            }
            _context.Update(editkh);
            _context.SaveChanges();
            return View("~/Areas/Admin/Views/TrangchuAdmin/Index.cshtml");
        }
    }
}
