using System.Web.Mvc;
using System.Threading.Tasks;
using OPBids.Web.Logic.Auth.Manager;
using OPBids.Entities.View.Auth;
using OPBids.Web.Models;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Collections.Generic;
using OPBids.Common;
using OPBids.Web.Helper;
using OPBids.Entities.View.Shared;
using OPBids.Web.Logic.ActivityLog;

namespace OPBids.Web.Controllers
{
    [Authorize]
    public class ChangePasswordController : Controller
    {
        private CustomUserManager CustomUserManager { get; set; }
        public ChangePasswordController()
            : this(new CustomUserManager()) { }

        public ChangePasswordController(CustomUserManager customUserManager)
        {
            CustomUserManager = customUserManager;
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        // GET: ChangePassword
        public ActionResult Index()
        {
            return View();
        }


        //TODO: for PM verification
        //[AllowAnonymous] // should not be allowed anonymouse users under Change Password Controller
        //After user changing password, all the session using the same user in other PC or devices should be logout. Otherwise they can change the password to new Passwords since there is no function to validate the current password.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePwdVM model)
        {
            ViewBag.Success = false;

            if (ModelState.IsValid)
            {

                model.id = OPBids.Web.Helper.UserInfoStore.GetUserId(Request.GetOwinContext()); ;
                var user = await CustomUserManager.ChangePassword(model.act_code, model.new_pwd, model.id);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid Reset Code.");
                }
                else
                {
                    Logic.ActivityLog.ActivityLogHelper.InsertActivityLog(ActivityLogModule.ITMS, ActivityLogType.Login, "Password Changed", "Success");

                    //this should logout after redirecting to same page
                    AuthenticationManager.SignOut();

                    ViewBag.Success = true;
                }
            }
            return RedirectToAction("ChangePassword", "User");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswordAjax(ChangePwdVM model)
        {
            //ViewBag.Success = false;

            if (ModelState.IsValid)
            {

                model.id = OPBids.Web.Helper.UserInfoStore.GetUserId(Request.GetOwinContext()); ;
                var user = await CustomUserManager.ChangePassword(model.act_code, model.new_pwd, model.id);
                if (user == null)
                {
                    return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Logic.ActivityLog.ActivityLogHelper.InsertActivityLog(ActivityLogModule.ITMS, ActivityLogType.Login, "Password Changed", "Success");

                    //this should logout after redirecting to same page
                    AuthenticationManager.SignOut();

                    //ViewBag.Success = true;


                    return Json(new { isSuccess = true }, JsonRequestBehavior.AllowGet);

                }
            }
            //return RedirectToAction("ChangePassword", "User");

            return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);

        }

    }
}