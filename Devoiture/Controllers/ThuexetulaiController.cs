using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var viewModel = new Thuexe_VM
            {
                CarModel = xe.MaMxNavigation.TenMx, // Assuming 'Model' is a property of MaMxNavigation
                Giathue = xe.Giathue,
                NamSx = xe.NamSx,
                Soghe = xe.Soghe,
                Muctieuthunhienlieu = xe.Muctieuthunhienlieu,
                Hinhanh = xe.Hinhanh,
                Diachixe = xe.Diachixe,
                Dieukhoanthuexe = xe.Dieukhoanthuexe,
                MotaDacDiemChucNang = xe.MotaDacDiemChucNang,
                Loainhienlieu = xe.Loainhienlieu,
                HinhAnhList = xe.HinhAnhXes.OrderByDescending(p => p.MaAnh).Select(p => p.Hinh).Take(4).ToList()
            };
            return View(viewModel);
        }
    }
}