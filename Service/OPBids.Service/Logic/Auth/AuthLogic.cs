using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using OPBids.Service.Utilities;
using System;
using System.Linq;
using System.Web.Http;

namespace OPBids.Service.Logic.Auth
{
    public class AuthLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<AccessUsersVM> Authenticate(Payload payload)
        {
            Result<AccessUsersVM> _result = new Result<AccessUsersVM>();
            try
            {
               

                if (payload.auth_x_un != null && payload.auth_x_un != string.Empty && payload.auth_x_pwd != null && payload.auth_x_pwd != string.Empty)
                {
                    var _user = (from user in db.AccessUser
                                 where user.username == payload.auth_x_un &&
                                         user.status == Constant.RecordStatus.Active
                                 select user).FirstOrDefault();


                    if (_user.username != null && _user.password != null)
                    {
                        if (_user.password == payload.auth_x_pwd)
                        {
                            // Remove password from result
                            _user.password = string.Empty;

                            var _dashboard = (from ag in db.AccessGroup
                                              where ag.id == _user.group_id
                                              select ag.dashboard_id).First();

                            var _dept = (from dp in db.Departments
                                         where dp.id == _user.dept_id
                                         select  dp).First();


                            _result.value = _user.ToView(_dashboard, _user.dept_id, _dept.dept_description, _dept.dept_code);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

                LogHelper.LogError(ex, "Login Error");
            }
            return _result;
        }

        public Result<AccessUsersVM> ChangePassword(Payload payload)
        {
            Result<AccessUsersVM> _result = new Result<AccessUsersVM>();

            if (payload.auth_x_code != null && payload.auth_x_code != string.Empty && payload.auth_x_pwd != null && payload.auth_x_pwd != string.Empty)
            {
                var _user = (from user in db.AccessUser
                             where user.activation_code == payload.auth_x_code &&
                                user.status == Constant.RecordStatus.Active &&
                                user.activation_flag == false
                             select user).FirstOrDefault();
                if (_user != null)
                {
                    _user.password = payload.auth_x_pwd;
                    _user.activation_flag = true;
                    db.SaveChanges();

                    // Remove password from result
                    _user.password = string.Empty;

                    var _dashboard = (from ag in db.AccessGroup
                                      where ag.id == _user.group_id
                                      select ag.dashboard_id).First();

                    _result.value = _user.ToView(_dashboard);
                }
                else
                {
                    _result.status = new Status() { code = 1 };
                }
            }
            else if(payload.id != 0 && payload.auth_x_pwd != null && payload.auth_x_pwd != string.Empty)
            {
                var _user = (from user in db.AccessUser
                             where user.id == payload.id &&
                                user.status == Constant.RecordStatus.Active 
                             select user).FirstOrDefault();
                if (_user != null)
                {
                    _user.password = payload.auth_x_pwd;
                    _user.activation_flag = true;
                    db.SaveChanges();

                    // Remove password from result
                    _user.password = string.Empty;

                    var _dashboard = (from ag in db.AccessGroup
                                      where ag.id == _user.group_id
                                      select ag.dashboard_id).First();

                    _result.value = _user.ToView(_dashboard);
                }
                else
                {
                    _result.status = new Status() { code = 1 };
                }
            }
            {



            }
            return _result;
        }

    }
}