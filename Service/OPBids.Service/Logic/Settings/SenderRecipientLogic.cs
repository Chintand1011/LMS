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
    public class SenderRecipientLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<SenderRecipientUserVM>> GetSenderRecipient(Payload payload)
        {
            var _result = new Result<IEnumerable<SenderRecipientUserVM>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from a in db.SenderRecipient
                                 join b in db.SenderRecipientUser on a.user_id equals b.id into sru
                                 from psru in sru.DefaultIfEmpty()
                                 join c in db.AccessUser on a.user_id equals c.id into au
                                 from pau in au.DefaultIfEmpty()
                                 join d in db.Departments on a.department_id equals d.id into dep
                                 from pdep in dep.DefaultIfEmpty()
                                 where pau.status != Constant.RecordStatus.Deleted && a.status != Constant.RecordStatus.Deleted
                                 select new SenderRecipientUserVM()
                                 {
                                     status = a.status,
                                     id = a.id,
                                     department_name = pdep.dept_code,
                                     user_id = a.user_id,
                                     mobile_no = a.mobile_no,
                                     created_by = a.created_by,
                                     department_id = a.department_id,
                                     created_date = a.created_date.ToString(),
                                     email_address = (a.is_system_user ? pau.email_address : psru.email_address),
                                     first_name = (a.is_system_user ? pau.first_name : psru.first_name),
                                     mi = (a.is_system_user ? pau.mi : psru.mi),
                                     last_name = (a.is_system_user ? pau.last_name : psru.last_name),
                                     salutation = (a.is_system_user ? pau.salutation : psru.salutation),
                                     is_recipient = a.is_recipient,
                                     is_sender = a.is_sender,
                                     is_system_user = a.is_system_user,
                                     updated_by = a.updated_by,
                                     updated_date = a.updated_date.ToString()
                                 }).ToList();
            }
            else
            {
                _result.value = (from a in db.SenderRecipient
                                 join b in db.SenderRecipientUser on a.user_id equals b.id into sru
                                 from psru in sru.DefaultIfEmpty()
                                 join c in db.AccessUser on a.user_id equals c.id into au
                                 from pau in au.DefaultIfEmpty()
                                 join d in db.Departments on a.department_id equals d.id into dep
                                 from pdep in dep.DefaultIfEmpty()
                                 where a.status != Constant.RecordStatus.Deleted &&
                                 (a.is_system_user == true && pau.status != Constant.RecordStatus.Deleted || a.is_system_user == false) &&
                                 (a.is_system_user == true ? (string.Concat(pau.first_name, " ", pau.mi, " ",
                                 pau.last_name)).Replace(" ", "").ToLower().Contains(payload.search_key.ToLower().Replace(" ", "")) ||
                                 pau.email_address.ToLower().Contains(payload.search_key.ToLower()) :
                                 ((string.Concat(psru.first_name, " ", psru.mi, " ",
                                 psru.last_name)).Replace(" ", "").ToLower().Contains(payload.search_key.ToLower()) ||
                                 psru.email_address.ToLower().Contains(payload.search_key.ToLower())))
                                 select new SenderRecipientUserVM()
                                 {
                                     status = a.status,
                                     id = a.id,
                                     department_name = pdep.dept_code,
                                     user_id = a.user_id,
                                     mobile_no = a.mobile_no,
                                     created_by = a.created_by,
                                     department_id = a.department_id,
                                     created_date = a.created_date.ToString(),
                                     email_address = (a.is_system_user ? pau.email_address : psru.email_address),
                                     first_name = (a.is_system_user ? pau.first_name : psru.first_name),
                                     mi = (a.is_system_user ? pau.mi : psru.mi),
                                     last_name = (a.is_system_user ? pau.last_name : psru.last_name),
                                     salutation = (a.is_system_user ? pau.salutation : psru.salutation),
                                     is_recipient = a.is_recipient,
                                     is_sender = a.is_sender,
                                     is_system_user = a.is_system_user,
                                     updated_by = a.updated_by,
                                     updated_date = a.updated_date.ToString()
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

        public Result<IEnumerable<SenderRecipientUserVM>> CreateSenderRecipient(SettingVM param)
        {
            var _result = new Result<IEnumerable<SenderRecipientUserVM>>();
            try
            {
                var senderRecipient = param.senderRecipientUser;
                var sr = new SenderRecipient();
                var sru = new SenderRecipientUser();
                using (var db = new DatabaseContext())
                {
                    var userId = senderRecipient.user_id;
                    if (senderRecipient.is_system_user == false)
                    {
                        sru.created_date = DateTime.Now;
                        sru.updated_date = DateTime.Now;
                        sru.created_by = senderRecipient.created_by;
                        sru.updated_by = senderRecipient.updated_by;
                        sru.id = senderRecipient.id;
                        sru.email_address = senderRecipient.email_address;
                        sru.first_name = senderRecipient.first_name;
                        sru.mi = senderRecipient.mi;
                        sru.last_name = senderRecipient.last_name;
                        sru.salutation = senderRecipient.salutation;
                        db.SenderRecipientUser.Add(sru);
                        db.SaveChanges();
                        userId = db.SenderRecipientUser.FirstOrDefault(a => a.email_address == senderRecipient.email_address).id;
                    }
                    sr.user_id = userId;
                    sr.status = Constant.RecordStatus.Active;
                    sr.created_date = DateTime.Now;
                    sr.updated_date = DateTime.Now;
                    sr.created_by = senderRecipient.created_by;
                    sr.updated_by = senderRecipient.updated_by;
                    sr.department_id = senderRecipient.department_id;
                    sr.is_recipient = senderRecipient.is_recipient;
                    sr.is_sender = senderRecipient.is_sender;
                    sr.is_system_user = senderRecipient.is_system_user;
                    sr.status = senderRecipient.status;
                    sr.mobile_no = senderRecipient.mobile_no;
                    db.SenderRecipient.Add(sr);
                    db.SaveChanges();
                    _result = GetSenderRecipient(new Payload() { page_index = param.page_index });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<SenderRecipientUserVM>> UpdateSenderRecipient([FromBody] SettingVM param)
        {
            var _result = new Result<IEnumerable<SenderRecipientUserVM>>();
            try
            {
                var senderRecipient = param.senderRecipientUser;
                using (var db = new DatabaseContext())
                {
                    var sr = db.SenderRecipient.FirstOrDefault(a => a.id == senderRecipient.id);
                    sr.status = Constant.RecordStatus.Active;
                    sr.created_date = DateTime.Now;
                    sr.updated_date = DateTime.Now;
                    sr.created_by = senderRecipient.created_by;
                    sr.updated_by = senderRecipient.updated_by;
                    sr.department_id = senderRecipient.department_id;
                    sr.id = senderRecipient.id;
                    sr.user_id = senderRecipient.user_id;
                    sr.mobile_no = senderRecipient.mobile_no;
                    sr.is_recipient = senderRecipient.is_recipient;
                    sr.is_sender = senderRecipient.is_sender;
                    sr.is_system_user = senderRecipient.is_system_user;
                    sr.status = senderRecipient.status;
                    sr.mobile_no = senderRecipient.mobile_no;
                    db.SenderRecipient.AddOrUpdate(sr);
                    if (senderRecipient.is_system_user == true)
                    {
                        var au = db.AccessUser.FirstOrDefault(a => a.email_address == senderRecipient.email_address);
                        au.updated_by = senderRecipient.updated_by;
                        au.first_name = senderRecipient.first_name.ToSafeString();
                        au.mi = senderRecipient.mi.ToSafeString();
                        au.last_name = senderRecipient.last_name.ToSafeString();
                        au.email_address = senderRecipient.email_address;
                        au.salutation = senderRecipient.salutation;
                        db.AccessUser.AddOrUpdate(au);
                    }
                    var sru = db.SenderRecipientUser.FirstOrDefault(a => a.email_address == senderRecipient.email_address);
                    if (sru != null)
                    {
                        sru.created_date = DateTime.Now;
                        sru.updated_date = DateTime.Now;
                        sru.created_by = senderRecipient.created_by;
                        sru.updated_by = senderRecipient.updated_by;
                        sru.email_address = senderRecipient.email_address;
                        sru.first_name = senderRecipient.first_name;
                        sru.mi = senderRecipient.mi;
                        sru.last_name = senderRecipient.last_name;
                        sru.salutation = senderRecipient.salutation;
                        db.SenderRecipientUser.AddOrUpdate(sru);
                    }
                    db.SaveChanges();
                    _result = GetSenderRecipient(new Payload() { page_index = param.page_index });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<SenderRecipientUserVM>> StatusUpdateSenderRecipient([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<SenderRecipientUserVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (var id in payload.item_list)
                        {
                            var _SenderRecipient = db.SenderRecipient.Find(Convert.ToInt32(id));
                            _SenderRecipient.status = payload.status;
                            _SenderRecipient.updated_date = DateTime.Now;
                            _SenderRecipient.updated_by = payload.user_id;
                            db.SenderRecipient.AddOrUpdate(_SenderRecipient);
                        }
                    }
                    db.SaveChanges();

                    _result = GetSenderRecipient(new Payload() { page_index = payload.page_index });
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