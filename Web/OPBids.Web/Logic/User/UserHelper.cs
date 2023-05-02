using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OPBids.Web.Logic.User
{
    public class UserHelper
    {

        public UserHelper()
        {

        }

        public AccessUsersVM GetUserbyUserName(Payload payload)
        {
            var taskInvoke = Task<AccessUsersVM>.Factory.StartNew(() =>
            {
                //List<AccessUsersVM> _result;

                var apiManager = new ApiManager<AccessUsersVM>();

                var _result = apiManager.InvokeGetLog(ConfigManager.BaseServiceURL, "service/GetAccessUserByUserName", payload);

                return _result;
            });

            return taskInvoke.Result;
        }

        public bool UpdateUserInfo(AccessUsersVM model)
        {
            var taskInvoke = Task<bool>.Factory.StartNew(() =>
            {
                //List<AccessUsersVM> _result;

                var apiManager = new ApiManager<bool>();

                var _result = apiManager.InvokeUpdateUserInfo(ConfigManager.BaseServiceURL, "service/UpdateUerInfo", model);

                return _result;
            });

            return taskInvoke.Result;
        }
    }
}