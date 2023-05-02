using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using OPBids.Entities.Common;
using OPBids.Entities.View.Auth;
using OPBids.Web.Helper;
using OPBids.Web.Models;
using OPBids.Common;
using OPBids.Entities.View.Setting;

namespace OPBids.Web.Logic.Auth.Manager
{
    public class CustomUserManager : UserManager<AuthUser>
    {
        public CustomUserManager()
            : base(new CustomUserStore<AuthUser>())
        {

        }

        public override Task<AuthUser> FindAsync(string userName, string password)
        {
            var taskInvoke = Task<AuthUser>.Factory.StartNew(() =>
            {
                try
                {
                    Result<AccessUsersVM> _result;
                    var apiManager = new ApiManager<Result<AccessUsersVM>>();

                    AesHelper aes = new AesHelper(ConfigManager.AesKey, ConfigManager.AesIV);

                    //TODO: Encrypt Data
                    AuthPayload payload = new AuthPayload() { auth_x_un = userName, auth_x_pwd = aes.Encrypt(password) };

                    _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/Authenticate", payload);

                    if (_result.status.code == Constant.Status.Success && _result.value != null)
                    {
                        var _user = _result.value;

                        return new AuthUser
                        {
                            Id = _user.id.ToString(),
                            UserName = _user.username,
                            GroupId = _user.group_id.ToString(),
                            department_id = _user.dept_id.ToString(),
                            full_name = string.Concat(_user.first_name, " ", _user.mi, " ", _user.last_name),
                            email = _user.email_address,
                            dashboard_id = _user.dashboard_id,
                            department_code = _user.dept_code,
                            department_name = _user.dept_description,
                            dts_access = _user.dts_access,
                            pmfs_access = _user.pfms_access,
                            vip = _user.vip_access
                        };
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError(ex, "Login Error");
                }
                return null;
            });

            return taskInvoke;
        }
        public Task<AuthUser> FindAsync_Old(string userName, string password)
        {
            var taskInvoke = Task<AuthUser>.Factory.StartNew(() =>
            {
                try
                {
                    Result<AccessUsersVM> _result;
                    var apiManager = new ApiManager<Result<AccessUsersVM>>();

                    AesHelper aes = new AesHelper(ConfigManager.AesKey, ConfigManager.AesIV);

                    //TODO: Encrypt Data
                    AuthPayload payload = new AuthPayload() { auth_x_un = userName, auth_x_pwd = aes.Encrypt(password) };

                    _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/Authenticate", payload);

                    if (_result.status.code == Constant.Status.Success && _result.value != null)
                    {
                        var _user = _result.value;

                        return new AuthUser
                        {
                            Id = _user.id.ToString(),
                            UserName = _user.username,
                            GroupId = _user.group_id.ToString(),
                            department_id = _user.dept_id.ToString(),
                            full_name = string.Concat(_user.first_name, " ", _user.mi, " ", _user.last_name),
                            email = _user.email_address,
                            dashboard_id = _user.dashboard_id,
                            department_code = _user.dept_code,
                            department_name = _user.dept_description,
                            dts_access = _user.dts_access,
                            pmfs_access = _user.pfms_access,
                            vip = _user.vip_access
                        };
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError(ex, "Login Error");
                }
                return null;
            });

            return taskInvoke;
        }

        public Task<AuthUser> ChangePassword(string activation_code, string password,int userid) {
            var taskInvoke = Task<AuthUser>.Factory.StartNew(() =>
            {
                Result<AccessUsersVM> _result;
                var apiManager = new ApiManager<Result<AccessUsersVM>>();

                AesHelper aes = new AesHelper(ConfigManager.AesKey, ConfigManager.AesIV);

                AuthPayload payload = new AuthPayload() { auth_x_code = activation_code, auth_x_pwd = aes.Encrypt(password), id = userid };

                _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/ChangePassword", payload);

                if (_result.status.code == Constant.Status.Success && _result.value != null)
                {
                    var _user = _result.value;

                    return new AuthUser
                    {
                        Id = _user.id.ToString(),
                        UserName = _user.username,
                        GroupId = _user.group_id.ToSafeString(),
                        department_id = _user.dept_id.ToSafeString(),
                        full_name = string.Concat(_user.first_name, " ", _user.mi, " ", _user.last_name),
                        email = _user.email_address,
                        dashboard_id = _user.dashboard_id,
                        department_code = _user.dept_code,
                        department_name = _user.dept_description.ToSafeString(),
                        pmfs_access = _user.pfms_access,
                        dts_access = _user.dts_access
                    };
                }
                return null;
            });

            return taskInvoke;
        }
        //public Task<AuthUser> ChangePassword2(int user_id, string password)
        //{
        //    var taskInvoke = Task<AuthUser>.Factory.StartNew(() =>
        //    {
        //        Result<AccessUsersVM> _result;
        //        var apiManager = new ApiManager<Result<AccessUsersVM>>();

        //        AesHelper aes = new AesHelper(ConfigManager.AesKey, ConfigManager.AesIV);

        //        AuthPayload payload = new AuthPayload() { id = user_id, auth_x_pwd = aes.Encrypt(password) };

        //        _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/ChangePassword2", payload);

        //        if (_result.status.code == Constant.Status.Success && _result.value != null)
        //        {
        //            var _user = _result.value;

        //            return new AuthUser
        //            {
        //                Id = _user.id.ToString(),
        //                UserName = _user.username,
        //                GroupId = _user.group_id.ToSafeString(),
        //                department_id = _user.dept_id.ToSafeString(),
        //                full_name = string.Concat(_user.first_name, " ", _user.mi, " ", _user.last_name),
        //                email = _user.email_address,
        //                dashboard_id = _user.dashboard_id,
        //                department_code = _user.dept_code,
        //                department_name = _user.dept_description.ToSafeString(),
        //                pmfs_access = _user.pfms_access,
        //                dts_access = _user.dts_access
        //            };
        //        }
        //        return null;
        //    });

        //    return taskInvoke;
        //}
    }
}