using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Entities.View.Shared;
//using OPBids.Entities.View.User;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using OPBids.Entities.Base;
using OPBids.Entities.Common;
using OPBids.Common;
using OPBids.Web.Helper;
using OPBids.Web.Logic.Shared;
using OPBids.Entities.View.Setting;
using OPBids.Web.Logic.ActivityLog;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace OPBids.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            //List<UserMainVM> userMainVMList = new List<UserMainVM>();

            return View();// userMainVMList);
        }

        //public string Search(UserFilterVM filterClass)
        //{
        //    List<UserMainVM> userMainVMList = new List<UserMainVM>();
        //    userMainVMList.Add(new UserMainVM() {
        //        user = this.userList().Where(i => i.id == 1).FirstOrDefault(),
        //        userDetail = this.userDetailList().Where(i => i.user_id == 1).FirstOrDefault(),
        //        userCredential = this.userCredentials().Where(i => i.user_id == 1).FirstOrDefault()
        //    });
            
        //    return JsonConvert.SerializeObject(userMainVMList);
        //}

        //public ActionResult RenderSearch(UserFilterVM filter) {
        //    List<UserMainVM> userMainVMList = new List<UserMainVM>();
        //    userMainVMList.Add(new UserMainVM()
        //    {
        //        user = this.userList().Where(i => i.id == 1).FirstOrDefault(),
        //        userDetail = this.userDetailList().Where(i => i.user_id == 1).FirstOrDefault(),
        //        userCredential = this.userCredentials().Where(i => i.user_id == 1).FirstOrDefault()
        //    });
        //    return PartialView("Partial/SearchResult", userMainVMList);
        //}

        public ActionResult GetUserAlerts(BaseVM payload)
        {
            var ctxt = Request.GetOwinContext();
            UserAlertsVM model = new UserAlertsVM()
            {
                Announcements = new UserAnnouncementLogic().Maintain<UserAnnouncementVM>(new Entities.View.DTS.DocumentsPayloadVM()
                {
                    userAnnouncement = new UserAnnouncementVM() { sender_id = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.UserId).ToSafeInt() }
                }).value.ToList().Where(a => a.is_read == false).ToList(),
                Notifications = new UserNotificationLogic().Maintain<UserNotificationVM>(new Entities.View.DTS.DocumentsPayloadVM()
                {
                    userNotification = new UserNotificationVM() { sender_id = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.UserId).ToSafeInt() }
                }).value.ToList().Where(a => a.is_read == false).ToList()
            };

            return new JsonResult { Data = model };
        }

        #region Temp Data

        //private List<UserVM> userList()
        //{
        //    List<UserVM> _list = new List<UserVM>();
        //    _list.Add(new UserVM() { id = 1, last_name = "Arellano", first_name = "Ron", gender = "M" });
        //    _list.Add(new UserVM() { id = 2, last_name = "Gonzales", first_name = "Jay", gender = "M" });
        //    _list.Add(new UserVM() { id = 3, last_name = "Ordanza", first_name = "Jio", gender = "M" });
        //    return _list;
        //}
        //private List<UserCredentialVM> userCredentials()
        //{
        //    List<UserCredentialVM> _cred = new List<UserCredentialVM>();
        //    _cred.Add(new UserCredentialVM() { id = 1, user_id = 1, user_name = "ron", password = "test123" });
        //    _cred.Add(new UserCredentialVM() { id = 2, user_id = 2, user_name = "jay", password = "test456" });
        //    _cred.Add(new UserCredentialVM() { id = 3, user_id = 3, user_name = "jio", password = "test789" });
        //    return _cred;
        //}
        //private List<UserDetailVM> userDetailList()
        //{
        //    List<UserDetailVM> _details = new List<UserDetailVM>();
        //    _details.Add(new UserDetailVM() { id = 1, user_id = 1, designation = "Developer", department = "I.T." });
        //    _details.Add(new UserDetailVM() { id = 2, user_id = 2, designation = "Developer", department = "I.T." });
        //    _details.Add(new UserDetailVM() { id = 3, user_id = 3, designation = "Developer", department = "I.T." });
        //    return _details;
        //}

        #endregion


        #region "User Info"
        public ActionResult GetMyUserInfo()
        {
            var payload = new Payload { auth_x_un = HttpContext.User.Identity.Name };

            var model = new Logic.User.UserHelper().GetUserbyUserName(payload);
            
     
            return View(model);
        }

        public ActionResult EditMyUserInfo()
        {
            var payload = new Payload { auth_x_un = HttpContext.User.Identity.Name };

            var model = new Logic.User.UserHelper().GetUserbyUserName(payload);


            return View(model);

            
        }

        [HttpPost]
        public ActionResult UpdateMyUserInfo(AccessUsersVM model)
        {
            var result = new Logic.User.UserHelper().UpdateUserInfo(model);

            if (result)
            {
                if (model.profile_link != null)
                {
                    if (model.profile_link != model.username.Replace(".jpg", ""))
                    {
                        var sourceFile = Path.Combine(Server.MapPath("~/UserImages/temp"), model.profile_link);

                        var des = Path.Combine(Server.MapPath("~/UserImages/"), model.username + ".jpg");

                        System.IO.File.Copy(sourceFile, des, true);


                    }
                }


            }

            if(result == true)
            {
                ActivityLogHelper.InsertActivityLog(Logic.ActivityLog.ActivityLogModule.ITMS, Logic.ActivityLog.ActivityLogType.Settings, "Update User Info", "Success");
            }
            else
            {
                ActivityLogHelper.InsertActivityLog(Logic.ActivityLog.ActivityLogModule.ITMS, Logic.ActivityLog.ActivityLogType.Settings, "Attempted Update User Info", "Failed");
            }


            return Json(new { result = result }, JsonRequestBehavior.AllowGet);



        }

        public ActionResult UserProfileImage(string userName)
        {
            
                var dir = Server.MapPath("~/UserImages");

                var path = Path.Combine(dir, userName + ".jpg"); //validate the path for security or use other means to generate the path.

            if(!System.IO.File.Exists(path))
            {
                path = Path.Combine(dir, "default.png");
            }

            return base.File(path, "image/jpeg");
            
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadUserImage()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var userName = HttpContext.User.Identity.Name;
                var old_fileName = Path.GetFileName(file.FileName);
                var fileType = Path.GetExtension(file.FileName);
                var new_FileName = string.Format("{0}_{1}{2}",userName, Guid.NewGuid().ToString(), fileType);

                var path = Path.Combine(Server.MapPath("~/UserIMages/temp"), new_FileName);

                file.SaveAs(path);

                return Json(new { filename = new_FileName, isSuccess= true }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { filename = "", isSuccess = false }, JsonRequestBehavior.AllowGet);

            

        }

        #endregion

        #region "Activity Log"

        public ActionResult GetMyActivityLog()
        {
            var model = new object();

            return View(model);
        }

        #endregion

        #region "Change Password"
        public ActionResult ChangePassword()
        {
            return View();
        }

        #endregion
    }
}