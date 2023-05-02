using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Models.Shared;
using OPBids.Service.Utilities;
using OPBids.Entities.View.Shared;
using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;

namespace OPBids.Service.Logic.ActivityLog
{
    
    public class ActivityLogLogic : ApiController
    {

        private DatabaseContext _db;
        public ActivityLogLogic()
        {
            _db = new DatabaseContext();
        }

        public IEnumerable<ActivityLogModel> GetActivityLogByUserId(Payload payload)
        {
            var _result = new List<ActivityLogModel>();

            try
            {
                //var  = new ActivityLogModel { Action = "a6677678687687" };

                var log = _db.ActivityLog.Where(x => x.UserName == payload.auth_x_un)
                    .OrderByDescending(o => o.DateTime);

                //List<ActivityLogModel> ls = new List<ActivityLogModel>();

                foreach (var itm in log)
                {
                   

                    _result.Add(new ActivityLogModel
                    {
                       
                         Action = itm.Action
                         , Activities = itm.Activities
                         , DateTime = itm.DateTime
                         , FullName = itm.FullName
                         , IPAddress = itm.IPAddress
                         , Module = itm.Module
                         , Type = itm.Type
                         , UserId = itm.UserId
                         , UserName = itm.UserName
                    });
                }

               

                return _result;
                // return _result;
            }
            catch (Exception ex)
            {

            }

            return null;

            //return _result;
        }

        
        public Result<bool> InsertActivityLog(ActivityLogModel model)
        {
            var _result = new Result<bool>();

            //var _result = new Result<IEnumerable<DocumentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                  

                    var itm = new Models.ActivityLog.ActivityLog
                    {
                         Action = model.Action
                         , Activities = model.Activities
                         , DateTime = DateTime.Now
                         , FullName = model.FullName
                         , IPAddress = model.IPAddress
                         , Module = model.Module
                         , RecordId = model.RecordId
                         , UserId = model.UserId
                         , Type = model.Type
                         , UserName = model.UserName
                         , created_date = DateTime.Now
                         , id = 0
                         , updated_by = 0
                         , created_by = 0
                         , updated_date = DateTime.Now
                         
                    };

                    db.ActivityLog.Add(itm);
                    db.SaveChanges();
                    _result.value = true;
                    
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
            finally
            {
                //
            }


            return _result;

        }
    }

}
