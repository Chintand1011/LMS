using OPBids.Common;
using OPBids.Entities.Common;
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
    public class WorkflowLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<Workflow>> GetWorkflow(Payload payload)
        {
            var _result = new Result<IEnumerable<Workflow>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from flows in db.Workflows
                                 where flows.record_status == Constant.RecordStatus.Active
                                 select flows).ToList();
            }
            else
            {
                _result.value = (from flows in db.Workflows
                                 where flows.project_status.ToLower().Contains(payload.search_key.ToLower())
                                 && flows.record_status == Constant.RecordStatus.Active
                                 select flows).ToList();
            }
            //_result.total_count = _result.value.Count();
            //if (payload.page_index != -1)
            //{
            //    _result.page_count = _result.value.Count().GetPageCount();
            //    _result.value = _result.value.Skip(Constant.AppSettings.PageItemCount * payload.page_index).
            //                         Take(Constant.AppSettings.PageItemCount);
            //}
            return _result;
        }

        public Result<IEnumerable<Workflow>> CreateWorkflow(Workflow Workflow)
        {
            var _result = new Result<IEnumerable<Workflow>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    Workflow.created_date = DateTime.Now;
                    Workflow.updated_date = DateTime.Now;

                    db.Workflows.Add(Workflow);
                    db.SaveChanges();

                    //Select all records
                    _result = GetWorkflow(new Payload() { });
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

        public Result<IEnumerable<Workflow>> UpdateWorkflow([FromBody] Workflow Workflow)
        {
            var _result = new Result<IEnumerable<Workflow>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    Workflow.updated_date = DateTime.Now;

                    db.Workflows.AddOrUpdate(Workflow);
                    db.SaveChanges();
                    _result = GetWorkflow(new Payload() { });
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


        public Result<IEnumerable<Workflow>> StatusUpdateWorkflow([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<Workflow>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (var id in payload.item_list)
                        {
                            var _Workflow = db.Workflows.Find(Convert.ToInt32(id));
                            _Workflow.record_status = payload.status;
                            _Workflow.updated_date = DateTime.Now;
                            _Workflow.updated_by = payload.user_id;
                            db.Workflows.AddOrUpdate(_Workflow);
                        }
                    }
                    db.SaveChanges();
                    _result = GetWorkflow(new Payload() { page_index = payload.page_index });
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

        public Workflow GetNextWorkflow(DatabaseContext store, string type, string project_substatus, bool next = true, int step = 1) {
            Workflow nextFlow = new Workflow();

            var _workflow = (from wf in store.Workflows
                             where ((type != null && wf.type == type) || (wf.type == Constant.WorkflowType.Base) || (type == null)) && wf.record_status == Constant.RecordStatus.Active
                            orderby wf.seq_no
                            select wf).ToList();
            if (_workflow != null && _workflow.Count() > 0)
            {
                if (project_substatus == null)
                {
                    nextFlow = _workflow.First();
                }
                else
                {
                    var curr_index = _workflow.ToList().FindIndex(x => x.project_substatus == project_substatus);
                    if (next)
                    {
                        // Get Next work flow
                        nextFlow = _workflow.ToList()[curr_index + step];
                    }
                    else {
                        // Return to previous flow
                        nextFlow = _workflow.ToList()[curr_index - step];
                    }
                    
                }
            }

            return nextFlow;
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