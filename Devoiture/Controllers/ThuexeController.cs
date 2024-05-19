using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devoiture.Controllers
{
    public class ThuexeController : Controller
    {
        [Authorize]
        public IActionResult Thue()
        {
            return View();
        }
    }
}
