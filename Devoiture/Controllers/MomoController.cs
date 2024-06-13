using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.Models.Momo;
using Devoiture.Models.Order;
using Devoiture.Service;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Devoiture.Controllers
{
    public class MomoController : Controller
    {
        private readonly IMomoService _momoService;
        private readonly Devoiture1Context _context;

        public MomoController(IMomoService momoService, Devoiture1Context context)
        {
            _momoService = momoService;
            _context = context;
        }
        public IActionResult CreatePaymentUrl(int mayc)
        {
            var chitietyc = (from yc in _context.Yeucauthuexes
                             join xe in _context.Xes on yc.Biensoxe equals xe.Biensoxe
                             join chuxe in _context.Taikhoans on yc.Nguoithue equals chuxe.Email
                             where yc.MaYc == mayc
                             select new ChitietycThanhtoan_VM
                             {
                                 MaYc = yc.MaYc,
                                 TenMauXe = xe.MaMxNavigation.TenMx,
                                 Chuxe = chuxe.Email,
                                 BienSoXe = yc.Biensoxe,
                                 TongTienThue = yc.Tongtienthue,
                                 HoTen = chuxe.HoTen,
                                 Sdt = chuxe.Sdt,
                                 Baohiemthuexe = yc.Baohiemthuexe
                             }).FirstOrDefault();

            if (chitietyc == null)
            {
                return NotFound();
            }

            var momo = new MomoExecuteResponseModel
            {
                FullName = chitietyc.HoTen,
                Mayc = mayc,
                OrderInfo = $"{chitietyc.HoTen} thanh toán tiền cọc cho xe {chitietyc.BienSoXe}",
                Amount = (int)(chitietyc.TongTienThue * 0.3)
            };
            return View("CreatePaymentUrl", momo);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentAsync(model);
            return Redirect(response.PayUrl);
        }
        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var orderInfo = _context.Yeucauthuexes.FirstOrDefault(yc => yc.MaYc == response.Mayc);
            if (orderInfo != null)
            {
                var khachhang = _context.Taikhoans.FirstOrDefault(t => t.Email == orderInfo.Nguoithue);
                var chuxe = _context.Taikhoans.FirstOrDefault(t => t.Email == orderInfo.Chuxe);
                if (khachhang != null)
                {
                    var hoadonThuexe = new HoadonThuexe
                    {
                        MaHd = response.OrderId,
                        MaYc = orderInfo.MaYc,
                        Email = orderInfo.Nguoithue,
                        Biensoxe = orderInfo.Biensoxe,
                        NgaylapHd = DateTime.Now,
                        HoTen = response.FullName,
                        Sdt = khachhang.Sdt,
                        Tiendatcoc = Math.Round(orderInfo.Tongtienthue * 0.3, 0),
                        Baohiemthuexe = Math.Round(orderInfo.Baohiemthuexe, 0),
                        TongTienThue = Math.Round(orderInfo.Tongtienthue, 0),
                        Sotiencantra = Math.Round(orderInfo.Tongtienthue, 0) - response.Amount,
                    };
                    var hoadonchothuexe = new HoaDonChoThueXe
                    {
                        MaHdct = response.OrderId,
                        MaYc = orderInfo.MaYc,
                        Email = orderInfo.Chuxe,
                        Biensx = orderInfo.Biensoxe,
                        NglapHd = DateTime.Now,
                        Hoten = chuxe.HoTen,
                        Sdt = khachhang.Sdt,
                        Tongtiennhanduoc = response.Amount
                    };
                    _context.HoadonThuexes.Add(hoadonThuexe);
                    _context.HoaDonChoThueXes.Add(hoadonchothuexe);
                    orderInfo.Matt = 4;
                    orderInfo.Sotiencantra = Math.Round(orderInfo.Tongtienthue, 0) - response.Amount;
                    _context.Yeucauthuexes.Update(orderInfo);
                    _context.SaveChanges();
                    TempData["success"] = "Đã thanh toán tiền cọc thành công. Quý khách vui lòng thanh toán số tiền còn lại với chủ xe";
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}