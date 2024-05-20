using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Devoiture.Areas.Admin.Controllers
{
    [Authorize]
    public class PhanQuyenController : Controller
    {
        private readonly Devoiture1Context _context;
        public PhanQuyenController(Devoiture1Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> PhanQuyenRole()
        {
            var dsQuyen = await _context.Quyens
                .Where(q => q.MaQuyen != 1) // Exclude admin role
                .ToListAsync();

            ViewBag.Quyens = new SelectList(dsQuyen, "MaQuyen", "TenQuyen");

            return View("~/Areas/Admin/Views/PhanQuyen/PhanQuyenRole.cshtml");
        }
        [HttpPost]
        public IActionResult GetQuyenDetails(int maQuyen)
        {
            var quyen = _context.Quyens
                .Where(q => q.MaQuyen == maQuyen)
                .Select(q => new DanhsachQuyen_VM
                {
                    MaQuyen = q.MaQuyen,
                    TenQuyen = q.TenQuyen,
                    Websites = q.Websites.Select(w => new Website_VM
                    {
                        TenWebsite = w.TenWebsite,
                        Quyentruycap = w.Quyentruycap,
                        Create = w.Create,
                        Read = w.Read,
                        Update = w.Update,
                        Delete = w.Delete
                    }).ToList()
                })
                .FirstOrDefault();
            if (quyen != null)
            {
                return PartialView("~/Areas/Admin/Views/PhanQuyen/_QuyenDetails.cshtml", quyen);
            }

            return NotFound();
        }
        [HttpPost]
        public IActionResult CapQuyenChoRole(int maQuyen, Dictionary<string, List<string>> Permissions)
        {
            var websites = _context.Websites.Where(w => w.MaQuyen == maQuyen).ToList();

            foreach (var website in websites)
            {
                // Reset all permissions to false
                website.Quyentruycap = false;
                website.Create = false;
                website.Read = false;
                website.Update = false;
                website.Delete = false;

                // Update based on submitted permissions
                foreach (var permission in Permissions)
                {
                    if (permission.Value.Contains(website.TenWebsite))
                    {
                        switch (permission.Key)
                        {
                            case "Quyentruycap":
                                website.Quyentruycap = true;
                                break;
                            case "Create":
                                website.Create = true;
                                break;
                            case "Read":
                                website.Read = true;
                                break;
                            case "Update":
                                website.Update = true;
                                break;
                            case "Delete":
                                website.Delete = true;
                                break;
                        }
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("PhanQuyenRole", "PhanQuyen", new { area = "Admin", maQuyen = maQuyen });
        }
        [HttpPost]
        public IActionResult ThemQuyenMoi(string TenQuyen)
        {
            if (!string.IsNullOrEmpty(TenQuyen))
            {
                // Thêm quyền mới vào bảng Quyen
                var quyenMoi = new Quyen
                {
                    TenQuyen = TenQuyen
                };

                _context.Quyens.Add(quyenMoi);
                _context.SaveChanges();

                // Tạo danh sách các website mặc định
                var websites = new List<Website>
                {
                    new Website { TenWebsite = "Trang chủ quản trị viên", MaQuyen = quyenMoi.MaQuyen, Quyentruycap = false, Create = false, Read = false, Update = false, Delete = false },
                    new Website { TenWebsite = "Quản lí tài khoản khách hàng", MaQuyen = quyenMoi.MaQuyen, Quyentruycap = false, Create = false, Read = false, Update = false, Delete = false },
                    new Website { TenWebsite = "Kiểm duyệt xe", MaQuyen = quyenMoi.MaQuyen, Quyentruycap = false, Create = false, Read = false, Update = false, Delete = false },
                    new Website { TenWebsite = "Quản lí tài khoản nhân viên", MaQuyen = quyenMoi.MaQuyen, Quyentruycap = false, Create = false, Read = false, Update = false, Delete = false },
                    new Website { TenWebsite = "Phân quyền tài khoản", MaQuyen = quyenMoi.MaQuyen, Quyentruycap = false, Create = false, Read = false, Update = false, Delete = false },
                    new Website { TenWebsite = "Chuyển quyền tài khoản", MaQuyen = quyenMoi.MaQuyen, Quyentruycap = false, Create = false, Read = false, Update = false, Delete = false },
                    new Website { TenWebsite = "Đăng kí cho thuê xe tự lái", MaQuyen = quyenMoi.MaQuyen, Quyentruycap = false, Create = false, Read = false, Update = false, Delete = false },
                    new Website { TenWebsite = "Thuê xe tự lái", MaQuyen = quyenMoi.MaQuyen, Quyentruycap = false, Create = false, Read = false, Update = false, Delete = false },
                    new Website { TenWebsite = "Đổi tên quyền", MaQuyen = quyenMoi.MaQuyen, Quyentruycap = false, Create = false, Read = false, Update = false, Delete = false }
                };
                _context.Websites.AddRange(websites);
                _context.SaveChanges();

                return RedirectToAction(nameof(PhanQuyenRole));
            }

            return RedirectToAction(nameof(PhanQuyenRole));
        }
        public async Task<IActionResult> ChangeRole()
        {
            var usersWithRoles = await _context.Taikhoans
                .Where(tk => tk.IdQuyen == 2)
                .Include(u => u.IdQuyenNavigation)
                .Select(u => new DanhSachTKNVQ_VM
                {
                    Email = u.Email,
                    HoTen = u.HoTen,
                    HinhDaiDien = u.HinhDaiDien,
                    TenQuyen = u.IdQuyenNavigation.TenQuyen,
                    IdQuyen = u.IdQuyen,
                    Online = u.Online
                })
                .ToListAsync();
            var roles = await _context.Quyens.Where(r => r.MaQuyen != 1).ToListAsync();

            ViewBag.Roles = roles;

            return View("~/Areas/Admin/Views/PhanQuyen/ChangeRole.cshtml", usersWithRoles);
        }
        [HttpPost]
        public IActionResult UpdatePermissions(int maQuyen)
        {
            var websites = _context.Websites.Where(w => w.MaQuyen == maQuyen).ToList();
            foreach (var website in websites)
            {
                website.Quyentruycap = false;
                website.Create = false;
                website.Read = false;
                website.Update = false;
                website.Delete = false;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(PhanQuyenRole));
        }
        public async Task<IActionResult> ChangeNameRole()
        {
            var danhSachQuyen = await _context.Quyens
                .Where(q => q.MaQuyen != 1) // Exclude admin role
                .Select(q => new QuyenViewModel
                {
                    MaQuyen = q.MaQuyen,
                    TenQuyen = q.TenQuyen
                })
                .ToListAsync();

            var viewModel = new ChangeNameRole_VM
            {
                DanhSachQuyen = danhSachQuyen
            };

            return View("~/Areas/Admin/Views/PhanQuyen/ChangeNameRole.cshtml", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeNameRole(ChangeNameRole_VM viewModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var quyen in viewModel.DanhSachQuyen)
                {
                    var quyenDb = await _context.Quyens.FindAsync(quyen.MaQuyen);
                    if (quyenDb != null)
                    {
                        quyenDb.TenQuyen = quyen.TenQuyen;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PhanQuyenRole));
            }

            // If ModelState is not valid, reload the list of roles to ensure the dropdown is populated
            viewModel.DanhSachQuyen = await _context.Quyens
                .Where(q => q.MaQuyen != 1)
                .Select(q => new QuyenViewModel
                {
                    MaQuyen = q.MaQuyen,
                    TenQuyen = q.TenQuyen
                })
                .ToListAsync();

            return View("~/Areas/Admin/Views/PhanQuyen/ChangeNameRole.cshtml", viewModel);
        }
    }
}
