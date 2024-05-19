using Microsoft.AspNetCore.Mvc;

namespace Devoiture.Helpers
{
    public class ManagerAuthorizeAttribute : TypeFilterAttribute
    {
        public ManagerAuthorizeAttribute() : base(typeof(ManagerAuthorizeFilter))
        {
        }
    }
}
