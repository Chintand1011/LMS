using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Entities.Common;
using OPBids.Entities.View.Shared;

namespace OPBids.Web.Controllers
{
    public class ActivityLogController : Controller
    {
        // GET: ActivityLog
        public ActionResult GetMyActivityLog()
        {
            IEnumerable<ActivityLogModel> model;

            var payload = new Entities.Common.Payload();

            payload.auth_x_un = HttpContext.User.Identity.Name;

            model = Logic.ActivityLog.ActivityLogHelper.GetMyActivityLog(payload);

            

            return View(model);
        }
    }
}