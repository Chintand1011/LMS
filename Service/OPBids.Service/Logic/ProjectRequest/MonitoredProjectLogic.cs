using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Service.Data;
using OPBids.Service.Models.ProjectRequest;
using OPBids.Service.Models.Settings;
using OPBids.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace OPBids.Service.Logic
{
    public class MonitoredProjectLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<bool> MonitoredProject(PayloadVM payload)
        {
            Result<bool> _result = new Result<bool>();
            try
            {
                if (payload != null && payload.monitoredProject != null)
                {
                    var rec = db.MonitoredProjects.Where(x =>
                                x.project_request_id == payload.monitoredProject.project_request_id &&
                                x.created_by == payload.monitoredProject.created_by);

                    if (payload.monitoredProject.action == Constant.RecordStatus.Active)
                    {
                        if (rec.Count() == 0)
                        {
                            MonitoredProject mp = new MonitoredProject()
                            {
                                project_request_id = payload.monitoredProject.project_request_id,
                                created_by = payload.monitoredProject.created_by,
                                created_date = DateTime.Now
                            };
                            db.MonitoredProjects.Add(mp);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        if (rec.Count() > 0)
                        {
                            db.MonitoredProjects.RemoveRange(rec);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _result.status = new Status() { code = Constant.Status.Failed, description = "Error on Monitoring Project - " + ex.Message };
            }

            return _result;
        }

        public Result<ProjectSearchResultVM> SearchMonitorProject(PayloadVM payload)
        {
            Result<ProjectSearchResultVM> _result = new Result<ProjectSearchResultVM>();
            _result.value = this.SearchViewResult(db, payload);
            return _result;
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

                predicate = predicate.And(p => p.record_status == Constant.RecordStatus.Active);

                if (param.id.ToSafeInt() != 0)
                {
                    int intId = param.id.ToSafeInt();
                    predicate = predicate.And(p => p.id == intId);
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
                if (param.budget_min != 0)
                {
                    predicate = predicate.And(p => p.estimated_budget >= param.budget_min);
                }
                if (param.budget_max != 0)
                {
                    predicate = predicate.And(p => p.estimated_budget <= param.budget_max);
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
                if (payload.monitoredProject != null && payload.monitoredProject.created_by > 0)
                {
                    predicate_mp = predicate_mp.And(mp => mp.created_by == payload.monitoredProject.created_by);
                }

                if (!string.IsNullOrWhiteSpace(param.project_name))
                {
                    predicate = predicate.And(p => p.title.Contains(param.project_name));
                }

                var query = from proj in (store.ProjectRequests.Where(predicate))
                            join creator in store.AccessUser.Where(predicate_au) on proj.created_by equals creator.id
                            join update in store.AccessUser on proj.updated_by equals update.id
                            join workflow in store.Workflows.Where(predicate_wf) on proj.project_substatus equals workflow.project_substatus
                            join monitor in store.MonitoredProjects.Where(predicate_mp) on proj.id equals monitor.project_request_id

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

        private List<ProjectRequestAttachmentVM> GetAttachments(DatabaseContext store, int project_id)
        {
            var query = from pa in store.ProjectRequestAttachments
                        where pa.project_id == project_id
                        select pa;

            return query.ToList().Select(item => item.ToView()).ToList();
        }
    }
}