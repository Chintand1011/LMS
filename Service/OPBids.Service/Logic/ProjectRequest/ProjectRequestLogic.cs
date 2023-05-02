using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Service.Data;
using OPBids.Service.Models.Settings;
using OPBids.Service.Models.ProjectRequest;
using OPBids.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;
using System.Globalization;
using OPBids.Entities.View.Setting;
using System.Data.Entity.SqlServer;
using Newtonsoft.Json.Linq;

namespace OPBids.Service.Logic
{
    public class ProjectRequestLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();


        #region Project Request

        public Result<ProjectSearchResultVM> GetProjectRequest(PayloadVM payload)
        {
            //Result<IEnumerable<ProjectRequestVM>> _result =
            //                                    new Result<IEnumerable<ProjectRequestVM>>();
            //List<ProjectRequestVM> list = new List<ProjectRequestVM>();
            //_result.value = this.PrepareViewResult(db, payload);
            //return _result;

            Result<ProjectSearchResultVM> _result = new Result<ProjectSearchResultVM>();
            _result.value = this.PrepareViewResult(db, payload);
            return _result;
        }

        public Result<PayloadVM> GetProjectRequestInformation(PayloadVM payload)
        {
            Result<PayloadVM> _result = new Result<PayloadVM>() { value = new PayloadVM() };

            using (DatabaseContext db = new DatabaseContext())
            {
                payload.projectSearch = new ProjectSearchVM();

                // Codes Tables
                var ct_proj_cat = db.ProjectCategory.Where(pc => pc.status == Constant.RecordStatus.Active);
                var ct_workflow = db.Workflows.Where(wf => wf.record_status == Constant.RecordStatus.Active);

                // ProjectRequest                
                var pr = (from _pr in db.ProjectRequests
                          join _au in db.AccessUser on _pr.created_by equals _au.id
                          join _cat in ct_proj_cat on _pr.category equals _cat.id.ToString()
                          join _wf in ct_workflow on _pr.project_substatus equals _wf.project_substatus
                          where _pr.id == payload.id
                          select new { _pr, creator = _au.first_name + " " + _au.last_name, category = _cat.proj_cat, project_subStatus = _wf.project_substatus_desc }).FirstOrDefault();
                var prView = pr._pr.ToView();
                prView.created_by_name = pr.creator;
                prView.category_desc = pr.category;
                prView.project_substatus_desc = pr.project_subStatus;

                _result.value.projectRequest = prView;

                // ProjectRequestItems
                db.ProjectRequestItems.Where(pri => pri.project_id == payload.id).ToList().ForEach(pril =>
                {
                    _result.value.projectItems = new List<ProjectRequestItemVM>();
                    if (pril != null)
                    {
                        _result.value.projectItems.Add(pril.ToView());
                    }
                });

                // ProjectRequestBatch
                var batch = (from _prb in db.ProjectRequestBatch
                             join _pr in db.ProjectRequests on _prb.id equals _pr.batch_id
                             join bpm in db.ProcurementMethod on _prb.procurement_method equals bpm.proc_code into bpmt
                             from procMethod in bpmt.DefaultIfEmpty()

                             where _pr.id == payload.id
                             select new { _prb, procurement_method = procMethod.procurement_description, procMethod.procurement_mode }).FirstOrDefault();
                if (batch != null)
                {
                    _result.value.projectRequestBatch = batch._prb.ToView(batch.procurement_mode);
                    _result.value.projectRequest.procurement_method = batch.procurement_method;
                }

                // ProjectBids
                var bids = (from _pb in db.ProjectBid
                            join _s in db.Supplier on _pb.created_by equals _s.user_id
                            where _pb.project_request_id == payload.id
                            select new { _pb, sup_name = _s.comp_name, sup_address = _s.address, sup_con_person = _s.contact_person, sup_con_num = _s.contact_no }).ToList();
                if (bids.Count() > 0)
                {
                    bids.ForEach(i => {
                        if (_result.value.projectBids == null) { _result.value.projectBids = new List<ProjectBidVM>(); };
                        if (i._pb != null)
                        {
                            var _vm = new ProjectBidVM();
                            _vm = i._pb.ToView(i.sup_name, i.sup_address, i.sup_con_person);
                            _result.value.projectBids.Add(_vm);

                            // Awarded Supplier Bid
                            if (i._pb.bid_status == Constant.BidStatus.Awarded)
                            {
                                _result.value.awardedBid = _vm;
                            }
                        }
                    });
                }

                // ProjectRequestHistory
                payload.projectSearch.id = payload.id.ToString();
                _result.value.projectRequestHistories = new Controllers.Shared.SharedController().GetProjectLogs(payload).value.ToList();

                // Progress
                _result.value.progressList = new Shared.SharedLogic().GetProjectProgress(payload).value;
            }

            return _result;
        }

        public Result<ProjectSearchResultVM> SearchProjectRequest(PayloadVM payload)
        {
            Result<ProjectSearchResultVM> _result = new Result<ProjectSearchResultVM>();
            _result.value = this.SearchViewResult(db, payload);
            return _result;
        }

        public Result<int> CreateProjectRequest(PayloadVM payload)
        {
            Result<int> _result = new Result<int>();
            try
            {
                ProjectRequest request = payload.projectRequest.ToDomain();

                using (DatabaseContext db = new DatabaseContext())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            int granteeId = 0;
                            if (int.TryParse(request.grantee, out granteeId) == false)
                            {
                                var granteeObj = JObject.Parse(request.grantee);

                                var key = granteeObj["k"];
                                var val = granteeObj["v"];
                                if (key == null || val == null)
                                {
                                    _result.status = new Status()
                                    {
                                        code = Constant.Status.Failed,
                                        description = "Invalid Grantee"
                                    };
                                    return _result;
                                }

                                var newGrantee = new ProjectGrantee()
                                {

                                    id = 0,
                                    grantee_code = (string)val,
                                    grantee_name = (string)val,
                                    status = Constant.RecordStatus.Active,
                                    created_by = request.updated_by,
                                    created_date = DateTime.Now,
                                    updated_by = request.updated_by,
                                    updated_date = DateTime.Now
                                };

                                db.ProjectGrantees.AddOrUpdate(newGrantee);
                                db.SaveChanges();
                                request.grantee = newGrantee.id.ToString();

                                //int lat = (int)json["results"]["geometry"]["location"]["lat"];
                            }


                            // Set Workflow
                            Models.Settings.Workflow workflow = new Settings.WorkflowLogic().GetNextWorkflow(db, Constant.WorkflowType.Base, null);
                            request.project_status = workflow.project_status;
                            request.project_substatus = workflow.project_substatus;
                            request.sla = workflow.sla;

                            request.created_date = DateTime.Now;
                            request.updated_date = DateTime.Now;
                            db.ProjectRequests.Add(request);

                            db.SaveChanges();


                            if (payload.projectItems != null)
                            {
                                foreach (var item in payload.projectItems)
                                {
                                    item.project_id = request.id;
                                    item.created_by = request.updated_by;
                                    item.updated_by = request.updated_by;
                                    var entity = item.ToDomain();

                                    db.ProjectRequestItems.AddOrUpdate(entity);

                                }
                            }
                            if (payload.documentAttachments != null)
                            {
                                payload.documentAttachments.ForEach(a =>
                                {

                                    a.project_id = request.id;
                                    a.created_by = payload.projectRequest.updated_by;
                                    a.created_date = null;
                                    a.updated_by = payload.projectRequest.updated_by;
                                    a.updated_date = null;

                                    var docAtt = a.ToDomain();

                                    if (a.process == Constant.RecordStatus.Cancelled)
                                    {
                                        var entity = new ProjectRequestAttachment() { id = docAtt.id };
                                        db.ProjectRequestAttachments.Attach(entity);
                                        db.ProjectRequestAttachments.Remove(entity);
                                    }
                                    else if (a.process.Has(Constant.TransactionType.Update, Constant.TransactionType.Save, Constant.TransactionType.Create))
                                    {
                                        var entity = new ProjectRequestAttachment() { id = docAtt.id };
                                        db.ProjectRequestAttachments.Attach(entity);
                                        entity.status = Constant.RecordStatus.Active;
                                        entity.file_name = a.file_name;
                                        entity.attachment_name = a.attachment_name;
                                        entity.project_id = request.id;
                                        entity.barcode_no = docAtt.barcode_no;
                                        entity.updated_by = docAtt.updated_by;
                                        entity.updated_date = DateTime.Now;
                                        db.ProjectRequestAttachments.AddOrUpdate(entity);

                                    }
                                });
                            }
                            db.SaveChanges();
                            dbTransaction.Commit();
                            _result.value = request.id;
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            throw;
                        }
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
            }
            return _result;
        }

        public Result<int> UpdateProjectRequest([FromBody] PayloadVM payload)
        {
            Result<int> _result = new Result<int>();
            try
            {
                ProjectRequest request = payload.projectRequest.ToDomain();

                using (DatabaseContext db = new DatabaseContext())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {

                        try
                        {
                            var projReq = db.ProjectRequests.Find(request.id);
                            if (projReq != null)
                            {
                                // Save History
                                var _history = projReq.ToHistory();
                                _history.change_log = "Information Change";
                                // _history.notes = request.notes;
                                _history.created_by = request.updated_by;
                                _history.created_date = DateTime.Now;
                                _history.updated_by = request.updated_by;
                                _history.updated_date = DateTime.Now;
                                db.ProjectRequestsHistory.Add(_history);

                                projReq.updated_by = request.updated_by;
                                projReq.updated_date = request.updated_date;
                                projReq.notes = String.IsNullOrEmpty(request.notes) ? projReq.notes : request.notes;

                                // Only update editable columns of each module
                                if (projReq.project_status == Constant.ProjectRequest.ProjectStatus.Draft ||
                                    projReq.project_status == Constant.ProjectRequest.ProjectStatus.NewlyRequested)
                                {

                                    int granteeId = 0;
                                    if (int.TryParse(request.grantee, out granteeId) == false)
                                    {
                                        var granteeObj = JObject.Parse(request.grantee);

                                        var key = granteeObj["k"];
                                        var val = granteeObj["v"];
                                        if (key == null || val == null)
                                        {
                                            _result.status = new Status()
                                            {
                                                code = Constant.Status.Failed,
                                                description = "Invalid Grantee"
                                            };
                                            return _result;
                                        }

                                        var newGrantee = new ProjectGrantee()
                                        {

                                            id = 0,
                                            grantee_code = (string)val,
                                            grantee_name = (string)val,
                                            status = Constant.RecordStatus.Active,
                                            created_by = request.updated_by,
                                            created_date = DateTime.Now,
                                            updated_by = request.updated_by,
                                            updated_date = DateTime.Now
                                        };

                                        db.ProjectGrantees.AddOrUpdate(newGrantee);
                                        db.SaveChanges();
                                        request.grantee = newGrantee.id.ToString();

                                        //int lat = (int)json["results"]["geometry"]["location"]["lat"];
                                    }


                                    projReq.title = request.title;
                                    projReq.description = request.description;
                                    projReq.grantee = request.grantee;
                                    projReq.estimated_budget = request.estimated_budget;
                                    projReq.required_date = request.required_date;
                                    projReq.category = request.category;
                                    projReq.pr_number = request.pr_number;

                                    if (projReq.project_status == Constant.ProjectRequest.ProjectStatus.NewlyRequested)
                                    {
                                        projReq.approved_budget = request.estimated_budget;
                                        projReq.classification = request.classification;
                                        projReq.contract_type = request.contract_type;
                                        projReq.security_level = request.security_level;
                                        projReq.delivery_type = request.delivery_type;
                                    }
                                }
                                if (projReq.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval)
                                {
                                    projReq.approved_budget = request.approved_budget;
                                    projReq.earmark = request.earmark;
                                    projReq.earmark_date = request.earmark_date;
                                    projReq.source_fund = request.source_fund;

                                    //Update Project Staus
                                    UpdateProjectStatus(payload);
                                }
                                if (projReq.project_substatus == Constant.ProjectRequest.ProjectSubStatus.RFQ_Preparation)
                                {
                                    projReq.rfq_deadline = request.rfq_deadline;
                                    projReq.rfq_place = request.rfq_place;
                                    projReq.rfq_requestor = request.rfq_requestor;
                                    projReq.rfq_requestor_dept = request.rfq_requestor_dept;
                                    projReq.rfq_request_date = request.rfq_request_date;
                                }

                                if (payload.projectItems != null)
                                {
                                    foreach (var item in payload.projectItems)
                                    {
                                        if (item.process == Constant.TransactionType.Delete)
                                        {
                                            if (item.id != 0)
                                            {
                                                var entity = new ProjectRequestItem() { id = item.id };
                                                db.ProjectRequestItems.Attach(entity);
                                                db.ProjectRequestItems.Remove(entity);
                                            }
                                        }
                                        else //add or update
                                        {

                                            var entity = db.ProjectRequestItems.Find(item.id);

                                            if (entity != null)
                                            {
                                                entity.unit = item.unit;
                                                entity.description = item.description;
                                                entity.quantity = item.quantity.ToSafeInt();
                                                entity.unit_cost = item.unit_cost.ToSafeDecimal();

                                                entity.updated_by = request.updated_by;
                                                entity.updated_date = DateTime.Now;
                                            }
                                            else
                                            {
                                                item.project_id = request.id;
                                                item.created_by = request.updated_by;
                                                item.updated_by = request.updated_by;
                                                entity = item.ToDomain();
                                            }

                                            db.ProjectRequestItems.AddOrUpdate(entity);
                                        }
                                    }
                                }

                            }


                            if (payload.documentAttachments != null)
                            {
                                payload.documentAttachments.ForEach(a =>
                                {

                                    a.project_id = payload.projectRequest.id;
                                    a.created_by = payload.projectRequest.updated_by;
                                    a.created_date = payload.projectRequest.updated_date;
                                    a.updated_by = payload.projectRequest.updated_by;
                                    a.updated_date = payload.projectRequest.updated_date;

                                    var docAtt = a.ToDomain();

                                    if (a.process == Constant.RecordStatus.Cancelled)
                                    {
                                        var entity = new ProjectRequestAttachment() { id = docAtt.id };
                                        db.ProjectRequestAttachments.Attach(entity);
                                        db.ProjectRequestAttachments.Remove(entity);
                                    }
                                    else if (a.process.Has(Constant.TransactionType.Update, Constant.TransactionType.Save, Constant.TransactionType.Create))
                                    {
                                        var entity = new ProjectRequestAttachment() { id = docAtt.id };
                                        db.ProjectRequestAttachments.Attach(entity);
                                        entity.status = Constant.RecordStatus.Active;
                                        entity.file_name = a.file_name;
                                        entity.attachment_name = a.attachment_name;
                                        entity.project_id = payload.projectRequest.id;
                                        entity.barcode_no = docAtt.barcode_no;//from project requests; only barcode is editable
                                        entity.updated_by = docAtt.updated_by;
                                        entity.updated_date = docAtt.updated_date;
                                        db.ProjectRequestAttachments.AddOrUpdate(entity);

                                    }
                                });
                            }

                            db.SaveChanges();
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError(ex, "update project request");
                            dbTransaction.Rollback();
                            throw;
                        }
                        _result.value = request.id;
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
            }
            return _result;
        }

        public Result<int> UpdateProjectStatus([FromBody] PayloadVM payload)
        {
            Result<int> _result = new Result<int>();
            try
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var _request = db.ProjectRequests.Find(Convert.ToInt32(payload.projectRequest.id));

                    // Save History
                    ProjectRequestHistory _history = _request.ToHistory();
                    _history.change_log = "Project Status Change";
                    _history.created_by = payload.projectRequest.updated_by;
                    _history.created_date = DateTime.Now;
                    _history.updated_by = payload.projectRequest.updated_by;
                    _history.updated_date = DateTime.Now;
                    db.ProjectRequestsHistory.Add(_history);

                    _request.updated_date = DateTime.Now;
                    _request.notes = payload.projectRequest.notes;
                    _request.updated_by = payload.projectRequest.updated_by;
                    _request.current_user = payload.projectRequest.current_user;

                    Models.Settings.Workflow workflow = null;

                    // Get current workflow type
                    // Procurement Method is found only after Project Batch planning
                    string wf_type = null;
                    if (_request.batch_id > 0)
                    {
                        var _type = (from _prb in db.ProjectRequestBatch
                                     join _pm in db.ProcurementMethod on _prb.procurement_method equals _pm.proc_code
                                     where _prb.id == _request.batch_id
                                     select _pm.procurement_mode).First();
                        if (_type != null)
                        {
                            wf_type = _type;
                        }
                    }

                    // Update Workflow if status is Active
                    if (payload.projectRequest.user_action == Constant.UserAction.Approve ||
                        payload.projectRequest.user_action == Constant.UserAction.Received)
                    {
                        if (_request.project_status == Constant.ProjectRequest.ProjectStatus.Draft)
                        {
                            _request.submitted_date = DateTime.Now;
                        }

                        workflow = new Settings.WorkflowLogic().GetNextWorkflow(db, wf_type, _request.project_substatus);
                    }
                    else if (payload.projectRequest.user_action == Constant.UserAction.Return)
                    {
                        workflow = new Settings.WorkflowLogic().GetNextWorkflow(db, wf_type, _request.project_substatus, false);
                    }
                    else if (payload.projectRequest.user_action == Constant.UserAction.UpdateImplementationSatus)
                    {
                        // Update Implementation Status of the Project
                        var wf = new Settings.WorkflowLogic().GetWorkflow(new Payload());
                        if (wf.value.Count() > 0)
                        {
                            workflow = wf.value.Where(x => x.project_status == payload.projectRequest.project_status && x.project_substatus == payload.projectRequest.project_substatus).First();
                        }
                    }
                    if (workflow != null)
                    {
                        _request.project_status = workflow.project_status;
                        _request.project_substatus = workflow.project_substatus;
                        _request.sla = workflow.sla;
                    }

                    db.ProjectRequests.AddOrUpdate(_request);

                    db.SaveChanges();

                    _result.value = _request.id;
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

        public Result<string[]> UpdateRecordStatus([FromBody] PayloadVM payload)
        {
            Result<string[]> _result = new Result<string[]>();
            try
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _request = db.ProjectRequests.Find(Convert.ToInt32(id));

                            // Save History
                            ProjectRequestHistory _history = _request.ToHistory();
                            _history.change_log = "Record Status Change";
                            if (!string.IsNullOrEmpty(payload.projectRequest.user_action))
                            {
                                _history.user_action = this.GetUserAction(payload.projectRequest.user_action);

                                if (payload.projectRequest.user_action == Constant.UserAction.Received)
                                {
                                    _history.change_log = "Received Documents";
                                }
                            }
                            _history.notes = payload.projectRequest.notes;
                            _history.created_by = payload.projectRequest.updated_by;
                            _history.created_date = DateTime.Now;
                            _history.updated_by = payload.projectRequest.updated_by;
                            _history.updated_date = DateTime.Now;
                            db.ProjectRequestsHistory.Add(_history);

                            _request.updated_date = DateTime.Now;
                            _request.updated_by = payload.projectRequest.updated_by;
                            _request.record_status = !string.IsNullOrEmpty(payload.projectRequest.record_status) ? payload.projectRequest.record_status : _request.record_status;


                            db.ProjectRequests.AddOrUpdate(_request);
                        }
                    }
                    db.SaveChanges();

                    _result.value = payload.item_list;
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

        private ProjectSearchResultVM PrepareViewResult(DatabaseContext store, PayloadVM payload)
        {
            try
            {


                ProjectSearchResultVM _result = new ProjectSearchResultVM();

                List<ProjectRequestVM> list = new List<ProjectRequestVM>();

                var predicate = PredicateBuilder.True<ProjectRequest>();
                var param = payload.projectSearch;

                // Active records
                predicate = predicate.And(p => p.record_status == Constant.RecordStatus.Active);

                // Project Status
                if (!string.IsNullOrWhiteSpace(param.project_status))
                {
                    var statusPredicate = PredicateBuilder.False<ProjectRequest>();

                    param.project_status.Split(',').ToList().ForEach(i =>
                        statusPredicate = statusPredicate.Or(p => p.project_status == i)
                        );

                    predicate = predicate.And(statusPredicate);

                    //predicate = predicate.And(p => p.project_status == param.project_status);
                }

                // Project SubStatus
                if (!string.IsNullOrWhiteSpace(param.project_substatus))
                {
                    predicate = predicate.And(p => p.project_substatus == param.project_substatus);
                }

                // Project Request ID
                if (param.id.ToSafeInt() != 0)
                {
                    int intId = param.id.ToSafeInt();
                    predicate = predicate.And(p => p.id == intId);
                }

                // Project Batch ID
                if (!string.IsNullOrWhiteSpace(param.batch_id))
                {
                    predicate = predicate.And(p => p.batch_id.ToString() == param.batch_id);
                }

                var query = from proj in (store.ProjectRequests.Where(predicate))
                            join creator in store.AccessUser on proj.created_by equals creator.id
                            join update in store.AccessUser on proj.updated_by equals update.id
                            join workflow in store.Workflows on proj.project_substatus equals workflow.project_substatus

                            join cat in store.ProjectCategory on proj.category equals cat.id.ToString() into catt
                            from pcat in catt.DefaultIfEmpty()

                            join pc in store.ProjectClassification on proj.classification equals pc.id.ToString() into pct
                            from pclass in pct.DefaultIfEmpty()

                            join ct in store.ContractType on proj.contract_type equals ct.id.ToString() into ctt
                            from contract in ctt.DefaultIfEmpty()

                            join dsl in store.DocumentSecurityLevel on proj.security_level equals dsl.id into dslt
                            from seclevel in dslt.DefaultIfEmpty()

                            join d in store.Delivery on proj.delivery_type equals d.id into dt
                            from deltype in dt.DefaultIfEmpty()

                            join batch in store.ProjectRequestBatch on proj.batch_id equals batch.id into batcht
                            from projBatch in batcht.DefaultIfEmpty()

                            join bpm in store.ProcurementMethod on projBatch.procurement_method equals bpm.proc_code into bpmt
                            from procMethod in bpmt.DefaultIfEmpty()

                            orderby proj.id

                            select new
                            {
                                proj,
                                created_name = string.Concat(creator.first_name, " ", creator.mi, " ", creator.last_name),
                                updated_name = string.Concat(update.first_name, " ", update.mi, " ", update.last_name),
                                proj_status = workflow.project_status_desc,
                                proj_substatus = workflow.project_substatus_desc,
                                proj_category = pcat.proj_cat,
                                proj_classification = pclass.classification,
                                proj_contract = contract.contract_type,
                                proj_security = seclevel.description,
                                proj_delivery = deltype.delivery_code,
                                proj_procurement_method = procMethod.procurement_description
                            };


                _result.page_index = payload.projectSearch.page_index;
                _result.count = query.Count();

                if (query != null && _result.count > 0)
                {
                    int index = (payload.projectSearch.page_index - 1) * payload.projectSearch.page_size;
                    var paged = query.Skip(index).Take(payload.projectSearch.page_size).ToList();

                    paged.ForEach(qList =>
                    {
                        ProjectRequestVM _item = qList.proj.ToView();
                        _item.estimated_budget = _item.estimated_budget.ToSafeDecimal().ToString("N", CultureInfo.CurrentCulture);
                        _item.approved_budget = _item.approved_budget.ToSafeDecimal().ToString("N", CultureInfo.CurrentCulture);
                        _item.created_by_name = qList.created_name;
                        _item.updated_by_name = qList.updated_name;
                        _item.project_status_desc = qList.proj_status;
                        _item.project_substatus_desc = qList.proj_substatus;
                        _item.category_desc = qList.proj_category;
                        _item.classification_desc = qList.proj_classification;
                        _item.contract_type_desc = qList.proj_contract;
                        _item.security_level_desc = qList.proj_security;
                        _item.delivery_type_desc = qList.proj_delivery;
                        _item.procurement_method = qList.proj_procurement_method;
                        _item.attachments = GetAttachments(store, _item.id);
                        _item.index = ++index;
                        list.Add(_item);
                    });

                    if (param.get_total.GetValueOrDefault())
                    {
                        _result.total = query.Sum(r => r.proj.approved_budget);
                    }
                }
                _result.items = list;


                return _result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private ProjectSearchResultVM SearchViewResult(DatabaseContext store, PayloadVM payload)
        {
            try
            {
                List<ProjectRequestVM> list = new List<ProjectRequestVM>();
                ProjectSearchResultVM _result = new ProjectSearchResultVM();

                var param = payload.projectSearch;

                var predicate = PredicateBuilder.True<ProjectRequest>();
                var predicate_au = PredicateBuilder.True<AccessUser>();
                var predicate_wf = PredicateBuilder.True<Workflow>();
                var predicate_mp = PredicateBuilder.True<MonitoredProject>();

                if (string.IsNullOrEmpty(param.record_status)) { param.record_status = Constant.RecordStatus.Active; };
                predicate = predicate.And(p => p.record_status == param.record_status);

                if (param.id.ToSafeInt() != 0)
                {
                    int intId = param.id.ToSafeInt();
                    predicate = predicate.And(p => p.id == intId);
                }

                if (param.batch_id.ToSafeInt() != 0)
                {
                    predicate = predicate.And(p => p.batch_id == param.batch_id.ToSafeInt());
                }

                if (!string.IsNullOrEmpty(param.project_substatus_min) || !string.IsNullOrEmpty(param.project_substatus_max))
                {
                    int _minID, _maxID;
                    var workflowList = store.Workflows.Where(wf => wf.record_status == Constant.RecordStatus.Active);
                    if (!string.IsNullOrEmpty(param.project_substatus_min))
                    {
                        _minID = workflowList.Where(min => min.project_substatus == param.project_substatus_min).First().id;
                        predicate_wf = predicate_wf.And(p => p.id >= _minID);
                    }
                    if (!string.IsNullOrEmpty(param.project_substatus_max))
                    {
                        _maxID = workflowList.Where(max => max.project_substatus == param.project_substatus_max).First().id;
                        predicate_wf = predicate_wf.And(p => p.id <= _maxID);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(param.project_status))
                    {
                        if (param.project_status == "ONGOING")
                        {
                            predicate = predicate.And(p => p.project_status != Constant.ProjectRequest.ProjectStatus.Draft && p.project_status != Constant.ProjectRequest.ProjectStatus.Completed);
                        }
                        else if (param.project_status == "DELAYED")
                        {
                            DateTime currentDate = DateTime.Now.Date;
                            predicate = predicate.And(p => p.project_status != Constant.ProjectRequest.ProjectStatus.Draft && p.project_status != Constant.ProjectRequest.ProjectStatus.Completed && p.required_date < currentDate);
                        }
                        else if (param.project_status == "APPROVED")
                        {
                            predicate = predicate.And(p => p.project_status != Constant.ProjectRequest.ProjectStatus.Draft && p.project_status != Constant.ProjectRequest.ProjectStatus.NewlyRequested && p.project_status != Constant.ProjectRequest.ProjectStatus.ForBudgetApproval && p.project_status != Constant.ProjectRequest.ProjectStatus.Completed);
                        }
                        else
                        {
                            predicate = predicate.And(p => p.project_status == param.project_status);
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(param.project_substatus))
                    {
                        predicate = predicate.And(p => p.project_substatus == param.project_substatus);
                    }
                }


                if (param.submitted_from != null)
                {
                    predicate = predicate.And(p => p.submitted_date >= param.submitted_from);
                }
                if (param.submitted_to != null)
                {
                    var subToDate = param.submitted_to.Value.AddDays(1);
                    predicate = predicate.And(p => p.submitted_date < subToDate);
                }

                if (param.required_from != null)
                {
                    predicate = predicate.And(p => p.required_date >= param.required_from);
                }
                if (param.required_to != null)
                {
                    var reqToDate = param.required_to.Value.AddDays(1);
                    predicate = predicate.And(p => p.required_date < reqToDate);
                }

                if (param.budget_min != 0 && param.budget_max != 0)
                {
                    predicate = predicate.And(p => p.estimated_budget >= param.budget_min && p.estimated_budget <= param.budget_max);
                }
                else
                {
                    if (param.budget_min != 0)
                    {
                        predicate = predicate.And(p => p.estimated_budget >= param.budget_min);
                    }
                    if (param.budget_max != 0)
                    {
                        predicate = predicate.And(p => p.estimated_budget <= param.budget_max);
                    }
                }

                if (param.grantee != null)
                {
                    param.grantee = param.grantee.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    if (param.grantee.Count > 0)
                    {
                        predicate = predicate.And(p => param.grantee.Contains(p.grantee));
                    }
                }

                if (param.category != null)
                {
                    param.category = param.category.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    if (param.category.Count > 0)
                    {
                        predicate = predicate.And(p => param.category.Contains(p.category));
                    }
                }

                if (payload.projectSearch.current_user != 0)
                {
                    var _accessUser = store.AccessUser.Find(payload.projectSearch.current_user);
                    if (_accessUser != null)
                    {
                        predicate_au = predicate_au.And(p => p.dept_id == _accessUser.dept_id);
                    }
                }
                if (payload.monitoredProject != null && payload.monitoredProject.created_by > 0)
                {
                    predicate_mp = predicate_mp.And(mp => mp.created_by == payload.monitoredProject.created_by);
                }

                if (!string.IsNullOrWhiteSpace(param.project_name))
                {
                    predicate = predicate.And(p => p.title.Contains(param.project_name));
                }


                if (!string.IsNullOrWhiteSpace(param.dashboard_option))
                {
                    switch (param.dashboard_option)
                    {
                        case "DSI-UILESS10":
                            predicate = predicate.And(p => p.project_substatus == "PSS-16.1" || p.project_substatus == "PSS-16.1");
                            break;
                        case "DSI-LESS50":
                            predicate = predicate.And(p => p.project_substatus == "PSS-16.2" || p.project_substatus == "PSS-16.3" || p.project_substatus == "PSS-16.4" || p.project_substatus == "PSS-16.5");
                            break;
                        case "DSI-50-80":
                            predicate = predicate.And(p => p.project_substatus == "PSS-16.6" || p.project_substatus == "PSS-16.61" || p.project_substatus == "PSS-16.62");
                            break;
                        case "DSI-ABOVE80":
                            predicate = predicate.And(p => p.project_substatus == "PSS-16.63");
                            break;

                        case "DSI-UTWG":
                            predicate = predicate.And(p => (
                                p.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Eval ||
                                p.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_ForPostQual ||
                                p.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_LCB ||
                                p.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_PostEval ||
                                p.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_PostQual ||
                                p.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Ranking)
                                );
                            break;
                        case "DSI-NEWREQ":
                            predicate = predicate.And(p => p.project_status == Constant.ProjectRequest.ProjectStatus.NewlyRequested);
                            break;
                        case "DSI-BUDAPP":
                            predicate = predicate.And(p => p.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval);
                            break;
                        case "DSI-PREBID":
                            predicate = predicate.And(p => (
                                p.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_BudgetApproved ||
                                p.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_Hope ||
                                p.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_BACSEC ||
                                p.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_Posting ||
                                p.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_Close_ReOpen)
                                );
                            break;


                        case "DSI-PAST10":

                            break;

                            //case "DSI-REJ":

                            //    break;
                    }
                }

                var query = from proj in (store.ProjectRequests.Where(predicate))
                            join creator in store.AccessUser.Where(predicate_au) on proj.created_by equals creator.id
                            join update in store.AccessUser on proj.updated_by equals update.id
                            join workflow in store.Workflows.Where(predicate_wf) on proj.project_substatus equals workflow.project_substatus

                            join cat in store.ProjectCategory on proj.category equals cat.id.ToString() into catt
                            from pcat in catt.DefaultIfEmpty()

                            join pc in store.ProjectClassification on proj.classification equals pc.id.ToString() into pct
                            from pclass in pct.DefaultIfEmpty()

                            join ct in store.ContractType on proj.contract_type equals ct.id.ToString() into ctt
                            from contract in ctt.DefaultIfEmpty()

                            join dsl in store.DocumentSecurityLevel on proj.security_level equals dsl.id into dslt
                            from seclevel in dslt.DefaultIfEmpty()

                            join d in store.Delivery on proj.delivery_type equals d.id into dt
                            from deltype in dt.DefaultIfEmpty()

                            join g in store.ProjectGrantees on proj.grantee equals g.id.ToString() into gt
                            from pg in gt.DefaultIfEmpty()

                            join batch in store.ProjectRequestBatch on proj.batch_id equals batch.id into batcht
                            from projBatch in batcht.DefaultIfEmpty()

                            join bpm in store.ProcurementMethod on projBatch.procurement_method equals bpm.proc_code into bpmt
                            from procMethod in bpmt.DefaultIfEmpty()

                            join mp in store.MonitoredProjects.Where(predicate_mp) on proj.id equals mp.project_request_id into bmpt
                            from monitor in bmpt.DefaultIfEmpty()

                            orderby proj.id

                            select new
                            {
                                proj,
                                created_name = string.Concat(creator.first_name, " ", creator.mi, " ", creator.last_name),
                                updated_name = string.Concat(update.first_name, " ", update.mi, " ", update.last_name),
                                proj_status = workflow.project_status_desc,
                                proj_substatus = workflow.project_substatus_desc,
                                proj_category = pcat.proj_cat,
                                proj_classification = pclass.classification,
                                proj_contract = contract.contract_type,
                                proj_security = seclevel.description,
                                proj_delivery = deltype.delivery_code,
                                proj_grantee = pg.grantee_name,
                                proj_procurement_method = procMethod.procurement_description,
                                proj_bid_opening_place = projBatch.bid_opening_place,
                                proj_bid_opening_date = projBatch.pre_bid_date,
                                isMonitored = monitor.project_request_id > 0 ? true : false
                            };


                _result.page_index = payload.projectSearch.page_index;
                _result.count = query.Count();

                if (query != null && _result.count > 0)
                {
                    int index = (payload.projectSearch.page_index - 1) * payload.projectSearch.page_size;
                    var paged = query.Skip(index).Take(payload.projectSearch.page_size).ToList();
                    paged.ForEach(qList =>
                    {
                        ProjectRequestVM _item = qList.proj.ToView();
                        _item.estimated_budget = _item.estimated_budget.ToSafeDecimal().ToString("N", CultureInfo.CurrentCulture);
                        _item.approved_budget = _item.approved_budget.ToSafeDecimal().ToString("N", CultureInfo.CurrentCulture);
                        _item.created_by_name = qList.created_name;
                        _item.updated_by_name = qList.updated_name;
                        _item.project_status_desc = qList.proj_status;
                        _item.project_substatus_desc = qList.proj_substatus;
                        _item.category_desc = qList.proj_category;
                        _item.classification_desc = qList.proj_classification;
                        _item.contract_type_desc = qList.proj_contract;
                        _item.security_level_desc = qList.proj_security;
                        _item.delivery_type_desc = qList.proj_delivery;
                        _item.attachments = GetAttachments(store, _item.id);
                        _item.grantee_name = qList.proj_grantee;
                        _item.procurement_method = qList.proj_procurement_method;
                        _item.bid_opening_place = qList.proj_bid_opening_place;
                        _item.bid_opening_date = qList.proj_bid_opening_date?.ToString(Constant.DateFormat);
                        _item.isMonitored = qList.isMonitored;
                        _item.index = ++index;
                        list.Add(_item);
                    });


                }

                if (param.get_total.GetValueOrDefault())
                {
                    _result.total = query.Sum(r => (decimal?)r.proj.estimated_budget).GetValueOrDefault();
                }

                _result.items = list;


                return _result;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "search delayed");
                throw;
            }
        }

        public Result<IEnumerable<ProjectRequestVM>> GetProjectInfo(PayloadVM payload)
        {
            Result<IEnumerable<ProjectRequestVM>> _result = new Result<IEnumerable<ProjectRequestVM>>();
            List<ProjectRequestVM> lstProj = new List<ProjectRequestVM>();

            var query = (from pi in db.ProjectRequests
                         join bid in db.ProjectBid on pi.id equals bid.project_request_id

                         join supp in db.Supplier on bid.created_by equals supp.id

                         join cat in db.ProjectCategory on pi.category equals cat.id.ToString() into catt
                         from pcat in catt.DefaultIfEmpty()

                         join user in db.AccessUser on pi.created_by equals user.id into ausers
                         from auser in ausers.DefaultIfEmpty()

                         join dept in db.Departments on auser.dept_id equals dept.id into dep
                         from deptmt in dep.DefaultIfEmpty()

                         where pi.id == payload.id && bid.bid_status == "A"
                         select new
                         {
                             pi,
                             Cate_Desc = pcat.proj_cat,
                             EndUser = string.Concat(auser.first_name, " ", auser.mi, " ", auser.last_name),
                             Dept = deptmt.dept_description,
                             BidderName = supp.comp_name,
                             Address = supp.address
                         });
            var count = query.Count();
            if (query != null && count > 0)
            {
                query.ToList().ForEach(qlist =>
                {
                    ProjectRequestVM _item = qlist.pi.ToView();
                    _item.category_desc = qlist.Cate_Desc;
                    _item.endUser = qlist.EndUser;
                    _item.departmentName = qlist.Dept;
                    _item.bidderName = qlist.BidderName;
                    _item.supplierAddr = qlist.Address;
                    lstProj.Add(_item);
                });
            }

            _result.value = lstProj;
            return _result;
        }
        #endregion



        #region Project Request Batch 

        public Result<IEnumerable<ProjectRequestBatchVM>> GetProjectRequestBatch(PayloadVM payload)
        {
            Result<IEnumerable<ProjectRequestBatchVM>> _result =
                                                new Result<IEnumerable<ProjectRequestBatchVM>>();
            List<ProjectRequestBatchVM> list = new List<ProjectRequestBatchVM>();
            _result.value = this.PrepareViewResultBatch(db, payload);
            return _result;
        }

        public Result<ProjectBatchSearchResultVM> SearchProjectRequestBatch(PayloadVM payload)
        {
            Result<ProjectBatchSearchResultVM> _result = new Result<ProjectBatchSearchResultVM>();
            _result.value = this.SearchViewResultBatch(db, payload);
            return _result;
        }

        public Result<IEnumerable<ProjectRequestVM>> GetProjectRequestListForBatch(PayloadVM payload)
        {
            List<ProjectRequestVM> list = new List<ProjectRequestVM>();

            var predicate = PredicateBuilder.True<ProjectRequest>();
            var param = payload.projectSearch;

            // Project Batch ID
            if (!string.IsNullOrWhiteSpace(param.batch_id))
            {
                if (param.no_batch_id)
                {
                    // Include records with no Batch_Id and is available for consolidation
                    predicate = predicate.And(p => (p.batch_id.ToString() == param.batch_id) || (p.batch_id == 0 && p.project_substatus == param.project_substatus));
                    predicate = predicate.And(p => p.record_status == Constant.RecordStatus.Active);
                }
                else
                {
                    predicate = predicate.And(p => (p.batch_id.ToString() == param.batch_id));
                }
            }
            else
            {
                predicate = predicate.And(p => p.batch_id == 0 && p.project_substatus == param.project_substatus);
                predicate = predicate.And(p => p.record_status == Constant.RecordStatus.Active);
            }

            var query = from proj in (db.ProjectRequests.Where(predicate))
                        join creator in db.AccessUser on proj.created_by equals creator.id
                        join update in db.AccessUser on proj.updated_by equals update.id
                        join workflow in db.Workflows on proj.project_substatus equals workflow.project_substatus

                        join cat in db.ProjectCategory on proj.category equals cat.id.ToString() into catt
                        from pcat in catt.DefaultIfEmpty()

                        join pc in db.ProjectClassification on proj.classification equals pc.id.ToString() into pct
                        from pclass in pct.DefaultIfEmpty()

                        orderby proj.id

                        select new
                        {
                            proj,
                            created_name = string.Concat(creator.first_name, " ", creator.mi, " ", creator.last_name),
                            updated_name = string.Concat(update.first_name, " ", update.mi, " ", update.last_name),
                            proj_status = workflow.project_status_desc,
                            proj_substatus = workflow.project_substatus_desc,
                            proj_category = pcat.proj_cat,
                            proj_classification = pclass.classification
                        };

            if (query != null && query.Count() > 0)
            {
                query.ToList().ForEach(qList =>
                {
                    ProjectRequestVM _item = qList.proj.ToView();
                    _item.created_by_name = qList.created_name;
                    _item.updated_by_name = qList.updated_name;
                    _item.project_status_desc = qList.proj_status;
                    _item.project_substatus_desc = qList.proj_substatus;
                    _item.category_desc = qList.proj_category;
                    _item.classification_desc = qList.proj_classification;

                    list.Add(_item);
                });
            }
            return new Result<IEnumerable<ProjectRequestVM>>() { value = list };
        }

        public Result<int> CreateProjectRequestBatch(PayloadVM payload)
        {
            Result<int> _result = new Result<int>();
            try
            {
                ProjectRequestBatch batch = payload.projectRequestBatch.ToDomain();

                using (DatabaseContext db = new DatabaseContext())
                {
                    // Set Workflow
                    var workflow = new Settings.WorkflowLogic().GetWorkflow(new Payload());
                    var wf = workflow.value.Where(x => x.project_substatus == Constant.ProjectRequest.ProjectSubStatus.BudgetApproved_ProcMethodRecom).FirstOrDefault();

                    batch.project_status = wf.project_status;
                    batch.project_substatus = wf.project_substatus;
                    batch.sla = wf.sla;

                    batch.created_date = DateTime.Now;
                    batch.updated_date = DateTime.Now;
                    db.ProjectRequestBatch.Add(batch);

                    db.SaveChanges();

                    // Update ProjectRequest
                    payload.item_list.ToList().ForEach(i =>
                    {
                        var proj = db.ProjectRequests.Find(Convert.ToInt32(i));
                        proj.batch_id = batch.id;
                    });
                    db.SaveChanges();

                    _result.value = batch.id;
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

        public Result<int> UpdateProjectRequestBatch([FromBody] PayloadVM payload)
        {
            Result<int> _result = new Result<int>();
            try
            {
                ProjectRequestBatch request = payload.projectRequestBatch.ToDomain();

                using (DatabaseContext db = new DatabaseContext())
                {
                    var batch = db.ProjectRequestBatch.Find(request.id);
                    if (batch != null)
                    {
                        // Save History
                        var _history = batch.ToHistory();
                        _history.change_log = "Batch Information Change";
                        _history.notes = request.notes;
                        _history.created_by = request.updated_by;
                        _history.created_date = DateTime.Now;
                        _history.updated_by = request.updated_by;
                        _history.updated_date = DateTime.Now;
                        db.ProjectRequestBatchHistory.Add(_history);


                        batch.updated_by = request.updated_by;
                        batch.updated_date = DateTime.Now;

                        // Prepare Batch
                        if (batch.project_substatus == Constant.ProjectRequest.ProjectSubStatus.BudgetApproved_ProcMethodRecom)
                        {
                            batch.applicable_year = request.applicable_year;
                            batch.procurement_method = request.procurement_method;

                            // Clear all ProjectRequest that has the same batch_id
                            var oldRec = from pr in db.ProjectRequests
                                         where pr.batch_id == request.id
                                         select pr;
                            if (oldRec.Count() > 0)
                            {
                                oldRec.ToList().ForEach(x => x.batch_id = 0);
                            }

                            // Update ProjectRequest
                            payload.item_list.ToList().ForEach(i =>
                            {
                                var proj = db.ProjectRequests.Find(Convert.ToInt32(i));
                                proj.batch_id = batch.id;
                            });
                            db.SaveChanges();
                        }

                        // Prepare Invitation to Bid
                        if (batch.project_substatus == Constant.ProjectRequest.ProjectSubStatus.ITB_Preparation)
                        {
                            batch.pre_bid_date = request.pre_bid_date;
                            batch.pre_bid_place = request.pre_bid_place;
                            batch.bid_deadline_date = request.bid_deadline_date;
                            batch.bid_deadline_place = request.bid_deadline_place;
                            batch.bid_opening_date = request.bid_opening_date;
                            batch.bid_opening_place = request.bid_opening_place;
                            db.SaveChanges();
                        }

                        // Advertisement
                        if (batch.project_substatus == Constant.ProjectRequest.ProjectSubStatus.OpenForBidding)
                        {
                            batch.philgeps_publish_date = request.philgeps_publish_date;
                            batch.philgeps_publish_by = request.philgeps_publish_by;
                            batch.mmda_publish_date = request.mmda_publish_date;
                            batch.mmda_publish_by = request.mmda_publish_by;
                            batch.conspost_date_lobby = request.conspost_date_lobby;
                            batch.conspost_date_reception = request.conspost_date_reception;
                            batch.conspost_date_command = request.conspost_date_command;
                            batch.conspost_by = request.conspost_by;
                            batch.newspaper_sent_date = request.newspaper_sent_date;
                            batch.newspaper_publisher = request.newspaper_publisher;
                            batch.newspaper_received_by = request.newspaper_received_by;
                            batch.newspaper_post_date = request.newspaper_post_date;
                            batch.newspaper_post_by = request.newspaper_post_by;

                            batch.philgeps_att = request.philgeps_att;
                            batch.mmda_portal_att = request.mmda_portal_att;
                            batch.conspost_lobby_att = request.conspost_lobby_att;
                            batch.conspost_reception_att = request.conspost_reception_att;
                            batch.conspost_command_att = request.conspost_command_att;
                            batch.newspaper_att = request.newspaper_att;

                            db.SaveChanges();
                        }
                    }

                    _result.value = batch.id;
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

        internal Result<int> UpdateBatchProjectStatus(PayloadVM payload)
        {
            Result<int> _result = new Result<int>();
            Models.Settings.Workflow _newWorkFlow = null;
            try
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var _request = db.ProjectRequestBatch.Find(Convert.ToInt32(payload.projectRequestBatch.id));

                    // Save History
                    var _history = _request.ToHistory();
                    _history.change_log = "Batch ProjectStatus Change";
                    _history.notes = payload.projectRequestBatch.notes;
                    _history.created_by = payload.projectRequestBatch.updated_by;
                    _history.created_date = DateTime.Now;
                    _history.updated_by = payload.projectRequestBatch.updated_by;
                    _history.updated_date = DateTime.Now;
                    db.ProjectRequestBatchHistory.Add(_history);


                    _request.updated_date = DateTime.Now;
                    _request.updated_by = payload.projectRequestBatch.updated_by;

                    // Get Current Seq
                    var _currWorkFlow = db.Workflows.Where(wf => wf.project_status == _request.project_status && wf.project_substatus == _request.project_substatus).First();
                    // Get Workflowtype
                    var _procMode = db.ProcurementMethod.Where(pm => pm.proc_code == _request.procurement_method).First();

                    if (payload.projectRequestBatch.user_action == Constant.UserAction.Approve)
                    {
                        // Update Workflow if status is Active
                        _newWorkFlow = new Settings.WorkflowLogic().GetNextWorkflow(db, _procMode.procurement_mode, _request.project_substatus);
                    }
                    else if (payload.projectRequestBatch.user_action == Constant.UserAction.Return)
                    {
                        _newWorkFlow = new Settings.WorkflowLogic().GetNextWorkflow(db, _procMode.procurement_mode, _request.project_substatus, false);
                    }

                    if (_newWorkFlow != null)
                    {
                        _request.project_status = _newWorkFlow.project_status;
                        _request.project_substatus = _newWorkFlow.project_substatus;
                        _request.sla = _newWorkFlow.sla;
                    }
                    db.ProjectRequestBatch.AddOrUpdate(_request);

                    // Update ProjectRequest records                    
                    var _project = from pr in db.ProjectRequests
                                   join wf in db.Workflows on pr.project_substatus equals wf.project_substatus
                                   where pr.record_status == Constant.RecordStatus.Active && pr.batch_id == payload.projectRequestBatch.id
                                   select new { pr, wf.seq_no };
                    if (_project != null && _project.Count() > 0)
                    {
                        _project.ToList().ForEach(proj =>
                        {
                            // Dont update Project if status has already moved forward individually
                            if (_currWorkFlow.seq_no == proj.seq_no)
                            {
                                // Save History of each ProjectRequest records
                                var _prhistory = proj.pr.ToHistory();
                                _prhistory.change_log = "Batch ProjectStatus Change";
                                _prhistory.notes = payload.projectRequestBatch.notes;
                                _prhistory.created_by = payload.projectRequestBatch.updated_by;
                                _prhistory.created_date = DateTime.Now;
                                _prhistory.updated_by = payload.projectRequestBatch.updated_by;
                                _prhistory.updated_date = DateTime.Now;
                                db.ProjectRequestsHistory.Add(_prhistory);

                                // Update Project details
                                proj.pr.updated_date = DateTime.Now;
                                proj.pr.updated_by = payload.projectRequestBatch.updated_by;
                                proj.pr.notes = payload.projectRequestBatch.notes;

                                proj.pr.project_status = _newWorkFlow.project_status;
                                proj.pr.project_substatus = _newWorkFlow.project_substatus;
                                proj.pr.sla = _newWorkFlow.sla;
                            }
                        });
                    }
                    db.SaveChanges();

                    _result.value = _request.id;
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

        internal Result<int> UpdateBatchRecordStatus(PayloadVM payload)
        {
            Result<int> _result = new Result<int>();
            try
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var _request = db.ProjectRequestBatch.Find(Convert.ToInt32(payload.projectRequestBatch.id));
                    _request.updated_date = DateTime.Now;
                    _request.updated_by = payload.projectRequestBatch.updated_by;
                    _request.notes = payload.projectRequestBatch.notes;
                    _request.record_status = !string.IsNullOrEmpty(payload.projectRequestBatch.record_status) ? payload.projectRequestBatch.record_status : _request.record_status;

                    db.ProjectRequestBatch.AddOrUpdate(_request);

                    // Update ProjectRequest records                    
                    var _project = from pr in db.ProjectRequests
                                   join wf in db.Workflows on pr.project_substatus equals wf.project_substatus
                                   where pr.batch_id == payload.projectRequestBatch.id
                                   select pr;
                    if (_project != null && _project.Count() > 0)
                    {
                        _project.ToList().ForEach(proj =>
                        {
                            // Save History of each ProjectRequest records
                            var _prhistory = proj.ToHistory();
                            _prhistory.change_log = "Batch ProjectStatus Change";
                            _prhistory.notes = payload.projectRequestBatch.notes;
                            _prhistory.created_by = payload.projectRequestBatch.updated_by;
                            _prhistory.created_date = DateTime.Now;
                            _prhistory.updated_by = payload.projectRequestBatch.updated_by;
                            _prhistory.updated_date = DateTime.Now;
                            db.ProjectRequestsHistory.Add(_prhistory);

                            // Update Project details
                            proj.updated_date = DateTime.Now;
                            proj.updated_by = _request.updated_by;
                            proj.notes = _request.notes;
                            proj.record_status = _request.record_status;
                        });
                    }

                    db.SaveChanges();

                    _result.value = _request.id;
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

        private IEnumerable<ProjectRequestBatchVM> PrepareViewResultBatch(DatabaseContext store, PayloadVM payload)
        {
            List<ProjectRequestBatchVM> list = new List<ProjectRequestBatchVM>();
            var query = from batch in store.ProjectRequestBatch
                        join creator in store.AccessUser on batch.created_by equals creator.id
                        join workflow in store.Workflows on batch.project_substatus equals workflow.project_substatus

                        // Procurement Method
                        join pm in store.ProcurementMethod on batch.procurement_method equals pm.proc_code into pmt
                        from proc in pmt.DefaultIfEmpty()

                        where batch.record_status == Constant.RecordStatus.Active
                        &&
                        // ID
                        (string.IsNullOrEmpty(payload.projectSearch.id) || batch.id.ToString() == payload.projectSearch.id)

                        &&
                        // Project Status
                        (string.IsNullOrEmpty(payload.projectSearch.project_status) || batch.project_status == payload.projectSearch.project_status)

                        &&
                        // Project SubStatus
                        (string.IsNullOrEmpty(payload.projectSearch.project_substatus) || batch.project_substatus == payload.projectSearch.project_substatus)

                        orderby batch.id

                        select new
                        {
                            batch,
                            created_name = string.Concat(creator.first_name, " ", creator.mi, " ", creator.last_name),
                            batch_status = workflow.project_status_desc,
                            batch_substatus = workflow.project_substatus_desc,
                            proc_method = proc.procurement_description
                        };

            var proj_sum = from proj in store.ProjectRequests
                           where proj.record_status == Constant.RecordStatus.Active && proj.batch_id > 0
                           group proj by proj.batch_id into projreq
                           select new
                           {
                               batch_id = projreq.Key,
                               total_count = projreq.Count(),
                               total_amount = projreq.Sum(x => x.approved_budget)
                           };
            if (query != null && query.Count() > 0)
            {
                query.ToList().ForEach(qList =>
                {
                    // If batch has no corresponding projectrequest, do not dispay
                    var pj = proj_sum.Where(x => x.batch_id == qList.batch.id);
                    if (pj != null && pj.Count() > 0)
                    {
                        ProjectRequestBatchVM _batch = qList.batch.ToView();
                        _batch.created_by_name = qList.created_name;
                        _batch.project_status_desc = qList.batch_status;
                        _batch.project_substatus_desc = qList.batch_substatus;
                        _batch.procurement_method_desc = qList.proc_method;
                        _batch.total_projects = proj_sum.Where(x => x.batch_id == qList.batch.id).FirstOrDefault().total_count;
                        _batch.total_amount = proj_sum.Where(x => x.batch_id == qList.batch.id).FirstOrDefault().total_amount.ToSafeDecimal().ToString("N", CultureInfo.CurrentCulture);
                        list.Add(_batch);
                    }
                });
            };
            return list;
        }

        private ProjectBatchSearchResultVM SearchViewResultBatch(DatabaseContext store, PayloadVM payload)
        {
            try
            {


                ProjectBatchSearchResultVM _result = new ProjectBatchSearchResultVM();
                List<ProjectRequestBatchVM> list = new List<ProjectRequestBatchVM>();

                var param = payload.projectSearch;

                var predicate = PredicateBuilder.True<ProjectRequestBatch>();
                var predicate_pr = PredicateBuilder.True<ProjectRequest>();

                if (!string.IsNullOrEmpty(param.record_status))
                {
                    predicate = predicate.And(p => p.record_status == param.record_status);
                    predicate_pr = predicate_pr.And(p => p.record_status == param.record_status);
                }
                else
                {
                    predicate = predicate.And(p => p.record_status == Constant.RecordStatus.Active);
                    predicate_pr = predicate_pr.And(p => p.record_status == Constant.RecordStatus.Active);
                }

                int intId = param.batch_id.ToSafeInt();
                if (intId != 0)
                {
                    predicate = predicate.And(p => p.id == intId);
                }
                if (param.submitted_from != null)
                {
                    predicate = predicate.And(p => p.created_date >= param.submitted_from);
                }
                if (param.submitted_to != null)
                {
                    DateTime submit_to = param.submitted_to.Value.AddDays(1);
                    predicate = predicate.And(p => p.created_date < submit_to);
                }
                if (!string.IsNullOrWhiteSpace(param.project_status))
                {
                    predicate = predicate.And(p => p.project_status == param.project_status);
                }
                if (!string.IsNullOrWhiteSpace(param.project_substatus))
                {
                    predicate = predicate.And(p => p.project_substatus == param.project_substatus);
                }
                if (param.applicable_year != 0)
                {
                    predicate = predicate.And(p => p.applicable_year == param.applicable_year);
                }

                int p_id = param.id.ToSafeInt();
                if (p_id != 0)
                {
                    predicate_pr = predicate_pr.And(p => p.id == p_id);
                }
                if (!string.IsNullOrWhiteSpace(param.project_name))
                {
                    predicate_pr = predicate_pr.And(p => p.title.Contains(param.project_name));
                }
                if (!string.IsNullOrWhiteSpace(param.barcode))
                {
                    //TODO    
                }



                var query1 = (from batch in store.ProjectRequestBatch.Where(predicate)
                              join pr in store.ProjectRequests.Where(predicate_pr) on batch.id equals pr.batch_id
                              join creator in store.AccessUser on batch.created_by equals creator.id
                              join workflow in store.Workflows on batch.project_substatus equals workflow.project_substatus

                              // Procurement Method
                              join pm in store.ProcurementMethod on batch.procurement_method equals pm.proc_code into pmt
                              from proc in pmt.DefaultIfEmpty()

                              orderby batch.id

                              select new
                              {
                                  batch,
                                  created_name = string.Concat(creator.first_name, " ", creator.mi, " ", creator.last_name),
                                  batch_status = workflow.project_status_desc,
                                  batch_substatus = workflow.project_substatus_desc,
                                  proc_method = proc.procurement_description
                              }).Distinct();

                var proj_sum = from proj in store.ProjectRequests.Where(predicate_pr)
                               where proj.batch_id > 0
                               group proj by proj.batch_id into projreq
                               select new
                               {
                                   batch_id = projreq.Key,
                                   total_count = projreq.Count(),
                                   total_amount = projreq.Sum(x => x.approved_budget)
                               };

                var query = from q1 in query1
                            join ps in proj_sum on q1.batch.id equals ps.batch_id
                            where ps.total_count > 0
                            orderby q1.batch.id
                            select new { q1, ps };

                _result.page_index = payload.projectSearch.page_index;
                _result.count = query.Count();

                if (query != null && _result.count > 0)
                {
                    int index = (payload.projectSearch.page_index - 1) * payload.projectSearch.page_size;
                    var paged = query.Skip(index).Take(payload.projectSearch.page_size).ToList();

                    paged.ForEach(qList =>
                    {
                        // If batch has no corresponding projectrequest, do not dispay
                        //var pj = proj_sum.Where(x => x.batch_id == qList.batch.id);
                        //if (pj != null && pj.Count() > 0)
                        //{

                        ProjectRequestBatchVM _batch = qList.q1.batch.ToView();
                        _batch.created_by_name = qList.q1.created_name;
                        _batch.project_status_desc = qList.q1.batch_status;
                        _batch.project_substatus_desc = qList.q1.batch_substatus;
                        _batch.procurement_method_desc = qList.q1.proc_method;
                        _batch.total_projects = qList.ps.total_count;// proj_sum.Where(x => x.batch_id == qList.q1.batch.id).FirstOrDefault().total_count;
                        _batch.total_amount = qList.ps.total_amount.ToSafeDecimal().ToString("N", CultureInfo.CurrentCulture);// proj_sum.Where(x => x.batch_id == qList.q1.batch.id).FirstOrDefault().total_amount.ToString();
                        _batch.index = ++index;

                        list.Add(_batch);


                    });
                };
                _result.items = list;


                if (param.get_total.GetValueOrDefault())
                {
                    _result.total = query.Sum(r => (decimal?)r.ps.total_amount).GetValueOrDefault();
                }
                return _result;

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "Batch Search");
                throw;
            }
        }

        public Result<ProjectRequestBatchVM> GetProjectRequestBatchAdvertisement(PayloadVM payload)
        {
            try
            {
                Result<ProjectRequestBatchVM> _result = new Result<ProjectRequestBatchVM>();
                ProjectRequestBatchVM _value = new ProjectRequestBatchVM();


                var predicate_b = PredicateBuilder.True<ProjectRequestBatch>();
                var predicate_p = PredicateBuilder.True<ProjectRequest>();

                if (payload.sub_menu_id == "P-ONGOING")
                {
                    predicate_p = predicate_p.And(p => p.id == payload.id);
                }
                else //payload.sub_menu_id == "P-PROJECTPLAN"
                {
                    predicate_b = predicate_b.And(p => p.id == payload.id);
                }
                using (DatabaseContext db = new DatabaseContext())
                {
                    var query = from batch in db.ProjectRequestBatch.Where(predicate_b)
                                join proj in db.ProjectRequests.Where(predicate_p) on batch.id equals proj.batch_id
                                //where batch.id == payload.id
                                select new
                                {
                                    batch.philgeps_publish_date,
                                    batch.philgeps_publish_by,
                                    batch.philgeps_att,

                                    batch.mmda_publish_date,
                                    batch.mmda_publish_by,
                                    batch.mmda_portal_att,

                                    batch.conspost_by,
                                    batch.conspost_date_command,
                                    batch.conspost_date_reception,
                                    batch.conspost_date_lobby,

                                    batch.conspost_lobby_att,
                                    batch.conspost_reception_att,
                                    batch.conspost_command_att,

                                    batch.newspaper_publisher,
                                    batch.newspaper_post_by,
                                    batch.newspaper_received_by,
                                    batch.newspaper_post_date,
                                    batch.newspaper_sent_date,
                                    batch.newspaper_att,

                                };

                    if (query != null && query.Any())
                    {
                        var batch = query.First();
                        _value.philgeps_publish_date = batch.philgeps_publish_date?.ToString(Constant.DateFormat);
                        _value.philgeps_publish_by = batch.philgeps_publish_by;
                        _value.philgeps_att = batch.philgeps_att;

                        _value.mmda_publish_date = batch.mmda_publish_date?.ToString(Constant.DateFormat);
                        _value.mmda_publish_by = batch.mmda_publish_by;
                        _value.mmda_portal_att = batch.mmda_portal_att;

                        _value.conspost_by = batch.conspost_by;
                        _value.conspost_date_command = batch.conspost_date_command?.ToString(Constant.DateFormat);
                        _value.conspost_date_reception = batch.conspost_date_reception?.ToString(Constant.DateFormat);
                        _value.conspost_date_lobby = batch.conspost_date_lobby?.ToString(Constant.DateFormat);

                        _value.conspost_lobby_att = batch.conspost_lobby_att;
                        _value.conspost_reception_att = batch.conspost_reception_att;
                        _value.conspost_command_att = batch.conspost_command_att;

                        _value.newspaper_publisher = batch.newspaper_publisher;
                        _value.newspaper_post_by = batch.newspaper_post_by;
                        _value.newspaper_received_by = batch.newspaper_received_by;
                        _value.newspaper_post_date = batch.newspaper_post_date?.ToString(Constant.DateFormat);
                        _value.newspaper_sent_date = batch.newspaper_sent_date?.ToString(Constant.DateFormat);
                        _value.newspaper_att = batch.newspaper_att;


                        _result.value = _value;
                    }
                }

                return _result;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "Get Batch Advertisement");
                throw;
            }
        }
        #endregion



        #region Project Bid
        public Result<IEnumerable<ProjectBidVM>> GetProjectBid(PayloadVM payload)
        {
            Result<IEnumerable<ProjectBidVM>> _result = new Result<IEnumerable<ProjectBidVM>>();
            _result.value = this.PrepareViewResultBid(db, payload);
            return _result;
        }

        private IEnumerable<ProjectBidVM> PrepareViewResultBid(DatabaseContext store, PayloadVM payload)
        {
            List<ProjectBidVM> list = new List<ProjectBidVM>();
            var query = from bid in store.ProjectBid
                        join proj in store.ProjectRequests on bid.project_request_id equals proj.id
                        join batch in store.ProjectRequestBatch on proj.batch_id equals batch.id

                        where bid.record_status == Constant.RecordStatus.Active
                        &&
                        // Bid ID
                        (string.IsNullOrEmpty(payload.projectSearch.bid_id) || bid.id.ToString() == payload.projectSearch.bid_id)
                        &&
                        // Project Request ID
                        (string.IsNullOrEmpty(payload.projectSearch.id) || bid.project_request_id.ToString() == payload.projectSearch.id)

                        orderby bid.bid_amount

                        select new
                        {
                            bid,
                            project_title = proj.title,
                            bid_open_date = batch.bid_opening_date,
                            bid_open_place = batch.bid_opening_place
                        };

            if (query != null && query.Count() > 0)
            {
                query.ToList().ForEach(qList =>
                {
                    ProjectBidVM _bid = qList.bid.ToView();
                    _bid.project_request_title = qList.project_title;
                    _bid.bid_opening_date = qList.bid_open_date?.ToString(Constant.DateFormat);
                    _bid.bid_opening_time = qList.bid_open_date?.ToString(Constant.TimeFormat);
                    _bid.bid_opening_place = qList.bid_open_place;
                    list.Add(_bid);
                });
            };
            return list;
        }

        public Result<IEnumerable<ProjectBidChecklistVM>> GetProjectBidChecklist(PayloadVM payload)
        {
            var _list = (from cl in db.ProjectBidChecklist
                         where cl.project_request_id.ToString() == payload.projectSearch.id &&
                         cl.bid_id.ToString() == payload.projectSearch.bid_id
                         select cl.ToView()
                         );
            return new Result<IEnumerable<ProjectBidChecklistVM>>() { value = _list };
        }

        public Result<ProjectBidVM> GetLowestCalculatedBid(PayloadVM payload)
        {
            var _lcb = db.ProjectBid.OrderBy(x => x.bid_amount).ThenBy(x => x.duration).FirstOrDefault();
            return new Result<ProjectBidVM>() { value = _lcb.ToView() };
        }

        public Result<bool> CreateProjectBidChecklist(PayloadVM payload)
        {
            try
            {
                // Delete old records
                var _rec = (from cl in db.ProjectBidChecklist
                            where cl.project_request_id.ToString() == payload.projectSearch.id &&
                             cl.bid_id.ToString() == payload.projectSearch.bid_id
                            select cl
                            );
                db.ProjectBidChecklist.RemoveRange(_rec);
                db.SaveChanges();

                // Save new records
                if (payload.projectBidChecklists != null && payload.projectBidChecklists.Count() > 0)
                {
                    payload.projectBidChecklists.ForEach(list =>
                    {
                        var domain = list.ToDomain();
                        db.ProjectBidChecklist.Add(domain);
                    });
                    db.SaveChanges();
                }

                return new Result<bool>() { value = true };
            }
            catch (Exception ex)
            {
                return new Result<bool>() { status = new Status() { code = Constant.Status.Failed, description = "Error Encountered" }, value = false };
            }
        }

        #endregion



        #region Project Bid Checklist
        public Result<bool> CreateProjectBidChecklists(PayloadVM payload)
        {
            Result<bool> _result = new Result<bool>() { value = true };
            try
            {
                if (payload.projectBidChecklists.Count > 0)
                {
                    //Delete existing records
                    var _proj_id = payload.projectBidChecklists.First().project_request_id;
                    var _bid = payload.projectBidChecklists.First().bid_id;
                    var _stage = payload.projectBidChecklists.First().stage;
                    var _existing = db.ProjectBidChecklist.Where(x =>
                                    x.project_request_id == _proj_id &&
                                    x.bid_id == _bid &&
                                    x.stage == _stage);
                    if (_existing.Count() > 0)
                    {
                        db.ProjectBidChecklist.RemoveRange(_existing);
                    }

                    //Save records
                    payload.projectBidChecklists.ForEach(checklistVM =>
                    {
                        ProjectBidChecklist _domain = checklistVM.ToDomain();
                        _domain.created_by = payload.created_by;
                        _domain.created_date = DateTime.Now;
                        _domain.updated_by = payload.updated_by;
                        _domain.updated_date = DateTime.Now;
                        db.ProjectBidChecklist.Add(_domain);
                    });
                }

                if (payload.projectBids.Count > 0)
                {
                    ProjectBid _vm = payload.projectBids.First().ToDomain();
                    var _domain = db.ProjectBid.Find(_vm.id);
                    _domain.bid_status = _vm.bid_status;
                    _domain.procured_docs = _vm.procured_docs;
                    _domain.eval_result = _vm.eval_result;
                    _domain.gen_eval = _vm.gen_eval;
                    _domain.notes = _vm.notes;
                    _domain.created_by = payload.created_by;
                    _domain.created_date = DateTime.Now;
                    _domain.updated_by = payload.updated_by;
                    _domain.updated_date = DateTime.Now;
                    db.ProjectBid.AddOrUpdate(_domain);
                }
                db.SaveChanges();
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

        public Result<List<ProjectBidChecklist>> GetProjectBidChecklists(PayloadVM payload)
        {
            Result<List<ProjectBidChecklist>> _result = new Result<List<ProjectBidChecklist>>();
            try
            {
                _result.value = (from cl in db.ProjectBidChecklist
                                 where cl.project_request_id.ToString() == payload.projectSearch.id &&
                                 cl.bid_id.ToString() == payload.projectSearch.bid_id &&
                                 cl.stage == payload.projectSearch.stage
                                 select cl).ToList();
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

        #endregion



        #region Project Request Advertisement

        public Result<ProjectRequestAdvertisementVM> GetProjectRequestAdvertisement(PayloadVM payload)
        {
            Result<ProjectRequestAdvertisementVM> _result = new Result<ProjectRequestAdvertisementVM>();
            try
            {
                if (payload.id > 0)
                {
                    var advertise = db.ProjectRequestAdvertisements.Where(pa => pa.project_request_id == payload.id).First();
                    if (advertise != null)
                    {
                        _result.value = advertise.ToView();
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
            }
            return _result;
        }

        public Result<bool> UpdateProjectRequestAdvertisement(PayloadVM payload)
        {
            Result<bool> _result = new Result<bool>() { value = true };
            try
            {
                if (payload.projectAdvertisement != null)
                {
                    var advertiseData = payload.projectAdvertisement.ToDomain();

                    var data = db.ProjectRequestAdvertisements.Where(pa => pa.project_request_id == advertiseData.project_request_id);
                    if (data != null && data.Count() > 0)
                    {
                        advertiseData.id = data.First().id;
                    }
                    db.ProjectRequestAdvertisements.AddOrUpdate(advertiseData);
                    db.SaveChanges();
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

        #endregion


        #region Document Attachment
        private void SaveDocumentAttachments(List<ProjectRequestAttachmentVM> documentAttachments)
        {
            //var _result = new Result<IEnumerable<DocumentAttachmentVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    documentAttachments.ForEach(a =>
                    {

                        var docAtt = a.ToDomain();

                        //if (a.id == 0)
                        //{
                        //    db.ProjectRequestAttachments.Add(
                        //    new ProjectRequestAttachment()
                        //    {
                        //        project_id = docAtt.project_id,
                        //        status = Constant.RecordStatus.Active,
                        //        batch_id = docAtt.batch_id,
                        //        attachment_name = docAtt.attachment_name,
                        //        file_name = docAtt.file_name,
                        //        barcode_no = docAtt.barcode_no,

                        //        updated_by = docAtt.updated_by,
                        //        updated_date = docAtt.updated_date,
                        //        created_by = docAtt.created_by,
                        //        created_date = docAtt.created_date
                        //    });
                        //}
                        //else
                        if (a.process == Constant.RecordStatus.Cancelled)
                        {
                            var entity = new ProjectRequestAttachment() { id = docAtt.id };
                            db.ProjectRequestAttachments.Attach(entity);
                            db.ProjectRequestAttachments.Remove(entity);
                        }
                        else if (a.process == Constant.TransactionType.Update)
                        {
                            var entity = new ProjectRequestAttachment() { id = docAtt.id };
                            db.ProjectRequestAttachments.Attach(entity);

                            entity.barcode_no = docAtt.barcode_no;//from project requests; only barcode is editable
                            entity.updated_by = docAtt.updated_by;
                            entity.updated_date = docAtt.updated_date;
                            db.ProjectRequestAttachments.AddOrUpdate(entity);

                        }
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                //_result.status = new Status()
                //{
                //    code = Constant.Status.Failed,
                //    description = ex.Message
                //};
            }
        }

        public Result<int> SaveDocumentAttachment(ProjectRequestAttachmentVM documentAttachment)
        {
            var _result = new Result<int>();
            try
            {
                using (var db = new DatabaseContext())
                {

                    var docAtt = documentAttachment.ToDomain();

                    var entity = new ProjectRequestAttachment()
                    {
                        project_id = docAtt.project_id,
                        status = Constant.RecordStatus.Active,
                        batch_id = docAtt.batch_id,
                        attachment_name = docAtt.attachment_name,
                        file_name = docAtt.file_name,
                        barcode_no = docAtt.barcode_no,

                        updated_by = docAtt.updated_by,
                        updated_date = docAtt.updated_date,
                        created_by = docAtt.created_by,
                        created_date = docAtt.created_date
                    };

                    db.ProjectRequestAttachments.Add(entity);

                    db.SaveChanges();
                    _result.value = entity.id;
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

        private List<ProjectRequestAttachmentVM> GetAttachments(DatabaseContext store, int project_id)
        {
            var query = from pa in store.ProjectRequestAttachments
                        where pa.project_id == project_id
                        select pa;

            return query.ToList().Select(item => item.ToView()).ToList();
        }

        internal Result<IEnumerable<ProjectRequestAttachmentVM>> GetProjectRequestAttachments(PayloadVM payload)
        {
            Result<IEnumerable<ProjectRequestAttachmentVM>> _result =
                                               new Result<IEnumerable<ProjectRequestAttachmentVM>>();

            var query = from pa in db.ProjectRequestAttachments
                        where pa.project_id == payload.id
                        select pa;




            _result.value = query.ToList().Select(item => item.ToView()).ToList();

            foreach (var itm in _result.value)
            {
                var re = db.AccessUser
                    .Where(x => x.id == itm.updated_by).FirstOrDefault();

                if (re != null)
                {
                    itm.updated_by_name = re.username;
                }


            }

            //_result.value = query.ToList().Select(item => item.ToView()).ToList();




            return _result;
        }

        internal Result<IEnumerable<ProjectRequestItemVM>> GetProjectRequestItems(PayloadVM payload)
        {
            Result<IEnumerable<ProjectRequestItemVM>> _result = new Result<IEnumerable<ProjectRequestItemVM>>();

            var query = from pi in db.ProjectRequestItems
                        where pi.project_id == payload.id
                        select pi;



            _result.value = query.ToList().Select(item => item.ToView()).ToList();
            return _result;
        }
        #endregion



        #region Utilities

        private string GetUserAction(string status)
        {
            string _action = "Unknown Status";
            switch (status)
            {
                case Constant.RecordStatus.Active:
                    _action = "Activate";
                    break;
                case Constant.RecordStatus.Cancelled:
                    _action = "Cancelled";
                    break;
                case Constant.RecordStatus.Deleted:
                    _action = "Deleted";
                    break;
                case Constant.RecordStatus.Draft:
                    _action = "Draft";
                    break;
                case Constant.RecordStatus.Inactive:
                    _action = "Deactivate";
                    break;
                case Constant.RecordStatus.Rejected:
                    _action = "Rejected";
                    break;
                default:
                    break;
            }
            return _action;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public Result<IEnumerable<ProjectGranteeVM>> GetProjectGranteesFilter()
        {
            Result<IEnumerable<ProjectGranteeVM>> _result =
                                               new Result<IEnumerable<ProjectGranteeVM>>();
            var query = from pg in db.ProjectGrantees
                        where pg.status != Constant.RecordStatus.Deleted
                        select new ProjectGranteeVM
                        {
                            id = pg.id,
                            grantee_name = pg.grantee_name
                        };

            _result.value = query.ToList();
            return _result;
        }
        public Result<IEnumerable<ProjectCategoryVM>> GetProjectCategoriesFilter()
        {
            Result<IEnumerable<ProjectCategoryVM>> _result =
                                               new Result<IEnumerable<ProjectCategoryVM>>();
            var query = from pc in db.ProjectCategory
                        where pc.status != Constant.RecordStatus.Deleted
                        select new ProjectCategoryVM
                        {
                            id = pc.id,
                            proj_cat = pc.proj_cat
                        };

            _result.value = query.ToList();
            return _result;
        }
        #endregion
    }
}

