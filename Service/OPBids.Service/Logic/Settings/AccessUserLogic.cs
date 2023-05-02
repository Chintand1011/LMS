using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;

namespace OPBids.Service.Logic.Settings
{
    public class AccessUserLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<AccessUsersVM>> GetAccessUser(Payload payload)
        {
            var _result = new Result<IEnumerable<AccessUsersVM>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from a in db.AccessUser
                                 join b in db.AccessGroup on a.group_id equals b.id into ag
                                 from agu in ag.DefaultIfEmpty()
                                 where a.status != Constant.RecordStatus.Deleted
                                 select new AccessUsersVM()
                                 {
                                     updated_date = a.updated_date.ToString(),
                                     created_by = a.created_by,
                                     created_date = a.created_date.ToString(),
                                     group_id = a.group_id,
                                     first_name = a.first_name,
                                     mi = a.mi,
                                     last_name = a.last_name,
                                     salutation = a.salutation,
                                     status = a.status,
                                     username = a.username,
                                     dept_id = a.dept_id,
                                     activation_code = a.activation_code,
                                     email_address = a.email_address,
                                     group_code = agu.group_code,
                                     id = a.id,
                                     updated_by = a.updated_by,
                                     dts_access = a.dts_access,
                                     pfms_access = a.pfms_access,
                                     vip_access = a.vip_access,
                                 }).ToList();

            }
            else
            {
                payload.search_key = payload.search_key.ToLower().Trim();
                _result.value = (from a in db.AccessUser
                                 join b in db.AccessGroup on a.group_id equals b.id into ag
                                 from agu in ag.DefaultIfEmpty()
                                 join c in db.Departments on a.dept_id equals c.id into ad
                                 from adu in ad.DefaultIfEmpty()
                                 where (a.status != Constant.RecordStatus.Deleted && (agu.group_code.ToLower().Replace(payload.search_key, "") != agu.group_code.ToLower() ||
                                 a.username.ToLower().Replace(payload.search_key, "") != a.username.ToLower() ||
                                 agu.group_description.ToLower().Replace(payload.search_key, "") != agu.group_description.ToLower() ||
                                 adu.dept_code.ToLower().Replace(payload.search_key, "") != adu.dept_code.ToLower() ||
                                 adu.dept_description.ToLower().Replace(payload.search_key, "") != adu.dept_description.ToLower() ||
                                 string.Concat(a.first_name, " ", a.mi, " ", a.last_name).ToLower().Replace(payload.search_key, "") !=
                                 string.Concat(a.first_name, " ", a.mi, " ", a.last_name).ToLower()))
                                 select new AccessUsersVM()
                                 {
                                     updated_date = a.updated_date.ToString(),
                                     created_by = a.created_by,
                                     created_date = a.created_date.ToString(),
                                     group_id = a.group_id,
                                     first_name = a.first_name,
                                     mi = a.mi,
                                     last_name = a.last_name,
                                     salutation = a.salutation,
                                     status = a.status,
                                     username = a.username,
                                     dept_id = a.dept_id,
                                     activation_code = a.activation_code,
                                     email_address = a.email_address,
                                     group_code = agu.group_code,
                                     id = a.id,
                                     updated_by = a.updated_by,
                                     dts_access = a.dts_access,
                                     pfms_access = a.pfms_access,
                                     vip_access = a.vip_access,
                                 }).ToList();
            }
            _result.total_count = _result.value.Count();
            if (payload.page_index != -1)
            {
                _result.page_count = _result.value.Count().GetPageCount();
                _result.value = _result.value.Skip(Constant.AppSettings.PageItemCount * payload.page_index).
                                     Take(Constant.AppSettings.PageItemCount);
            }
            return _result;
        }

        public AccessUsersVM GetAccessUserByUserName(string userName)
        {
            //var _result = new AccessUsersVM>();


            var a = db.AccessUser
                .Where(x => x.username == userName)
                .FirstOrDefault();

            if (a != null)
            {
                var model = new AccessUsersVM()
                {
                    username = a.username
                     ,
                    nickname = a.nickname
                     ,
                    first_name = a.first_name
                     ,
                    last_name = a.last_name
                     ,
                    email_address = a.email_address
                     ,
                    business_email_address = a.business_email_address
                     ,
                    contact_no = a.contact_no
                     ,
                    mobile_no = a.mobile_no
                    ,
                    profile_link = a.profile_link
                    ,
                    address1 = a.address1
                    ,
                    address2 = a.address2
                    ,
                    address3 = a.address3
                    ,
                    DOB = String.Format("{0:yyyy-MM-dd}", a.DOB)


                };

                return model;

            }

            return null;
        }

        public bool UpdateUserInfo(AccessUsersVM model)
        {
            var result = db.AccessUser
                .Where(x => x.username == model.username).FirstOrDefault();

            if (result == null)
            {
                return false;

            }

            result.first_name = model.first_name;
            result.nickname = model.nickname;
            result.last_name = model.last_name;
            result.email_address = model.email_address;
            result.business_email_address = model.business_email_address;
            result.contact_no = model.contact_no;
            result.mobile_no = model.mobile_no;
            result.profile_link = model.profile_link; // this should be image filename not link
            result.address1 = model.address1;
            result.address2 = model.address2;
            result.address3 = model.address3;
            result.DOB = Convert.ToDateTime(model.DOB);
            // try catch here
            //:TODO
            db.SaveChanges();
            return true;

        }



        public Result<IEnumerable<AccessUsersVM>> SendResetEmail(Payload payload)
        {
            var _result = new Result<IEnumerable<AccessUsersVM>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result = GetAccessUser(new Payload() { });
                if (_result.value != null && _result.value.Count() > 0)
                {
                    var recepient = _result.value.FirstOrDefault(a => a.email_address.ToLower() == payload.search_key.ToLower());
                    if (recepient != null)
                    {
                        recepient.activation_code = System.Guid.NewGuid().ToString();
                        UpdateAccessUser(recepient);
                        CommonMethods.SendEmail("Verify your registration", string.Concat(Constant.AppSettings.VerifyEmailUrl,
                            recepient.activation_code), new List<string>() { recepient.email_address });
                    }
                }
            }
            return _result;
        }

        public Result<IEnumerable<AccessUsersVM>> CreateAccessUser(AccessUsersVM param)
        {
            var _result = new Result<IEnumerable<AccessUsersVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    string _guid = System.Guid.NewGuid().ToString();
                    db.AccessUser.Add(new AccessUser()
                    {
                        activation_code = _guid,
                        created_by = param.created_by,
                        created_date = DateTime.Now,
                        email_address = param.email_address,
                        first_name = param.first_name,
                        mi = param.mi,
                        last_name = param.last_name,
                        group_id = param.group_id,
                        id = param.id,
                        password = param.password,
                        salutation = param.salutation,
                        status = Constant.RecordStatus.Active,
                        dept_id = param.dept_id,
                        updated_by = param.updated_by,
                        updated_date = DateTime.Now,
                        username = param.username,
                        vip_access = param.vip_access,
                        pfms_access = param.pfms_access,
                        dts_access = param.dts_access,
                        DOB = DateTime.Today,
                    });
                    db.SaveChanges();
                    //Select all records
                    _result = GetAccessUser(new Payload() { page_index = param.page_index });

                    System.Text.StringBuilder content = new System.Text.StringBuilder();
                    content.AppendFormat("Welcome {0},", string.Concat(param.first_name, " ", param.mi, " ", param.last_name));
                    content.AppendLine("<br><br>");
                    content.AppendLine("You have been registered to MMDA PFMS Portal.");
                    content.AppendLine("<br>");
                    content.AppendLine("To continue, please click the link below and reset your passsord.");
                    content.AppendLine("<br>");
                    content.AppendLine(string.Concat(Constant.AppSettings.VerifyEmailUrl, _guid));
                    content.AppendLine("<br>");
                    content.AppendLine("Thank you.");
                    content.AppendLine("<br><br>");
                    content.AppendLine("MMDA Portal Admin");

                    CommonMethods.SendEmail("Verify your registration",
                                            content.ToString(),
                                            new List<string>() { param.email_address });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            _result = GetAccessUser(new Payload());
            return _result;
        }

        public Result<IEnumerable<AccessUsersVM>> UpdateAccessUser([FromBody] AccessUsersVM param)
        {
            var _result = new Result<IEnumerable<AccessUsersVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    var itm = db.AccessUser.First(a => a.id == param.id);
                    itm.activation_code = System.Guid.NewGuid().ToString();
                    itm.activation_flag = false;
                    itm.email_address = param.email_address;
                    itm.first_name = param.first_name;
                    itm.mi = param.mi;
                    itm.last_name = param.last_name;
                    itm.group_id = param.group_id;
                    itm.id = param.id;
                    itm.dept_id = param.dept_id;
                    itm.salutation = param.salutation;
                    itm.status = Constant.RecordStatus.Active;
                    itm.updated_by = param.updated_by;
                    itm.updated_date = DateTime.Now;
                    itm.username = param.username;
                    itm.dts_access = param.dts_access;
                    itm.pfms_access = param.pfms_access;
                    itm.vip_access = param.vip_access;

                    if (!string.IsNullOrEmpty(param.password))
                    {
                        itm.password = param.password;
                    }

                    db.AccessUser.AddOrUpdate(itm);
                    db.SaveChanges();
                    _result = GetAccessUser(new Payload() { page_index = param.page_index });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<AccessUsersVM>> StatusUpdateAccessUser([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<AccessUsersVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (var id in payload.item_list)
                        {
                            var _AccessUser = db.AccessUser.Find(Convert.ToInt32(id));
                            _AccessUser.status = payload.status;
                            _AccessUser.updated_date = DateTime.Now;
                            _AccessUser.updated_by = payload.updated_by;
                            db.AccessUser.AddOrUpdate(_AccessUser);
                        }
                    }
                    db.SaveChanges();
                    _result = GetAccessUser(payload);
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
            }
            return _result;
        }

        public Result<bool> ResetAccessUserPassword([FromBody] string email)
        {
            var _result = new Result<bool>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    var _AccessUser = db.AccessUser.Where(au => au.email_address == email).First();
                    if (_AccessUser != null)
                    {
                        _AccessUser.activation_code = Guid.NewGuid().ToString();
                        _AccessUser.activation_flag = false;
                        db.AccessUser.AddOrUpdate(_AccessUser);
                        db.SaveChanges();
                        _result.value = true;
                        string resetlink = string.Concat(Constant.AppSettings.VerifyEmailUrl, _AccessUser.activation_code);
                        string linkedresource = System.Web.HttpContext.Current.Server.MapPath(@"~/Images/logo.jpg");
                        string contentId = "companylogo";
                        string mailbody = "<p style='text-align:center; '><img src=cid:" + contentId + " style='display: block;margin-left: auto; margin-right: auto; width: 20%;' /></p>";
                        mailbody += "<br /><p><font size='3'>Hi <b>" + _AccessUser.first_name + "</b>,</font></p>";
                        mailbody += "<p><font size='3'>We've received a request to reset your password. If you didn't make the request, please ignore this email. Otherwise, click the button below to reset it:</font></p>";
                        mailbody += "<p><a href='" + resetlink + "' style='background-color:#ff0000;border:1px solid #EB7035;border-radius:3px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:16px;line-height:44px;text-align:center;text-decoration:none;width:100%;-webkit-text-size-adjust:none;mso-hide:all;'>Reset Password</a></p>";
                        mailbody += "<p><font size='3'>Thanks,";
                        mailbody += "<br />DTS Admin</font></p>";
                        mailbody += "<br /><br /><hr>";
                        mailbody += "<p><font size='2'>If you're having trouble clicking 'Reset Password' button, click the URL below:</font>";
                        mailbody += "<br /><font size='2'><a href = '" + resetlink + "'>" + resetlink + "</a></font>";
                        mailbody += "<br /><br /><font size='2'>For questions or concerns, please email <b><a href = 'mailto:dtsadmin@mmda.gov.ph?subject = Feedback&body = Message'>dtsadmin @mmda.gov.ph</a></b></font></p>";

                        CommonMethods.SendEmailEmbedImage("Reset Password", mailbody, new List<string>() { _AccessUser.email_address }, linkedresource, contentId);
                    }
                    else
                    {
                        _result.status = new Status()
                        {
                            code = Constant.Status.Failed,
                            description = "Record does not exists"
                        };
                        _result.value = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
                _result.value = false;
            }
            return _result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}