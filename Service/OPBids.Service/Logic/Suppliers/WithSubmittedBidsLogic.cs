using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Models.ProjectRequest;
using OPBids.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;
using System.Globalization;
using OPBids.Entities.View.Suppliers;
using OPBids.Entities.View.Supplier;
using static OPBids.Common.Constant.ProjectRequest;

namespace OPBids.Service.Logic
{
    public class WithSubmittedBidsLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<SuppliersVM>> MaintainData(SupplierPayloadVM param)
        {
            Result<IEnumerable<SuppliersVM>> rslts;
            switch (param.process.ToSafeString().ToUpper().Trim())
            {
                case Constant.TransactionType.Search:
                    rslts = GetData(param);
                    break;
                case Constant.TransactionType.Save:
                    rslts = Update(param);
                    break;
                case Constant.TransactionType.Withdraw:
                    rslts = Withdraw(param);
                    break;
                default:
                    rslts = GetData(param);
                    break;
            }
            return rslts;
        }

        public Result<IEnumerable<SuppliersVM>> GetData(SupplierPayloadVM payload)
        {
            var _result = new Result<IEnumerable<SuppliersVM>>();
            List<SuppliersVM> list = new List<SuppliersVM>();

            var predicate = PredicateBuilder.True<ProjectRequest>();

            predicate = predicate.And(p => p.record_status == Constant.RecordStatus.Active);
            predicate = predicate.And(p => p.project_substatus == Constant.ProjectRequest.ProjectSubStatus.CloseForShortlising);

            if (payload.filter.budget_min != 0)
            {
                predicate = predicate.And(p => p.estimated_budget >= payload.filter.budget_min);
            }

            if (payload.filter.budget_max != 0)
            {
                predicate = predicate.And(p => p.estimated_budget <= payload.filter.budget_max);
            }

            if (payload.filter.category != null)
            {
                payload.filter.category = payload.filter.category.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                if (payload.filter.category.Count > 0)
                {
                    predicate = predicate.And(p => payload.filter.category.Contains(p.category));
                }
            }

            if (payload.filter.grantee != null)
            {
                payload.filter.grantee = payload.filter.grantee.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                if (payload.filter.grantee.Count > 0)
                {
                    predicate = predicate.And(p => payload.filter.grantee.Contains(p.grantee));
                }
            }


            if (!string.IsNullOrWhiteSpace(payload.filter.project_name))
            {
                predicate = predicate.And(p => p.title.Contains(payload.filter.project_name));
            }

            if (payload.filter.RefNo != 0)
            {
                predicate = predicate.And(p => p.batch_id == payload.filter.RefNo);
            }
            if (payload.filter.required_from != null)
            {
                predicate = predicate.And(p => p.required_date >= payload.filter.required_from);
            }
            if (payload.filter.required_to != null)
            {
                var reqEndDate = payload.filter.required_to.Value.AddDays(1);
                predicate = predicate.And(p => p.required_date < reqEndDate);
            }

            if (payload.filter.submitted_from != null)
            {
                predicate = predicate.And(p => p.earmark_date >= payload.filter.submitted_from);
            }
            if (payload.filter.submitted_to != null)
            {
                var subEndDate = payload.filter.submitted_to.Value.AddDays(1);
                predicate = predicate.And(p => p.earmark_date < subEndDate);
            }


            var query = (from proj in db.ProjectRequests.Where(predicate)
                         join creator in db.AccessUser on proj.created_by equals creator.id
                             join update in db.AccessUser on proj.updated_by equals update.id
                             join workflow in db.Workflows on proj.project_substatus equals workflow.project_substatus
                             join cat in db.ProjectCategory on proj.category equals cat.id.ToString() into catt
                             from pcat in catt.DefaultIfEmpty()
                             join pc in db.ProjectClassification on proj.classification equals pc.id.ToString() into pct
                             from pclass in pct.DefaultIfEmpty()
                             join ct in db.ContractType on proj.contract_type equals ct.id.ToString() into ctt
                             from contract in ctt.DefaultIfEmpty()
                             join dsl in db.DocumentSecurityLevel on proj.security_level equals dsl.id into dslt
                             from seclevel in dslt.DefaultIfEmpty()
                             join d in db.Delivery on proj.delivery_type equals d.id into dt
                             from deltype in dt.DefaultIfEmpty()
                             //where proj.record_status == Constant.RecordStatus.Active && //proj.project_substatus == ProjectSubStatus.CloseForShortlising &&
                             //(payload.filter.budget_min == 0 || proj.estimated_budget >= payload.filter.budget_min) &&
                             //(payload.filter.budget_max == 0 || proj.estimated_budget < payload.filter.budget_max) &&
                             //(payload.filter.category == 0 || proj.category == payload.filter.category.ToString()) &&
                             //(payload.filter.grantee == null || proj.grantee == payload.filter.grantee) &&
                             //(payload.filter.project_name == null || payload.filter.project_name.Contains(proj.title)) &&
                             //(payload.filter.RefNo == 0 || proj.batch_id == payload.filter.RefNo) &&
                             //(payload.filter.required_from == null || proj.required_date >= payload.filter.required_from) &&
                             //(payload.filter.required_to == null || proj.required_date < payload.filter.required_to) &&
                             //(payload.filter.submitted_from == null || proj.earmark_date >= payload.filter.submitted_from) &&
                             //(payload.filter.submitted_to == null || proj.earmark_date < payload.filter.submitted_to)
                             orderby proj.batch_id
                             select new 
                             {
                                 ref_no = proj.batch_id,
                                 bid_bond = proj.bid_bond,
                                 category = pcat.proj_cat,
                                 created_by = proj.created_by,
                                 created_date = proj.created_date,
                                 id = proj.id,
                                 project_duration = proj.project_duration,
                                 created_by_name = string.Concat(creator.first_name, " ", creator.mi, " ", creator.last_name),
                                 updated_by_name = string.Concat(update.first_name, " ", update.mi, " ", update.last_name),
                                 amount = proj.estimated_budget,
                                 approved_budget = proj.approved_budget,
                                 date_submitted = proj.routed_date,
                                 deadline = proj.required_date,
                                 status = workflow.project_substatus_desc,
                                 project = proj.title,
                                 project_desc = proj.description,
                             }).ToList();

            _result.total_count = query.Count;
            if (payload.page_index != -1)
            {
                int index = (payload.page_index) * Constant.AppSettings.PageItemCount;
                _result.page_index = payload.page_index + 1;// _result.total_count.GetPageCount();
                var paged = query.Skip(Constant.AppSettings.PageItemCount * payload.page_index).
                                     Take(Constant.AppSettings.PageItemCount).ToList();

                paged.ForEach(qList =>
                {
                    var item = new SuppliersVM()
                    {
                        ref_no = qList.ref_no,
                        bid_bond = qList.bid_bond,
                        category = qList.category,
                        created_by = qList.created_by,
                        created_date = qList.created_date.ToString(),
                        id = qList.id,
                        project_duration = qList.project_duration,
                        created_by_name = qList.created_by_name,
                        updated_by_name = qList.updated_by_name,
                        amount = qList.amount.ToString("N", CultureInfo.CurrentCulture),
                        approved_budget = qList.approved_budget.ToString("N", CultureInfo.CurrentCulture),
                        date_submitted = qList.date_submitted.HasValue ? qList.date_submitted.Value.ToString(Constant.DateFormat) : "",
                        deadline = qList.deadline.ToString(),
                        status = qList.status,
                        project = qList.project,
                        project_desc = qList.project_desc,
                        index = ++index
                    };
                    list.Add(item);
                }
                );
            }
            _result.value = list;
            return _result;
        }

        public Result<IEnumerable<SuppliersVM>> Update([FromBody] SupplierPayloadVM param)
        {
            var _result = new Result<IEnumerable<SuppliersVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    var itm = db.ProjectRequests.FirstOrDefault(a => a.id == param.supplier.id);
                    itm.bid_bond = param.supplier.bid_bond;
                    itm.project_duration = param.supplier.project_duration;
                    itm.estimated_budget = param.supplier.amount.ToSafeInt();
                    itm.project_status = param.supplier.status;
                    db.ProjectRequests.AddOrUpdate(itm);
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
        public Result<IEnumerable<SuppliersVM>> Withdraw([FromBody] SupplierPayloadVM param)
        {
            var _result = new Result<IEnumerable<SuppliersVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    var itm = db.ProjectRequests.FirstOrDefault(a => a.id == param.supplier.id);
                    itm.record_status = Constant.RecordStatus.Cancelled;
                    itm.notes = param.supplier.notes;
                    db.ProjectRequests.AddOrUpdate(itm);
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

