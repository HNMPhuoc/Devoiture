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
    public class DuyetXeController : Controller
    {
        private readonly Devoiture1Context _context;
        public DuyetXeController(Devoiture1Context context)
        {
            _context = context;
        }
        public IActionResult DanhsachxeCanDuyet()
        {
           
            var dsxe = _context.Xes
                        .Join(_context.MauXes,
                            x => x.MaMx,
                            m => m.MaMx,
                            (x, m) => new { Xe = x, MauXe = m })
                        .Where(xm => xm.Xe.Hide || !xm.Xe.TrangthaiDuyet)
                        .Select(xm => new DanhsachxeKH_VM
                        {
                            Email = xm.Xe.Email,
                            Biensoxe = xm.Xe.Biensoxe,
                            TenXe = xm.MauXe.TenMx,
                            NamSx = xm.Xe.NamSx,
                            Giathue = xm.Xe.Giathue,
                            Muctieuthunhienlieu = xm.Xe.Muctieuthunhienlieu,
                            Hinhanh = xm.Xe.Hinhanh
                        })
                        .OrderBy(p => p.TenXe)
                        .ToList();
            return View("~/Areas/Admin/Views/DuyetXe/DanhsachxeCanDuyet.cshtml", dsxe);
        }
        [HttpPost]
        public IActionResult Duyet(string xeId)
        {
            var xe = _context.Xes.FirstOrDefault(x => x.Biensoxe == xeId);
            if (xe != null)
            {
                xe.TrangthaiDuyet = true;
                xe.Hide = false;
                _context.SaveChanges();
                return RedirectToAction("DanhsachxeCanDuyet");
            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> ChiTietXe(string bienSoXe)
        {
            var xe = await _context.Xes
                .Include(x => x.MaMxNavigation)
                .Include(x => x.MaLoaiNavigation)
                .Include(x => x.MakvNavigation)
                .FirstOrDefaultAsync(x => x.Biensoxe == bienSoXe);
            if (xe == null)
            {
                return NotFound();
            }
            var viewModel = new Chitietxekh_VM
            {
                Email = xe.Email,
                Biensoxe = xe.Biensoxe,
                tenLoai = xe.MaLoaiNavigation.TenLoai,
                tenMx = xe.MaMxNavigation.TenMx,
                Giathue = xe.Giathue,
                NamSx = xe.NamSx,
                Soghe = xe.Soghe,
                Muctieuthunhienlieu = xe.Muctieuthunhienlieu,
                Diachixe = xe.Diachixe,
                Giaoxetannoi = xe.Giaoxetannoi,
                Dieukhoanthuexe = xe.Dieukhoanthuexe,
                MotaDacDiemChucNang = xe.MotaDacDiemChucNang,
                Loainhienlieu = xe.Loainhienlieu,
                Hinhanh = xe.Hinhanh,
                Trangthaibaotri = xe.Trangthaibaotri,
                tenkv = xe.MakvNavigation.TenKv
            };
            return View("~/Areas/Admin/Views/DuyetXe/Chitietxe.cshtml", viewModel);
        }
    }
}
