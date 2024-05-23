using Devoiture.Models;
using AutoMapper;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Devoiture.Helpers;
using System.Data.SqlTypes;

namespace Devoiture.Controllers
{
    public class KhachhangController : Controller
    {
        private readonly Devoiture1Context _context;
        private readonly IMapper _mapper;
        public KhachhangController(Devoiture1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dangky(DangkyKH_VM model)
        {
            if (ModelState.IsValid)
            {
                var kh = _mapper.Map<Taikhoan>(model);
                kh.Email = model.Email;
                kh.Matkhau = BCrypt.Net.BCrypt.HashPassword(model.Matkhau);
                kh.Username = model.Username;
                kh.Sdt = model.Sdt;
                kh.Ngsinh = model.Ngsinh;
                kh.Gioitinh = model.Gioitinh;
                kh.Online = false;
                kh.Lock = false;
                kh.IdQuyen = 3;
                var khach = _context.Quyens.FirstOrDefault(p => p.MaQuyen == kh.IdQuyen);
                if (khach != null)
                {
                    kh.IdQuyenNavigation = khach;
                }                
                _context.Taikhoans.Add(kh);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        public IActionResult Dangnhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dangnhap(DangnhapKH_VM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var kh = await _context.Taikhoans
                        .Include(tk => tk.IdQuyenNavigation)
                        .SingleOrDefaultAsync(p => p.Email.ToLower() == model.Email.ToLower());
                if (kh == null)
                {
                    ModelState.AddModelError("loi", "Không tồn tại email này");
                }
                else
                {
                    if (kh.Lock == true)
                    {
                        ModelState.AddModelError("loi", "Tài khoản này bị khóa. Vui lòng liên hệ admin");
                        return View("LockAcc");
                    }
                    else
                    {
                        if (!BCrypt.Net.BCrypt.Verify(model.Matkhau, kh.Matkhau))
                        {
                            ModelState.AddModelError("loi", "Tài khoản hoặc mật khẩu. Vui lòng kiểm tra lại");
                        }
                        else
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, kh.Email),
                                new Claim(ClaimTypes.Name,kh.Username),
                                new Claim("AccountID",kh.Email),
                                new Claim(ClaimTypes.Role,kh.IdQuyen.ToString())
                            };
                            var claimIdentity = new ClaimsIdentity(claims, "Dangnhap");
                            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                            HttpContext.Session.SetString(MySettings.ACCOUNT_KEY, kh.Email);
                            HttpContext.Session.SetString(MySettings.ACCOUNT_NAME, kh.Username);
                            HttpContext.Session.SetInt32(MySettings.ACCOUNT_ROLE, kh.IdQuyen);
                            if (kh.IdQuyenNavigation != null)
                            {
                                HttpContext.Session.SetString(MySettings.ACCOUNT_ROLENAME, kh.IdQuyenNavigation.TenQuyen);
                            }
                            if (kh.HinhDaiDien != null)
                            {
                                HttpContext.Session.SetString(MySettings.ACCOUNT_AVATAR, kh.HinhDaiDien);
                            }                                
                            await HttpContext.SignInAsync(claimPrincipal);
                            kh.Online = true;
                            _context.Taikhoans.Update(kh);
                            _context.SaveChanges();
                            if (kh.IdQuyen == 1 || kh.IdQuyen == 2)
                            {
                                return RedirectToAction("Index", "TrangchuAdmin", new { area = "Admin" });
                            }
                            else if (kh.IdQuyen == 3 && Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Dangxuat()
        {
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
            var logoutkh = _context.Taikhoans.FirstOrDefault(a => a.Email == kh);
            await HttpContext.SignOutAsync();
            logoutkh.Online = false;
            _context.Taikhoans.Update(logoutkh);
            _context.SaveChanges();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ProfileKH()
        {
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
            var detailkh = _context.Taikhoans.FirstOrDefault(a => a.Email == kh);
            ViewBag.Email = detailkh.Email;
            ViewBag.Username = detailkh.Username;
            ViewBag.HoTen = detailkh.HoTen;
            ViewBag.Sdt = detailkh.Sdt;
            ViewBag.Ngsinh = detailkh.Ngsinh;
            //Hinh dai dien
            if (detailkh.HinhDaiDien == null)
            {
                ViewBag.HinhDD = "Chưa cập nhật thông tin";
                ViewBag.HinhDD1 = "userdefault.png";
            }
            else
            {
                ViewBag.HinhDD = detailkh.HinhDaiDien;
                ViewBag.HinhDD1 = detailkh.HinhDaiDien;
            }
            // Gioi tinh
            if (detailkh.Gioitinh == true)
            {
                ViewBag.Gioitinh = "Nam";
            }
            else
            {
                ViewBag.Gioitinh = "Nữ";
            }
            // So giay phep lai xe
            if (detailkh.SoGplxB2 == null)
            {
                ViewBag.SoGPLX = "Chưa cập nhật thông tin";
            }
            else
            {
                ViewBag.SoGPLX = detailkh.SoGplxB2;
            }
            // Hinh giay phep lai xe
            if (detailkh.HinhGplxb2 == null)
            {
                ViewBag.HinhGPLX = "Chưa cập nhật thông tin";
                ViewBag.HinhGPLX1 = "gplxDefault.png";
            }
            else
            {
                ViewBag.HinhGPLX = detailkh.HinhGplxb2;
                ViewBag.HinhGPLX1 = detailkh.HinhGplxb2;
            }
            // So can cuoc cong dan
            if (detailkh.SoCccd == null)
            {
                ViewBag.SoCCCD = "Chưa cập nhật thông tin";
            }
            else
            {
                ViewBag.SoCCCD = detailkh.SoCccd;
            }
            // Hinh can cuoc cong dan
            if (detailkh.HinhCccd == null)
            {
                ViewBag.HinhCCCD = "Chưa cập nhật thông tin";
                ViewBag.HinhCCCD1 = "cccdDefault.png";
            }
            else
            {
                ViewBag.HinhCCCD = detailkh.HinhCccd;
                ViewBag.HinhCCCD1 = detailkh.HinhCccd;
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult Editprofile()
        {
            var model = new ProfileKH_VM();
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
            var editkh = _context.Taikhoans.FirstOrDefault(p => p.Email == kh);
            model.Email = editkh.Email;
            model.Sdt = editkh.Sdt;
            model.Username = editkh.Username;
            model.SoCccd = editkh.SoCccd;
            model.HoTen = editkh.HoTen;
            model.SoGplxB2 = editkh.SoGplxB2;
            model.Ngsinh = editkh.Ngsinh;
            model.Gioitinh = editkh.Gioitinh;
            model.HinhDaiDien = editkh.HinhDaiDien;
            model.HinhCccd = editkh.HinhCccd;
            model.HinhGplx = editkh.HinhGplxb2;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editprofile(ProfileKH_VM model, IFormFile Hinh1, IFormFile Hinh2, IFormFile Hinh3)
        {
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
            var editkh = _context.Taikhoans.FirstOrDefault(p => p.Email == kh); 
            if (model.Username != null)
            {
                editkh.Username = model.Username;
            }
            if(model.SoCccd != null)
            {
                editkh.SoCccd = model.SoCccd;
            }
            if (model.Sdt != null)
            {
                editkh.Sdt = model.Sdt;
            }
            if (model.HoTen != null)
            {
                editkh.HoTen = model.HoTen;
            }
            if (model.SoGplxB2 != null)
            {
                editkh.SoGplxB2 = model.SoGplxB2;
            }
            if (model.Gioitinh != null)
            {
                editkh.Gioitinh = model.Gioitinh;
            }

            if (model.Ngsinh != null && IsDateTimeValid(model.Ngsinh))
            {
                editkh.Ngsinh = model.Ngsinh;
            }
            else
            {
                ModelState.AddModelError("Ngsinh", "Ngày sinh không hợp lệ");
            }

            // Cập nhật hình ảnh nếu có
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

            // Cập nhật thông tin trong session nếu cần
            if (editkh.HinhDaiDien != null)
            {
                HttpContext.Session.SetString(MySettings.ACCOUNT_AVATAR, editkh.HinhDaiDien);
            }
            if (editkh.Username != null)
            {
                HttpContext.Session.SetString(MySettings.ACCOUNT_NAME, editkh.Username);
            }

            return RedirectToAction("Index", "Home");
        }

        // Phương thức kiểm tra ngày hợp lệ
        private bool IsDateTimeValid(DateTime dateTime)
        {
            return dateTime >= SqlDateTime.MinValue.Value && dateTime <= SqlDateTime.MaxValue.Value;
        }
    }
}