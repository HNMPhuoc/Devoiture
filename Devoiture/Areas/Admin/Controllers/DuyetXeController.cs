using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devoiture.Areas.Admin.Controllers
{
    [CustomAuthorize]
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
            // Tìm xe trong cơ sở dữ liệu
            var xe = _context.Xes.FirstOrDefault(x => x.Biensoxe == xeId);
            if (xe != null)
            {
                // Cập nhật trạng thái và ẩn
                xe.TrangthaiDuyet = true;
                xe.Hide = false;
                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
                return Ok(); // Trả về mã HTTP 200 OK
            }
            else
            {
                return NotFound(); // Trả về mã HTTP 404 Not Found nếu không tìm thấy xe
            }
        }
    }
}
