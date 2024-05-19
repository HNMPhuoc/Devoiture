using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Devoiture.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Devoiture1Context _context;

        public HomeController(ILogger<HomeController> logger, Devoiture1Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var dsxe = _context.Xes
                .Where(p => p.Hide == false && p.TrangthaiDuyet == true)
                .Select(x => new DanhsachxeKH_VM
                {
                    Biensoxe = x.Biensoxe,
                    TenXe = x.MaMxNavigation.TenMx,
                    NamSx = x.NamSx,
                    Tenloai = x.MaLoaiNavigation.TenLoai,
                    Muctieuthunhienlieu = x.Muctieuthunhienlieu,
                    Hinhanh = x.Hinhanh,
                    Giathue = x.Giathue,
                    Trangthaibaotri = x.Trangthaibaotri,
                    TrangthaiDuyet = x.TrangthaiDuyet,
                    Hide = x.Hide
                })
                .OrderBy(p => p.TenXe)
                .ToList();
            return View(dsxe);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult AboutContent()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
