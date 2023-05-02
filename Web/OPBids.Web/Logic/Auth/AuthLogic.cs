using OPBids.Web.Logic.Auth.Manager;
using System.Web.Mvc;

namespace OPBids.Web.Logic.Auth
{
    [Authorize]
    public class AuthLogic : Controller
    {
        private CustomUserManager CustomUserManager { get; set; }

        public AuthLogic() : this(new CustomUserManager()) { }

        public AuthLogic(CustomUserManager customUserManager)
        {
            CustomUserManager = customUserManager;
        }
    }
}