using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using System;
using System.Linq;

namespace Devoiture.Controllers
{
    [Authorize]
    public class ThuexetulaiController : Controller
    {
        private readonly Devoiture1Context _context;

        public ThuexetulaiController(Devoiture1Context context)
        {
            _context = context;
            //_env = env;
            //_emailConfig = emailConfig;
            //_iemailSender = iemailSender;
        }
        public IActionResult Thuexe(string biensoxe)
        {
            if (string.IsNullOrEmpty(biensoxe))
            {
                return BadRequest("Biensoxe is required");
            }
            var xe = _context.Xes
                .Include(x => x.HinhAnhXes)
                .Include(x => x.MaMxNavigation)
                .FirstOrDefault(x => x.Biensoxe == biensoxe);
            if (xe == null)
            {
                return NotFound("Xe not found");
            }
            var hinhAnhList = xe.HinhAnhXes?.OrderByDescending(p => p.MaAnh).Select(p => p.Hinh).Take(4).ToList() ?? new List<string>();
            var viewModel = new Thuexe_VM
            {
                CarModel = xe.MaMxNavigation.TenMx,
                Giathue = xe.Giathue,
                NamSx = xe.NamSx,
                Soghe = xe.Soghe,
                Muctieuthunhienlieu = xe.Muctieuthunhienlieu,
                Hinhanh = xe.Hinhanh,
                Diachixe = xe.Diachixe,
                Dieukhoanthuexe = xe.Dieukhoanthuexe,
                MotaDacDiemChucNang = xe.MotaDacDiemChucNang,
                Loainhienlieu = xe.Loainhienlieu,
                HinhAnhList = hinhAnhList,
                HinhThucThanhToanList = _context.Hinhthucthanhtoans.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Thuexe(Thuexe_VM model, string biensoxe)
        {
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;                           
            var xe = _context.Xes
                .Include(x => x.HinhAnhXes)
                .Include(x => x.MaMxNavigation)
                .FirstOrDefault(x => x.Biensoxe == model.Biensoxe);
            var hinhAnhList = xe.HinhAnhXes?.OrderByDescending(p => p.MaAnh).Select(p => p.Hinh).Take(4).ToList() ?? new List<string>();
            var viewModel = new Thuexe_VM
            {
                CarModel = xe.MaMxNavigation.TenMx,
                Giathue = xe.Giathue,
                NamSx = xe.NamSx,
                Soghe = xe.Soghe,
                Muctieuthunhienlieu = xe.Muctieuthunhienlieu,
                Hinhanh = xe.Hinhanh,
                Diachixe = xe.Diachixe,
                Dieukhoanthuexe = xe.Dieukhoanthuexe,
                MotaDacDiemChucNang = xe.MotaDacDiemChucNang,
                Loainhienlieu = xe.Loainhienlieu,
                HinhAnhList = hinhAnhList,
                HinhThucThanhToanList = _context.Hinhthucthanhtoans.ToList()
            };
            if (model == null)
            {
                return View(viewModel);
            }
            if (xe.Email == kh)
            {
                ModelState.AddModelError(string.Empty, "Không thể thuê xe của chính mình.");
                return View(viewModel);
            }
            var ngaythue = _context.Yeucauthuexes
                .Where(y => y.Biensoxe == biensoxe && y.Matt == 2)
                .Where(y => (model.Ngaynhanxe >= y.Ngaynhanxe && model.Ngaynhanxe <= y.Ngaytraxe) ||
                            (model.Ngaytraxe >= y.Ngaynhanxe && model.Ngaytraxe <= y.Ngaytraxe))
                .ToList();

            if (ngaythue.Any())
            {
                TempData["Error"] = "Xe đã được thuê trong khoảng thời gian này.";
                return View(viewModel);
            }
            var owner = _context.Taikhoans.SingleOrDefault(u => u.Email == xe.Email);
            if (owner != null && owner.Lock == true)
            {
                ModelState.AddModelError(string.Empty, "Chủ xe này hiện đang không thể cho thuê.");
                return View(viewModel);
            }
            if(owner != null && owner.HinhCccd == null && owner.SoCccd == null && owner.HinhGplxb2 == null && owner.SoGplxB2 == null)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng cung cấp thông tin CCCD và GPLX trong profile của bạn.");
                return View(viewModel);
            }
            var yeucauthuexe = new Yeucauthuexe
            {
                Biensoxe = xe.Biensoxe,
                Email = kh,
                Maht = model.Maht,
                Matt = 1,
                Diadiemnhanxe = model.diachinhanxe,
                Ngaynhanxe = model.Ngaynhanxe,
                Ngaytraxe = model.Ngaytraxe,
                Songaythue = model.Songaythue,
                Baohiemthuexe = model.Baohiemthuexe,
                Dongiathue = model.Dongiathue,
                Tongtienthue = model.Tongtienthue,
                Chapnhan = false // initial status
            };
            _context.Yeucauthuexes.Add(yeucauthuexe);
            _context.SaveChanges();
            //var htmlPath = Path.Combine(_env.WebRootPath, "SuccessNotification.html");
            //var messageContent = System.IO.File.ReadAllText(htmlPath);
            //var message = new Message(new string[] { kh.ToString() },_emailConfig.DisplayName, messageContent);
            //_emailSender.SendEmail(message);
            return RedirectToAction("Index","Home");
        }

    }
}