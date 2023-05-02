using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Shared;
using OPBids.Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using OPBids.Service.Logic.Settings;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Service.Utilities;
using OPBids.Service.Models.Settings;

using OPBids.Service.Models.ProjectRequest;

namespace OPBids.Service.Logic.Shared
{
    public class SharedLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<ProjectTotalVM> GetProjectTotal(Payload payload)
        {

            Result<ProjectTotalVM> _result = new Result<ProjectTotalVM>();
            _result.value = new ProjectTotalVM();
            try
            {
                var currYear = DateTime.Now.Year; //2018;
                var prefix = string.Empty;

                var predicate = PredicateBuilder.True<ProjectRequest>();
                // Active records
                predicate = predicate.And(p => p.record_status == Constant.RecordStatus.Active);

                switch (payload.dashboard_id)
                {
                    case 6: //budget
                        predicate = predicate.And(p => (p.project_status != Constant.ProjectRequest.ProjectStatus.Draft && p.project_status != Constant.ProjectRequest.ProjectStatus.NewlyRequested && p.project_status != Constant.ProjectRequest.ProjectStatus.ForBudgetApproval) && p.updated_date.Year == currYear);

                        _result.value.desc = "Total Approved Projects";
                        prefix = "₱ ";
                        break;

                    default:
                        predicate = predicate.And(p => p.project_status == Constant.ProjectRequest.ProjectStatus.Completed && p.updated_date.Year == currYear);

                        prefix = "₱ ";
                        _result.value.desc = "Total Completed Projects";
                        break;
                }



                var items = (from pr in db.ProjectRequests.Where(predicate)
                                 //where pr.project_status == projStatus && pr.updated_date.Year == currYear
                             group pr by pr.updated_date.Month into grp
                             select new ChartData { index = grp.Key, amount = grp.Sum(g => g.approved_budget) }
                            ).ToList();

                _result.value.data = new List<ChartData>();

                for (var i = 1; i <= 12; i++)
                {
                    var cd = new ChartData() { index = i };
                    cd.amount = items.Where(item => item.index == i).Select(item => item.amount).FirstOrDefault();
                    _result.value.data.Add(cd);
                }

                _result.value.total = prefix + items.Sum(i => i.amount).ToString("N2"); // "P934,283,992";
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "PROJECT_TOTAL");
            }
            return _result;
        }
        public Result<ProjectTotalVM> GetDocumentTotal(Payload payload)
        {

            Result<ProjectTotalVM> _result = new Result<ProjectTotalVM>();
            var docCatLst = new List<ChartData>();
            DateTime dateFrom;
            DateTime dateTo;
            decimal amt;
            var curDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            for (var i = 1; i <= 12; i++)
            {
                dateFrom = new DateTime(DateTime.Today.Year, i, 1);
                dateTo = new DateTime(DateTime.Today.Year, i, 1).AddMonths(1);
                var itm = new ChartData()
                {
                    index = i,
                    amount = db.OnHandDocuments.Where(a => a.status == Constant.RecordStatus.Active & a.created_date >= dateFrom && a.created_date < dateTo).Count() +
                    db.ReceivedDocuments.Where(a => a.status == Constant.RecordStatus.Active & a.created_date >= dateFrom &&
                    a.created_date < dateTo && (db.AccessUser.ToList().Any(c => c.id == a.created_by &&
                    db.DocumentRoutes.Any(b => b.batch_id == a.id && b.department_id == c.dept_id && b.sequence != null)))).Count() +
                    db.FinalizedDocuments.Where(a => (a.status == Constant.RecordStatus.Active || a.status == Constant.RecordStatus.MarkForArchive) & a.created_date >= dateFrom && a.created_date < dateTo).Count() +
                    db.ArchivedDocuments.Where(a => (a.status == Constant.RecordStatus.Active) & a.created_date >= dateFrom && a.created_date < dateTo).Count(),
                    sum_amount = dateFrom > curDate ? 0 : 1
                };
                amt = itm.amount;
                itm.amount = itm.amount <= 0 ? itm.sum_amount : itm.amount;
                itm.sum_amount = amt;
                docCatLst.Add(itm);
            }
            _result.value = new ProjectTotalVM() { data = docCatLst, desc = "Total Tracked Documents", total = docCatLst.Sum(a => a.sum_amount).ToSafeInt().ToSafeString() };
            return _result;
        }

        public Result<List<ProgressVM>> GetProjectProgress(PayloadVM payload)
        {
            Result<List<ProgressVM>> _result = new Result<List<ProgressVM>>();
            try
            {
                ProjectRequest _pr = (from pr in db.ProjectRequests
                                      where pr.id == payload.id
                                      select pr).First();
                string current_project_status = _pr.project_status;

                List<ProjectRequestHistory> _history;
                _history = (from _h in db.ProjectRequestsHistory
                            where _h.project_request_id == payload.id
                            select _h).ToList();

                var _workflow = db.Workflows.Where(wf => wf.record_status == Constant.RecordStatus.Active);

                List<ProgressVM> _list = new List<ProgressVM>();
                ProgressVM _newly, _budget, _bidding, _twg, _install, _comp;
                Func<int, List<Workflow>, string> _getProgress = (current, list) =>
                {
                    var _active = list.Where(a => a.seq_no == current).Count() > 0;
                    if (_active) { return Constant.ProgressStatus.Active; };

                    var _compl = list.Max(com => com.seq_no);
                    if (_compl < current) { return Constant.ProgressStatus.Complete; };

                    var _pending = list.Min(pen => pen.seq_no);
                    if (_pending > current) { return Constant.ProgressStatus.Pending; };

                    return Constant.ProgressStatus.Pending;
                };
                Func<string, string> _getHistory = (target) =>
                {
                    string _date = string.Empty;
                    var _projhis = _history.Where(hl => hl.project_status == target).OrderByDescending(o => o.id).FirstOrDefault();
                    if (_projhis != null)
                    {
                        _date = _projhis.created_date.ToString(Constant.DateFormat);
                    }
                    return _date;
                };

                //========== Get Current Status workflow
                var _current = _workflow.Where(w => w.project_status == current_project_status);
                int _current_seq_no = _current.First().seq_no;

                //========== Newly Requested
                var _newlyWF = _workflow.Where(w => w.project_status == Constant.ProjectRequest.ProjectStatus.NewlyRequested);
                var _newStatus = _getProgress(_current_seq_no, _newlyWF.ToList());
                string _newDate = string.Empty;
                if (_newStatus == Constant.ProgressStatus.Complete)
                {
                    _newDate = _getHistory(Constant.ProjectRequest.ProjectStatus.NewlyRequested);
                }
                _newly = new ProgressVM() { order = 1, header = _newlyWF.First().project_status_desc, detail = _newDate, status = _newStatus, customStyle = "padding-right: 75px;" };
                _list.Add(_newly);


                // Budget Approval            
                var _budgetWF = _workflow.Where(w => w.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval);
                var _budgetStatus = _getProgress(_current.First().seq_no, _budgetWF.ToList());
                string _budgetDate = string.Empty;
                if (_budgetStatus == Constant.ProgressStatus.Complete)
                {
                    _budgetDate = _getHistory(Constant.ProjectRequest.ProjectStatus.ForBudgetApproval);
                }
                _budget = new ProgressVM() { order = 2, header = _budgetWF.First().project_status_desc, detail = _budgetDate, status = _budgetStatus, customStyle = "padding-right: 100px;" };
                _list.Add(_budget);


                //========== Pre-Bidding
                List<string> bidList = new List<string>() {
                Constant.ProjectRequest.ProjectStatus.PreBidding_BudgetApproved,
                Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_Hope,
                Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_BACSEC,
                Constant.ProjectRequest.ProjectStatus.PreBidding_Posting,
                Constant.ProjectRequest.ProjectStatus.PreBidding_Close_ReOpen
            };
                var _biddingWF = _workflow.Where(w => bidList.Contains(w.project_status));
                var _bidStatus = _getProgress(_current.First().seq_no, _biddingWF.ToList());
                string _bidDate = string.Empty;
                if (_bidStatus == Constant.ProgressStatus.Complete)
                {
                    _bidDate = _getHistory(Constant.ProjectRequest.ProjectStatus.PreBidding_Close_ReOpen);
                }
                _bidding = new ProgressVM() { order = 3, header = _biddingWF.First().project_status_desc, detail = _bidDate, status = _bidStatus, customStyle = "padding-right: 250px;" };
                _list.Add(_bidding);


                //========== Under TWG Qualification
                List<string> twgList = new List<string>() {
                Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Ranking,
                Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_LCB,
                Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Eval,
                Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_PostEval,
                Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_ForPostQual,
                Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_PostQual
            };
                var _twgWF = _workflow.Where(w => twgList.Contains(w.project_status));
                var _twgStatus = _getProgress(_current.First().seq_no, _twgWF.ToList());
                string _twgDate = string.Empty;
                if (_twgStatus == Constant.ProgressStatus.Complete)
                {
                    _twgDate = _getHistory(Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_PostQual);
                }
                _twg = new ProgressVM() { order = 4, header = _twgWF.First().project_status_desc, detail = _twgDate, status = _twgStatus, customStyle = "padding-right: 50px;" };
                _list.Add(_twg);


                //========== Project Installation
                var _installWF = _workflow.Where(w => w.project_status == Constant.ProjectRequest.ProjectStatus.ProjectInstallation);
                var _installStatus = _getProgress(_current.First().seq_no, _installWF.ToList());
                string _installDate = string.Empty;
                if (_installStatus == Constant.ProgressStatus.Complete)
                {
                    _installDate = _getHistory(Constant.ProjectRequest.ProjectStatus.ProjectInstallation);
                }
                _install = new ProgressVM() { order = 5, header = _installWF.First().project_status_desc, detail = _installDate, status = _installStatus, customStyle = "padding-right: 20px;" };
                _list.Add(_install);


                //========== Completed
                var _compWF = _workflow.Where(w => w.project_status == Constant.ProjectRequest.ProjectStatus.Completed);
                var _compStatus = _getProgress(_current.First().seq_no, _compWF.ToList());
                string _compDate = string.Empty;
                if (_compStatus == Constant.ProgressStatus.Complete)
                {
                    _compDate = _getHistory(Constant.ProjectRequest.ProjectStatus.Completed);
                }
                _comp = new ProgressVM() { order = 6, header = _compWF.First().project_status_desc, detail = _compDate, status = _compStatus, customStyle = "" };
                _list.Add(_comp);

                _result.value = _list;
            }
            catch (Exception ex)
            {
                _result.status.code = Constant.Status.Failed;
                _result.status.description = ex.Message;
            }
            return _result;
        }

        public Result<List<KeyValue>> GetSettingsList(Payload payload)
        {
            Result<List<KeyValue>> result = new Result<List<KeyValue>>();
            try
            {
                List<KeyValue> _list = new List<KeyValue>();
                payload.setting_list.ToList().ForEach(sl =>
                {
                    switch (sl)
                    {
                        case Constant.Setting.Selection.ProjectCategory:
                            var _projCat = new ProjectCategoryLogic().GetProjectCategory(payload).value;
                            if (_projCat != null && _projCat.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectCategory, key = string.Empty, value = Constant.PleaseSelect });
                                _projCat.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectCategory, key = pj.id.ToString(), value = pj.proj_cat });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.ProjectStatus:
                            var _projStat = new ProjectStatusLogic().GetProjectStatus(payload).value;
                            if (_projStat != null && _projStat.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectStatus, key = string.Empty, value = Constant.PleaseSelect });
                                _projStat.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectStatus, key = pj.id.ToString(), value = pj.proj_statcode });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.ProjectClassification:
                            var _projClass = new ProjectClassificationLogic().GetProjectClassification(payload).value;
                            if (_projClass != null && _projClass.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectClassification, key = string.Empty, value = Constant.PleaseSelect });
                                _projClass.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectClassification, key = pj.id.ToString(), value = pj.classification });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.ContractType:
                            var _contract = new ContractTypeLogic().GetContractType(payload).value;
                            if (_contract != null && _contract.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.ContractType, key = string.Empty, value = Constant.PleaseSelect });
                                _contract.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.ContractType, key = pj.id.ToString(), value = pj.contract_type });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.DocumentSecurityLevel:
                            var _security = new DocumentSecurityLevelLogic().GetDocumentSecurityLevel(payload).value;
                            if (_security != null && _security.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.DocumentSecurityLevel, key = string.Empty, value = Constant.PleaseSelect });
                                _security.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.DocumentSecurityLevel, key = pj.id.ToString(), value = pj.code });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.Delivery:
                            var _delivery = new DeliveryLogic().GetDelivery(payload).value;
                            if (_delivery != null && _delivery.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.Delivery, key = string.Empty, value = Constant.PleaseSelect });
                                _delivery.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.Delivery, key = pj.id.ToString(), value = pj.delivery_code });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.SourceFunds:
                            var _source = new SourceFundsLogic().GetSourceFunds(payload).value;
                            if (_source != null && _source.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.SourceFunds, key = string.Empty, value = Constant.PleaseSelect });
                                _source.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.SourceFunds, key = pj.id.ToString(), value = pj.source_code });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.ProcurementMethod:
                            var _method = new ProcurementMethodLogic().GetProcurementMethod(payload).value;
                            if (_method != null && _method.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProcurementMethod, key = string.Empty, value = Constant.PleaseSelect });
                                _method.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProcurementMethod, key = pj.proc_code, value = pj.procurement_description });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.ProjectGrantee:
                            var _projGrantee = new ProjectGranteeLogic().GetProjectGrantee(payload).value;
                            if (_projGrantee != null && _projGrantee.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectGrantee, key = string.Empty, value = Constant.PleaseSelect });
                                _projGrantee.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectGrantee, key = pj.id.ToString(), value = pj.grantee_name });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.ProjectGranteeAuto:
                        case Constant.Setting.Selection.ProjectGranteePrevUsed:
                            var _projGrantees = new ProjectGranteeLogic().GetProjectGranteeAuto(payload).value;
                            if (_projGrantees != null && _projGrantees.Count() > 0)
                            {
                                _projGrantees.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectGrantee, key = pj.id.ToString(), value = pj.grantee_name });
                                });
                            }
                            break;
                        case Constant.Setting.Selection.ProjectProponent:
                            var _projProp = new ProjectProponentLogic().GetProjectProponent(payload).value;
                            if (_projProp != null && _projProp.Count() > 0)
                            {
                                _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectProponent, key = string.Empty, value = Constant.PleaseSelect });
                                _projProp.ToList().ForEach(pj =>
                                {
                                    _list.Add(new KeyValue() { type = Constant.Setting.Selection.ProjectProponent, key = pj.id.ToString(), value = pj.proponent_name });
                                });
                            }
                            break;
                    }
                });
                result.value = _list;
            }
            catch (Exception ex)
            {
                result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return result;
        }

        #region DocumentReceiving
        public Result<ProjectRequestVM> CheckProjectRequestDocument(PayloadVM payload)
        {
            try
            {
                ProjectRequestVM data = new ProjectRequestVM();

                var _grantee = from g in db.ProjectGrantees
                               select g;

                var _record = (from pr in db.ProjectRequests
                               join wf in db.Workflows on pr.project_substatus equals wf.project_substatus

                               join cat in db.ProjectCategory on pr.category equals cat.id.ToString() into catt
                               from pcat in catt.DefaultIfEmpty()

                               join batch in db.ProjectRequestBatch on pr.batch_id equals batch.id into batcht
                               from projBatch in batcht.DefaultIfEmpty()

                               join bpm in db.ProcurementMethod on projBatch.procurement_method equals bpm.proc_code into bpmt
                               from procMethod in bpmt.DefaultIfEmpty()

                               join pg in _grantee on pr.grantee equals pg.id.ToString() into pgt
                               from projGran in pgt.DefaultIfEmpty()

                               where pr.id.ToString() == payload.projectSearch.id
                               select new
                               {
                                   pr,
                                   project_substatus = wf.project_substatus_desc,
                                   proj_group = wf.access_group,
                                   proj_category = pcat.proj_cat,
                                   procurement_method = procMethod.procurement_description,
                                   grantee_name = projGran.grantee_name
                               }).FirstOrDefault();
                if (_record != null && _record.pr.id > 0)
                {

                    data = _record.pr.ToView();
                    data.category_desc = _record.proj_category;
                    data.session_group_id = _record.proj_group.ToString();
                    data.project_substatus_desc = _record.project_substatus;
                    data.procurement_method = _record.procurement_method;
                    data.grantee_name = _record.grantee_name;
                }

                return new Result<ProjectRequestVM>() { value = data };

            }
            catch (Exception ex)
            {
                return new Result<ProjectRequestVM>() { status = new Status() { code = Constant.Status.Failed, description = ex.Message } };
            }
        }

        public Result<IEnumerable<ProjectRequestHistoryVM>> GetProjectLogs(PayloadVM payload)
        {
            try
            {
                List<ProjectRequestHistoryVM> result = new List<ProjectRequestHistoryVM>();
                var _record = (from ph in db.ProjectRequestsHistory
                               join au in db.AccessUser on ph.created_by equals au.id
                               join dept in db.Departments on au.dept_id equals dept.id
                               join wf in db.Workflows on ph.project_substatus equals wf.project_substatus
                               where ph.project_request_id.ToString() == payload.projectSearch.id
                               orderby ph.id descending
                               select new { ph, au.first_name, au.mi, au.last_name, dept.dept_description, wf.project_status_desc, wf.project_substatus_desc }
                               );
                if (_record != null && _record.Count() > 0)
                {
                    _record.ToList().ForEach(item => {
                        ProjectRequestHistoryVM i = item.ph.ToView();
                        i.created_by_name = string.Concat(item.first_name, " ", item.mi, " ", item.last_name);
                        i.department_desc = item.dept_description;
                        i.project_status_desc = item.project_status_desc;
                        i.project_substatus_desc = item.project_substatus_desc;
                        result.Add(i);
                    });
                }
                return new Result<IEnumerable<ProjectRequestHistoryVM>>() { value = result };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<ProjectRequestHistoryVM>>() { status = new Status() { code = Constant.Status.Failed, description = ex.Message } };
            }
        }
        #endregion
    }
}