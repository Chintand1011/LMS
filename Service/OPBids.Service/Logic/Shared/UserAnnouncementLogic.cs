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
    public class UserAnnouncementLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<UserAnnouncementVM>> MaintainData(DocumentsPayload param)
        {
            Result<IEnumerable<UserAnnouncementVM>> rslts;
            switch (param.userAnnouncement.process.ToSafeString().ToUpper())
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
        public Result<IEnumerable<UserAnnouncementVM>> GetData(DocumentsPayload payload)
        {
            var _result = new Result<IEnumerable<UserAnnouncementVM>>();
            _result.value = (from d in db.UserAnnouncement
                             join s in db.AccessUser on d.sender_id equals s.id into s1
                             from s2 in s1.DefaultIfEmpty()
                             where d.is_hidden == false && (payload.userAnnouncement.is_starred == null || d.is_starred == payload.userAnnouncement.is_starred) &&
                             (payload.userAnnouncement.is_read == null || d.is_read == payload.userAnnouncement.is_read) &&
                             (payload.userAnnouncement.is_starred == null || d.is_starred == payload.userAnnouncement.is_starred)
                             select new UserAnnouncementVM()
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
                             }).ToList().Where(a => a.sender_id == payload.userAnnouncement.sender_id || a.recipient_ids.Split(',').Contains(payload.userAnnouncement.sender_id.ToString())).OrderBy(a=> a.date_sent.ToDate());
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

        public Result<IEnumerable<UserAnnouncementVM>> Create(DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserAnnouncementVM>>();
            try
            {
                var recipientIds = new List<int>();
                if (param.userAnnouncement.department_ids.ToSafeString() != "" || param.userAnnouncement.recipient_ids.ToSafeString() != "")
                {
                    var rslts = db.AccessUser.Where(a => ("," + param.userAnnouncement.department_ids + ",").Contains("," + a.dept_id.ToString() + ",") == true || 
                    ("," + param.userAnnouncement.recipient_ids + ",").Contains("," + a.id.ToString() + ",") == true).Select(a => a.id);
                    recipientIds.AddRange(rslts.ToArray());
                }
                using (var db = new DatabaseContext())
                {
                    var itm = new UserAnnouncement()
                    {
                        sender_id = param.userAnnouncement.sender_id,
                        is_hidden = param.userAnnouncement.is_hidden,
                        is_read = param.userAnnouncement.is_read ?? false,
                        is_starred = param.userAnnouncement.is_starred ?? false,
                        message = param.userAnnouncement.message,
                        recipient_ids = String.Join(",", recipientIds),
                        date_sent = DateTime.Now,
                    };
                    db.UserAnnouncement.Add(itm);
                    db.SaveChanges();
                    param.userAnnouncement.sender_id = param.userAnnouncement.sender_id;
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

        public Result<IEnumerable<UserAnnouncementVM>> Update([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserAnnouncementVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.updated_date = DateTime.Now;
                    var itm = db.UserAnnouncement.Find(param.userAnnouncement.id);
                    itm.sender_id = param.userAnnouncement.sender_id;
                    itm.is_hidden = param.userAnnouncement.is_hidden;
                    itm.is_read = param.userAnnouncement.is_read ?? false;
                    itm.is_starred = param.userAnnouncement.is_starred ?? false;
                    itm.message = param.userAnnouncement.message;
                    itm.recipient_ids = param.userAnnouncement.recipient_ids;
                    itm.date_sent = DateTime.Now;
                    db.UserAnnouncement.AddOrUpdate(itm);
                    db.SaveChanges();
                    param.userAnnouncement.sender_id = itm.sender_id;
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
        public Result<IEnumerable<UserAnnouncementVM>> StarUnStar([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserAnnouncementVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.userAnnouncement.ids.Split(',').ToList().ForEach(id =>
                    {
                        param.updated_date = DateTime.Now;
                        var itm = db.UserAnnouncement.Find(id.ToSafeInt());
                        itm.is_starred = param.userAnnouncement.process == Constant.TransactionType.Star;
                        db.UserAnnouncement.AddOrUpdate(itm);
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
        public Result<IEnumerable<UserAnnouncementVM>> ReadUnRead([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserAnnouncementVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.userAnnouncement.ids.Split(',').ToList().ForEach(id =>
                    {
                        param.updated_date = DateTime.Now;
                        var itm = db.UserAnnouncement.Find(id.ToSafeInt());
                        itm.is_read = param.userAnnouncement.process == Constant.TransactionType.Read;
                        db.UserAnnouncement.AddOrUpdate(itm);
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
        public Result<IEnumerable<UserAnnouncementVM>> Hide([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<UserAnnouncementVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.userAnnouncement.ids.Split(',').ToList().ForEach(id =>
                    {
                        param.updated_date = DateTime.Now;
                        var itm = db.UserAnnouncement.Find(id.ToSafeInt());
                        itm.is_hidden = true;
                        db.UserAnnouncement.AddOrUpdate(itm);
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