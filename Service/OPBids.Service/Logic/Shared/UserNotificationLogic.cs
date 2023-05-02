using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.DTS;
using OPBids.Entities.View.Shared;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;

namespace OPBids.Service.Logic.Shared
{
    public class UserNotificationLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<UserNotificationVM>> MaintainData(DocumentsPayload param)
        {
            Result<IEnumerable<UserNotificationVM>> rslts;
            switch (param.userNotification.process.ToSafeString().ToUpper())
            {
                case Constant.TransactionType.Create:
                    rslts = Create(param);
                    break;
                case Constant.TransactionType.Update:
                    rslts = Update(param);
                    break;
                case Constant.TransactionType.Hide:
                    rslts = Hide(param);
                    break;
                case Constant.TransactionType.Read:
                case Constant.TransactionType.UnRead:
                    rslts = ReadUnRead(param);
                    break;
                case Constant.TransactionType.Star:
                case Constant.TransactionType.UnStar:
                    rslts = StarUnStar(param);
                    break;
                default:
                    rslts = GetData(param);
                    break;
            }
            return rslts;
        }
        public Result<IEnumerable<UserNotificationVM>> GetData(DocumentsPayload payload)
        {
            var _result = new Result<IEnumerable<UserNotificationVM>>();
            _result.value = (from d in db.UserNotification
                             join s in db.AccessUser on d.sender_id equals s.id into s1
                             from s2 in s1.DefaultIfEmpty()
                             where d.is_hidden == false && (payload.userNotification.is_starred == null || d.is_starred == payload.userNotification.is_starred) &&
                             (payload.userNotification.is_read == null || d.is_read == payload.userNotification.is_read) && 
                             string.Concat(",",d.recipient_ids, ",").Replace(payload.userNotification.recipient_ids, "") != string.Concat(",", d.recipient_ids, ",") && 
                             (payload.userNotification.is_starred == null || d.is_starred == payload.userNotification.is_starred)
                             select new UserNotificationVM()
                             {
                                 id = d.id,
                                 is_hidden = d.is_hidden,
                                 is_read = d.is_read,
                                 is_starred = d.is_starred,
                                 message = d.message,
                                 recipient_ids = d.recipient_ids,
                                 sender_id = d.sender_id,
                                 sender_name = string.Concat(s2.first_name, " ", s2.mi, " ", s2.last_name),
                                 date_sent = d.date_sent.ToString()
                             }).ToList().Where(a => a.sender_id == payload.userNotification.sender_id || a.recipient_ids.Split(',').Contains(payload.userNotification.sender_id.ToString())).OrderBy(a=> a.date_sent.ToDate());
            _result.total_count = _result.value.Count();
            _result.value.ToList().ForEach(a =>
            {
                var rslts = db.AccessUser.Where(au => ("," + a.recipient_ids + ",").Contains("," + au.id.ToString() + ",")).Select(p => string.Concat(p.first_name, " ", p.mi, " ", p.last_name)).ToList();
                a.recipient_names = String.Join("; ", rslts);

            });
            if (payload.page_index != -1)
            {
                _result.page_count = _result.value.Count().GetPageCount();
                _result.value = _result.value.Skip(Constant.AppSettings.PageItemCount * payload.page_index).
                                     Take(Constant.AppSettings.PageItemCount);
            }
            return _result;
        }

        public Result<IEnumerable<UserNotificationVM>> Create(DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserNotificationVM>>();
            try
            {
                var recipientIds = new List<int>();
                if (param.userNotification.department_ids.ToSafeString() != "" || param.userNotification.recipient_ids.ToSafeString() != "")
                {
                    var rslts = db.AccessUser.Where(a => ("," + param.userNotification.department_ids + ",").Contains("," + a.dept_id.ToString() + ",") == true || 
                    ("," + param.userNotification.recipient_ids + ",").Contains("," + a.id.ToString() + ",") == true).Select(a => a.id);
                    recipientIds.AddRange(rslts.ToArray());
                }
                using (var db = new DatabaseContext())
                {
                    var itm = new UserNotification()
                    {
                        sender_id = param.userNotification.sender_id,
                        is_hidden = param.userNotification.is_hidden,
                        is_read = param.userNotification.is_read ?? false,
                        is_starred = param.userNotification.is_starred ?? false,
                        message = param.userNotification.message,
                        recipient_ids = String.Join(",", recipientIds),
                        date_sent = DateTime.Now,
                    };
                    db.UserNotification.Add(itm);
                    db.SaveChanges();
                    param.userNotification.sender_id = param.userNotification.sender_id;
                    return GetData(param);
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

        public Result<IEnumerable<UserNotificationVM>> Update([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserNotificationVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.updated_date = DateTime.Now;
                    var itm = db.UserNotification.Find(param.userNotification.id);
                    itm.sender_id = param.userNotification.sender_id;
                    itm.is_hidden = param.userNotification.is_hidden;
                    itm.is_read = param.userNotification.is_read ?? false;
                    itm.is_starred = param.userNotification.is_starred ?? false;
                    itm.message = param.userNotification.message;
                    itm.recipient_ids = param.userNotification.recipient_ids;
                    itm.date_sent = DateTime.Now;
                    db.UserNotification.AddOrUpdate(itm);
                    db.SaveChanges();
                    param.userNotification.sender_id = itm.sender_id;
                    return GetData(param);
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
        public Result<IEnumerable<UserNotificationVM>> StarUnStar([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserNotificationVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.userNotification.ids.Split(',').ToList().ForEach(id =>
                    {
                        param.updated_date = DateTime.Now;
                        var itm = db.UserNotification.Find(id.ToSafeInt());
                        itm.is_starred = param.userNotification.process == Constant.TransactionType.Star;
                        db.UserNotification.AddOrUpdate(itm);
                    });
                    db.SaveChanges();
                    return GetData(param);
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
        public Result<IEnumerable<UserNotificationVM>> ReadUnRead([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserNotificationVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.userNotification.ids.Split(',').ToList().ForEach(id =>
                    {
                        param.updated_date = DateTime.Now;
                        var itm = db.UserNotification.Find(id.ToSafeInt());
                        itm.is_read = param.userNotification.process == Constant.TransactionType.Read;
                        db.UserNotification.AddOrUpdate(itm);
                    });
                    db.SaveChanges();
                     return GetData(param);
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
        public Result<IEnumerable<UserNotificationVM>> Hide([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserNotificationVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.userNotification.ids.Split(',').ToList().ForEach(id =>
                    {
                        param.updated_date = DateTime.Now;
                        var itm = db.UserNotification.Find(id.ToSafeInt());
                        itm.is_hidden = true;
                        db.UserNotification.AddOrUpdate(itm);
                    });
                    db.SaveChanges();
                    return GetData(param);
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