using Microsoft.AspNetCore.Mvc;

namespace Devoiture.Helpers
{
    public class CustomerAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomerAuthorizeAttribute() : base(typeof(CustomerAuthorizeFilter))
        {
        }
    }
}
