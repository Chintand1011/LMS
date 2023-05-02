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
    public class AuthController : Controller
    {
        #region Authentication Objects
        private CustomUserManager CustomUserManager { get; set; }
        public AuthController()
            : this(new CustomUserManager()) { }

        public AuthController(CustomUserManager customUserManager)
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
        #endregion

        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Auth");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVM model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await CustomUserManager.FindAsync(model.user_name, model.password);
                if (user != null)
                {
                    //TODO: await SignInAsync(user, model.persist);
                    await SignInAsync(user, true);

                    // Set User Menu in Session

                    Session["user"] = user;

                    Logic.ActivityLog.ActivityLogHelper.InsertActivityLog(ActivityLogModule.ITMS, ActivityLogType.Login, "Success", "Login", user);

                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    Session["user"] = null;

                    ModelState.AddModelError("", "Invalid username or password.");

                    Logic.ActivityLog.ActivityLogHelper.InsertActivityLogAttempFailed(ActivityLogModule.ITMS, ActivityLogType.Login, string.Format("Login attempt ({0})", model.user_name), "Login", model.user_name);

                }
            }

            // If we got this far, something failed, redisplay form

            //hit the activity log
            

            return View("Login");
        }
        
        [AllowAnonymous]
        public async Task<ActionResult> ChangeProduct(string productId)
        {
            var user = new AuthUser();
            var ctxt = Request.GetOwinContext();
            user.dashboard_id = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.DashboardId).ToSafeInt();
            user.department_id = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.DeptId).ToSafeString();
            user.department_name = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.Department).ToSafeString();
            user.dts_access = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.DtsAccess).ToSafeBool();
            user.email = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.Email).ToSafeString();
            user.GroupId = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.GroupId).ToSafeString();
            user.full_name = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.FullName).ToSafeString();
            user.UserName = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.UserName).ToSafeString();
            user.Id = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.UserId).ToSafeString();
            user.pmfs_access = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.PfmsAccess).ToSafeBool();
            user.dts_access = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.DtsAccess).ToSafeBool();
            user.department_code = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.DeptCode).ToSafeString();
            user.vip = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.VIP).ToSafeBool();
            await SignInAsync(user, true, productId);
            if (productId == "2")
            {
                return Redirect("../DTS/Index");
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            ChangePwdVM change = new ChangePwdVM() { act_code = Request.QueryString["actcode"] };
            return View(change);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePwdVM model)
        {
            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                
                model.id = OPBids.Web.Helper.UserInfoStore.GetUserId(Request.GetOwinContext()); ;
                var user = await CustomUserManager.ChangePassword(model.act_code, model.new_pwd,model.id);
                if (user == null)
                {
                    Logic.ActivityLog.ActivityLogHelper.InsertActivityLog(ActivityLogModule.ITMS, ActivityLogType.Login, "Password Changed", "Failed");

                    ModelState.AddModelError("", "Invalid Reset Code.");
                }
                else
                {
                    Logic.ActivityLog.ActivityLogHelper.InsertActivityLog(ActivityLogModule.ITMS, ActivityLogType.Login, "Password Changed", "Success");

                    ViewBag.Success = true;
                }                
            }


            

            return View("ChangePassword");
        }

        //public async Task<ActionResult> ChangePassword2(ChangePwdVM model)
        //{
        //    ViewBag.Success = false;

        //    if (ModelState.IsValid)
        //    {

        //        model.id = OPBids.Web.Helper.UserInfoStore.GetUserId(Request.GetOwinContext()); ;
        //        var user = await CustomUserManager.ChangePassword2(model.id, model.new_pwd);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError("", "Invalid Reset Code.");
        //        }
        //        else
        //        {
        //            ViewBag.Success = true;
        //        }
        //    }
        //    return View("ChangePassword");
        //}


        //[HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Logic.ActivityLog.ActivityLogHelper.InsertActivityLog(ActivityLogModule.ITMS, ActivityLogType.SignOut, "User Sign-out", "Success");

            AuthenticationManager.SignOut();

            return RedirectToAction("Login", "Auth");
        }


        private async Task SignInAsync(AuthUser user, bool isPersistent, string productId = null)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var claims = new List<Claim>();
            claims.Add(new Claim(Constant.Auth.Claims.UserId, user.Id));
            claims.Add(new Claim(Constant.Auth.Claims.FullName, user.full_name));
            claims.Add(new Claim(Constant.Auth.Claims.UserName, user.UserName));
            claims.Add(new Claim(Constant.Auth.Claims.GroupId, user.GroupId));
            claims.Add(new Claim(Constant.Auth.Claims.Email, user.email));
            claims.Add(new Claim(Constant.Auth.Claims.DeptId, user.department_id));
            claims.Add(new Claim(Constant.Auth.Claims.DashboardId, user.dashboard_id.ToString()));
            claims.Add(new Claim(Constant.Auth.Claims.Department, user.department_name.ToSafeString()));
            claims.Add(new Claim(Constant.Auth.Claims.DeptCode, user.department_code.ToSafeString()));
            claims.Add(new Claim(Constant.Auth.Claims.PfmsAccess, user.pmfs_access.ToSafeString()));
            claims.Add(new Claim(Constant.Auth.Claims.DtsAccess, user.dts_access.ToSafeString()));
            claims.Add(new Claim(Constant.Auth.Claims.VIP, user.vip.ToString()));
            claims.Add(new Claim(Constant.Auth.Claims.CurrentProduct, (productId == null ? (user.pmfs_access == true ? "1" : (user.dts_access == true ? "2" : "0")): productId)));
            var identity = await CustomUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaims(claims);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

    }
}