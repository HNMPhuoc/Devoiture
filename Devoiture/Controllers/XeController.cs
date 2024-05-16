using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devoiture.Controllers
{
    public class XeController : Controller
    {
        private readonly Devoiture1Context _context;
        public XeController(Devoiture1Context context) 
        { 
            _context = context;
        }
        public IActionResult Danhsachxe()
        {
            var dsxe = _context.Xes
                .Where(p => p.Hide == false && p.TrangthaiDuyet == true && p.Trangthaibaotri == false)
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
    }
}
