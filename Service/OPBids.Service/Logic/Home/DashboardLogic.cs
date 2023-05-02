using OPBids.Entities.Common;
using OPBids.Entities.View.Home;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Data.Entity.Migrations;

using static OPBids.Common.Enum;
using OPBids.Common;
using OPBids.Service.Models.ProjectRequest;

namespace OPBids.Service.Logic.Home
{
    public class DashboardLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();


        public Result<DashboardSummaryVM> GetDashboardSummary(Payload payload)
        {
            // Retrieve data and summarize list
            XMLHelper<SummaryInfoList> xMLHelper = new XMLHelper<SummaryInfoList>();
            var summaryList = xMLHelper.Deserialize("~/App_Data/DashboardSummary.xml");

            // Set SummaryInfo by Group
            int[] item = SummaryHelper.SetSummaryInfoByGroup(payload.group_id);

            // Set Summary to Model
            DashboardSummaryVM _summary = new DashboardSummaryVM();
            if (summaryList != null && summaryList.SummaryInfos.Count > 0 && item.Length > 0)
            {
                _summary.summary1 = summaryList.SummaryInfos.Find(x => x.type == item[0]);
                _summary.summary2 = summaryList.SummaryInfos.Find(x => x.type == item[1]);
                _summary.summary3 = summaryList.SummaryInfos.Find(x => x.type == item[2]);
            }

            // Retrieve all ProjectRequest to get latest Count and Amount, based on Group_id
            List<ProjectRequest> projectList;
            List<Models.Settings.Workflow> _workflow;
            using (DatabaseContext db = new DatabaseContext())
            {
                var _qry_substatus = (from wf in db.Workflows select new { wf.project_substatus }).Distinct();

                projectList = (from list in db.ProjectRequests
                               join ss in _qry_substatus on list.project_substatus equals ss.project_substatus
                               where list.record_status == Constant.RecordStatus.Active
                               //join flow in db.Workflows on list.project_status equals flow.project_status //causing duplicate records
                               select list).ToList();

                _workflow = (from wf in db.Workflows
                             select wf).ToList();
            }

            // Update count and amount to model
            SummaryHelper.UpdateTotalAmount(_summary.summary1, payload.group_id, projectList, _workflow);
            SummaryHelper.UpdateTotalAmount(_summary.summary2, payload.group_id, projectList, _workflow);
            SummaryHelper.UpdateTotalAmount(_summary.summary3, payload.group_id, projectList, _workflow);

            Result<DashboardSummaryVM> _result = new Result<DashboardSummaryVM>();
            _result.value = _summary;
            return _result;
        }

        //public Result<IEnumerable<DashboardTableResultVM>> GetDashboardTable(DashboardPayloadVM payload)
        //{
        //    List<DashboardTableResultVM> _list = new List<DashboardTableResultVM>();
        //    try
        //    {
        //        using (DatabaseContext db = new DatabaseContext())
        //        {
        //            int toprecords = Convert.ToInt16(ConfigManager.DashboardRecord);

        //            //TODO: Get current year only
        //            var projReq = (from pr in db.ProjectRequests
        //                           join cat in db.ProjectCategory on pr.category equals cat.id.ToString() into catt
        //                           from pcat in catt.DefaultIfEmpty()
        //                           where pr.record_status == Constant.RecordStatus.Active
        //                           select new { pr, proj_category = pcat.proj_cat }).Take(toprecords).ToList();
        //            if (projReq != null && projReq.Count > 0)
        //            {
        //                projReq.ForEach(project => {
        //                    //TODO:
        //                    //category
        //                    //aging in days     task_deadline = routed_date + sla 
        //                    //                  aging = task_deadline - currend_date
        //                    //status

        //                    _list.Add(new DashboardTableResultVM()
        //                    {
        //                        refNo = project.pr.id,
        //                        category = project.proj_category,
        //                        agingInDays = -1,
        //                        project = project.pr.title,
        //                        amount = project.pr.estimated_budget,
        //                        status = "0% COMPLETED",
        //                        //agingInStatus = 3,
        //                        //agingInTWG = 4,
        //                        dateSubmitted = project.pr.created_date,
        //                        monitored = true
        //                    });
        //                });
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Exception handler
        //    }

        //    Result<IEnumerable<DashboardTableResultVM>> _result = new Result<IEnumerable<DashboardTableResultVM>>();
        //    _result.value = _list;

        //    return _result;
        //}

        public Result<DashboardChartsVM> GetDashboardCharts(Payload payload)
        {
            DashboardChartsVM model = new DashboardChartsVM();


            // Get Pie Chart
            model.ChartGroup1 = ChartHelper.GetPieChart(db, payload.id);

            // Get Bar Line Chart
            model.ChartGroup2 = ChartHelper.GetBarLineChart(db, payload.id);

            return new Result<DashboardChartsVM>() { value = model };
        }

        public Result<TWGResultHeadersVM> GetTWGResultHeaders(DashboardPayloadVM payload)
        {
            TWGResultHeadersVM model = new TWGResultHeadersVM();
            model.forEvaluation = new TWGResultHeadersVM.TWGHeaderItem()
            {
                amount = "724,292,195.53",
                count = 55
            };
            model.forDocumentSubmission = new TWGResultHeadersVM.TWGHeaderItem()
            {
                amount = "492,102,100",
                count = 23
            };
            model.forPostQualification = new TWGResultHeadersVM.TWGHeaderItem()
            {
                amount = "310,028,971.46",
                count = 42
            };


            Result<TWGResultHeadersVM> _result = new Result<TWGResultHeadersVM>();
            _result.value = model;
            return _result;
        }

        public Result<IEnumerable<DashboardTableResultVM>> GetDashboardTable(DashboardPayloadVM payload)
        {
            List<DashboardTableResultVM> _list = new List<DashboardTableResultVM>();
            try
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    int toprecords = Convert.ToInt16(ConfigManager.DashboardRecord);

                    List<string> lstStatus = new List<string>();
                    List<string> lstSubStatus = new List<string>();

                    var predicate = PredicateBuilder.True<ProjectRequest>();

                    predicate = predicate.And(p => p.record_status == Constant.RecordStatus.Active && p.updated_date.Year == DateTime.Today.Year);//2018 



                    if (!string.IsNullOrWhiteSpace(payload.status))
                    {


                        if (payload.status == "1")//TODO: use enum
                        {
                            lstStatus.Add(Constant.ProjectRequest.ProjectStatus.ProjectInstallation);


                            if (payload.completion.Contains("0"))
                            {
                                lstSubStatus.Add(Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_Initial);
                            }
                            if (payload.completion.Contains("1"))
                            {
                                lstSubStatus.Add(Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_40Percent);
                            }
                            if (payload.completion.Contains("2"))
                            {
                                lstSubStatus.Add(Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_80Percent);

                                //lstStatus.Add(Constant.ProjectRequest.ProjectStatus.Completed);
                                //lstSubStatus.Add(Constant.ProjectRequest.ProjectSubStatus.Completed);
                            }

                            if (lstStatus.Count() > 0)
                            {
                                predicate = predicate.And(p => lstStatus.Contains(p.project_status));

                            }
                            if (lstSubStatus.Count() > 0)
                            {
                                predicate = predicate.And(p => lstSubStatus.Contains(p.project_substatus));
                            }


                        }
                        else //"2": under bac procurement
                        {
                            var q1 = (from w in db.Workflows
                                      where w.access_group.Contains(((int)Common.Enum.AccessGroups.TWG).ToString())
                                      select new { w.access_group, w.project_status }
                                      ).ToList();

                            var q2 = q1.Where(q => q.access_group.Split(',').Contains(((int)Common.Enum.AccessGroups.TWG).ToString())).Select(q => q.project_status).ToList();


                            predicate = predicate.And(p => p.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval
                            || p.project_substatus == Constant.ProjectRequest.ProjectSubStatus.OpenForBidding
                            || q2.Contains(p.project_status));//TODO: verify status

                        }

                    }

                    //TODO: Get current year only
                    var projReq = (payload.view_all)
                        ?
                        (from pr in db.ProjectRequests.Where(predicate)
                         join cat in db.ProjectCategory on pr.category equals cat.id.ToString() into catt
                         from pcat in catt.DefaultIfEmpty()
                             //where pr.record_status == Constant.RecordStatus.Active
                         select new { pr, proj_category = pcat.proj_cat }).ToList()
                        :
                        (from pr in db.ProjectRequests.Where(predicate)
                         join cat in db.ProjectCategory on pr.category equals cat.id.ToString() into catt
                         from pcat in catt.DefaultIfEmpty()
                             //where pr.record_status == Constant.RecordStatus.Active
                         select new { pr, proj_category = pcat.proj_cat }).Take(toprecords).ToList();


                    if (projReq != null && projReq.Count > 0)
                    {
                        projReq.ForEach(project => {
                            //TODO:
                            //category
                            //aging in days     task_deadline = routed_date + sla 
                            //                  aging = task_deadline - currend_date
                            //status

                            var completion = "0% COMPLETED";
                            int aging = 0;

                            if (project.pr.project_status == Constant.ProjectRequest.ProjectStatus.ProjectInstallation)
                            {
                                var task_deadline = project.pr.routed_date.GetValueOrDefault(DateTime.Today).AddDays(project.pr.sla);
                                aging = (task_deadline - DateTime.Today).Days;

                                if (project.pr.project_substatus == Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_Initial)
                                {
                                    completion = "<50% COMPLETED";
                                }
                                else if (project.pr.project_substatus == Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_40Percent)
                                {
                                    completion = "50-80% COMPLETED";
                                }
                                else if (project.pr.project_substatus == Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_80Percent)
                                {
                                    completion = ">80% COMPLETED";
                                }
                            }

                            var agingStatus = (DateTime.Today - project.pr.updated_date).Days;//TODO: verify formula


                            _list.Add(new DashboardTableResultVM()
                            {
                                refNo = project.pr.id,
                                category = project.proj_category,
                                agingInDays = aging,
                                project = project.pr.title,
                                amount = project.pr.estimated_budget,
                                status = completion,
                                agingInStatus = agingStatus,
                                //agingInTWG = 4,
                                dateSubmitted = project.pr.created_date,
                                monitored = true
                            });
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                //TODO: Exception handler
                LogHelper.LogError(ex, "Dashboard Table End User");
            }

            Result<IEnumerable<DashboardTableResultVM>> _result = new Result<IEnumerable<DashboardTableResultVM>>();
            _result.value = _list;

            return _result;
        }

        public Result<List<ProjectRequest>> TopMonitored(int take, int skip)
        {
            Result<List<ProjectRequest>> _result = new Result<List<ProjectRequest>>();

            using (DatabaseContext db = new DatabaseContext())
            {
                _result.value = (from pr in db.ProjectRequests
                                 join mo in db.MonitoredProjects on pr.id equals mo.project_request_id
                                 where pr.record_status == Constant.RecordStatus.Active
                                 orderby pr.estimated_budget descending
                                 select pr
                       ).Skip(skip).Take(take).ToList();
            }
            return _result;
        }

    }
}
