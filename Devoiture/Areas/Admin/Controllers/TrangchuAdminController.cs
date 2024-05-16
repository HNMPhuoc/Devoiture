using Devoiture.Helpers;
using Devoiture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Devoiture.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class TrangchuAdminController : Controller
    {
        private readonly Devoiture1Context _context;
        public TrangchuAdminController(Devoiture1Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/TrangchuAdmin/Index.cshtml");
        }

    }
}
