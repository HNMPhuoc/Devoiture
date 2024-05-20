using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devoiture.Controllers
{
    [Authorize]
    public class ThuexeController : Controller
    {
        public ThuexeController()
        {

        }
        [Authorize]
        public IActionResult Thue()
        {
            return View();
        }
    }
}
