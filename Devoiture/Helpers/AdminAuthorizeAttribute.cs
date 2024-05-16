using Microsoft.AspNetCore.Mvc;

namespace Devoiture.Helpers
{
    public class AdminAuthorizeAttribute : TypeFilterAttribute
    {
        public AdminAuthorizeAttribute() : base(typeof(AdminAuthorizeFilter))
        {
        }
    }
}
