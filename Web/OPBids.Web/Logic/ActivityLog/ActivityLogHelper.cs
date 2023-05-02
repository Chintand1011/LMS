using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Shared;
using OPBids.Web.Helper;
using OPBids.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace OPBids.Web.Logic.ActivityLog
{
    public static class ActivityLogHelper
    {
        public static IEnumerable<ActivityLogModel> GetMyActivityLog(Payload payload)
        {
            var taskInvoke = Task<List<ActivityLogModel>>.Factory.StartNew(() =>
            {
                List<ActivityLogModel> _result;

                var apiManager = new ApiManager<List<ActivityLogModel>>();

                _result = apiManager.InvokeGetLog(ConfigManager.BaseServiceURL, "service/ActivityLog", payload);

                return _result;
            });

            return taskInvoke.Result;
        }


        public static bool InsertActivityLog(ActivityLogModule ActivityLogModule, ActivityLogType ActivityLogType, string Activity, string ActivityAction)
        {

            var user = new AuthUser();

            //attemp to unwrap the session
            if (HttpContext.Current.Session["user"] != null)
            {
                user = (AuthUser)HttpContext.Current.Session["user"];
            }

            return InsertActivityLog(ActivityLogModule, ActivityLogType, Activity, ActivityAction, user);

        }


        public static bool InsertActivityLogAttempFailed(ActivityLogModule ActivityLogModule, ActivityLogType ActivityLogType, string Activity, string ActivityAction, string userName)
        {

            var model = new ActivityLogModel();

            model.Action = ActivityAction;
            model.Activities = Activity;
            model.DateTime = DateTime.Now;
            model.FullName = "";
            model.IPAddress = GetUserClientIPAddress();
            model.Module = ActivityLogModule.ToString();
            model.RecordId = GenerateRecordId();
            model.UserId = "0";
            model.UserName = userName;
            model.Type = ActivityLogType.ToString();

            var result = ExecuteInsert(model);

            return result;

        }

        //public static bool InsertActivityLog(ActivityLogModule ActivityLogModule, ActivityLogType ActivityLogType, string Activity, string ActivityAction, string userName)
        //{

        //    var model = new ActivityLogModel();

        //    model.Action = ActivityAction;
        //    model.Activities = Activity;
        //    model.DateTime = DateTime.Now;
        //    model.FullName = "";
        //    model.IPAddress = GetUserClientIPAddress();
        //    model.Module = ActivityLogModule.ToString();
        //    model.RecordId = GenerateRecordId();
        //    model.UserId = "";
        //    model.UserName = userName;
        //    model.Type = ActivityLogType.ToString();

        //    var result = ExecuteInsert(model);

        //    return result;

        //}

        public static bool InsertActivityLog(ActivityLogModule ActivityLogModule, ActivityLogType ActivityLogType, string Activity, string ActivityAction, AuthUser userObject)
        {

            var model = new ActivityLogModel();

            model.Action = ActivityAction;
            model.Activities = Activity;
            model.DateTime = DateTime.Now;
            model.FullName = userObject.full_name;
            model.IPAddress = GetUserClientIPAddress();
            model.Module = ActivityLogModule.ToString();
            model.RecordId = GenerateRecordId();
            model.UserId = userObject.Id;
            model.UserName = userObject.UserName;
            model.Type = ActivityLogType.ToString();

            var result = ExecuteInsert(model);

            return result;

        }


        static string GenerateRecordId()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "").ToUpper().Substring(0, 5);
            var recordid = string.Format("{0:yyyyMMddHHmmss}-{1}", DateTime.Now, guid);

            return recordid;
        }

        static bool ExecuteInsert(ActivityLogModel model)
        {
            var taskInvoke = Task<bool>.Factory.StartNew(() =>
            {
                Result<bool> _result;
                var apiManager = new ApiManager<Result<bool>>();

                _result = apiManager.InvokeActivityLog(ConfigManager.BaseServiceURL, "service/InsertActivityLog", model);

                try
                {
                    if (_result.status.code == Constant.Status.Success && _result.value != false)
                    {
                        return true;
                    }
                }
                catch(Exception ex)
                {

                }
                return false;
            });

            return taskInvoke.Result;
        }

        

        static string GetUserClientIPAddress()
        {
            //return "192.168.8.1";
            var request = new HttpRequestWrapper(HttpContext.Current.Request);


            return GetIpAddress(request);
        }

        public static string GetIpAddress(this HttpRequestBase request)
        {
            if (request.Headers["CF-CONNECTING-IP"] != null)
                return request.Headers["CF-CONNECTING-IP"];

            var ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                var addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                    return addresses[0];
            }

            return request.UserHostAddress;
        }

        static string GetUserFullName(string userName)
        {
            return "Archie Angeles";
        }

        
    }

    public enum ActivityLogModule
    {
        ITMS,
        PFMS,
        DTS
    }

    public enum ActivityLogType
    {
        Login,
        SignOut,
        DTSDashBoard,
        DTSToolBar,
        Scan,
        OnHand,
        PFMSDashBoard,
        Settings

    }

    
}