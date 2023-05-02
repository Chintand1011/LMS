using System.Web.Mvc;

namespace OPBids.Web.Controllers
{
    public class ForgotPasswordController : Controller
    {

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}