using OPBids.Common;
using OPBids.Entities.View.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static OPBids.Common.Enum;

namespace OPBids.Service.Logic.Home
{
    public static class SummaryHelper
    {
        public static int[] SetSummaryInfoByGroup(int group_id)
        {
            int[] arr = new int[3];
            switch (group_id)
            {
                case (int)AccessGroups.BACSEC:
                    arr = new int[] { (int)SurveyType.SummaryBACSEC, (int)SurveyType.UnderBacProcurement, (int)SurveyType.UnderImplementation };
                    break;
                case (int)AccessGroups.ENDUSER:
                    arr = new int[] { (int)SurveyType.SummaryEndUser, (int)SurveyType.UnderBacProcurement, (int)SurveyType.UnderImplementation };
                    break;
                case (int)AccessGroups.BUDGET:
                    arr = new int[] { (int)SurveyType.SummaryBudget, (int)SurveyType.BudgetForApproval, (int)SurveyType.TopMonitored };
                    break;
                case (int)AccessGroups.TWG:
                    arr = new int[] { (int)SurveyType.SummaryTWG, (int)SurveyType.ForRankingAndEvaluation, (int)SurveyType.ForPostQualification };
                    break;
                case (int)AccessGroups.BAC:
                    arr = new int[] { (int)SurveyType.SummaryBAC, (int)SurveyType.UnderBacProcurement, (int)SurveyType.UnderImplementation };
                    break;
                case (int)AccessGroups.SUPPLIER:
                    arr = new int[] { (int)SurveyType.SummarySupplier, (int)SurveyType.SupplierOnGoing, (int)SurveyType.LostOpportunities };
                    break;
                case (int)AccessGroups.HOPE:
                    arr = new int[] { (int)SurveyType.SummaryHope, (int)SurveyType.HopeForApproval, (int)SurveyType.UnderImplementation };
                    break;
                //for testing only - start
                default:
                    arr = new int[] { (int)SurveyType.SummaryBACSEC, (int)SurveyType.UnderBacProcurement, (int)SurveyType.UnderImplementation };
                    break;
                    //for testing only - end
            }
            return arr;

        }
        /// <summary>
        /// This will update the Amount and Count based on the summaryItem type
        /// </summary>
        /// <param name="info"></param>
        /// <param name="projectRequests"></param>
        public static void UpdateTotalAmount(SummaryInfo info, int group_id, List<Models.ProjectRequest.ProjectRequest> projectRequests, List<Models.Settings.Workflow> workflows)
        {
            if (info != null)
            {
                info.items.ForEach(i =>
                {
                    switch (i.id)
                    {
                        /* ================= SUMMARY TABLE 1 =====================*/
                        #region SUMMARY TABLE 1
                        case Constant.Dashboard.SummaryItem.Completed:
                            GetCompleted(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.OnGoing:
                            GetOnGoing(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Delayed:
                            GetDelayed(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Drafts:
                            GetDrafts(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Approved:
                            GetApproved(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.ForApproval:
                            GetForApproval(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Rejected:
                            GetRejected(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Recommended:
                            GetRecommeneded(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Incoming:
                            GetIncoming(projectRequests, i);
                            break;
                        #endregion
                        /* ================= SUMMARY TABLE 1 =====================*/


                        /* ================= SUMMARY TABLE 2 =====================*/
                        #region SUMMARY TABLE 2
                        case Constant.Dashboard.SummaryItem.UnderTWG:
                            GetUnderTWG(projectRequests, i, workflows);
                            break;
                        case Constant.Dashboard.SummaryItem.PreBid:
                            GetPreBid(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.BudgetApproval:
                            GetBudgetApproval(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.NewlyRequest:
                            GetNewlyRequest(projectRequests, i);
                            break;

                        case Constant.Dashboard.SummaryItem.Past10Days:
                            GetPast10Days(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Days10Below:
                            GetDays10Below(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.NewlySubmitted:
                            GetNewlySubmitted(projectRequests, i);
                            break;

                        case Constant.Dashboard.SummaryItem.Past21Days:
                            GetPast21Days(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Between1321:
                            GetBetween1321(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Less13:
                            GetLess13(projectRequests, i);
                            break;

                        case Constant.Dashboard.SummaryItem.UnderImplementation:
                            GetUnderImplementation(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.BACResolution:
                            GetBACResolution(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.AwardByTWG:
                            GetAwardByTWG(projectRequests, i);
                            break;

                        case Constant.Dashboard.SummaryItem.Past30Days:
                            GetPast30Days(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Between1530:
                            GetBetween1530(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Less15:
                            GetLess15(projectRequests, i);
                            break;

                        #endregion
                        /* ================= SUMMARY TABLE 2 =====================*/


                        /* ================= SUMMARY TABLE 3 =====================*/
                        #region SUMMARY TABLE 3
                        case Constant.Dashboard.SummaryItem.Top1:
                            GetTop1(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Top2:
                            GetTop2(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Top3:
                            GetTop3(projectRequests, i);
                            break;

                        case Constant.Dashboard.SummaryItem.Past17Days:
                            GetPast17Days(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Between1017:
                            GetBetween1017(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Less10:
                            GetLess10(projectRequests, i);
                            break;

                        case Constant.Dashboard.SummaryItem.Missed:
                            GetMissed(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Unawarded:
                            GetUnawarded(projectRequests, i);
                            break;

                        #endregion
                        /* ================= SUMMARY TABLE 3 =====================*/


                        /* ================= SUMMARY SHARED TABLE =====================*/
                        #region SUMMARY TABLE 3
                        case Constant.Dashboard.SummaryItem.Above80:
                            GetAbove80(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Between5080:
                            GetBetween5080(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.Less50:
                            GetLess50(projectRequests, i);
                            break;
                        case Constant.Dashboard.SummaryItem.UILess10:
                            GetLess10(projectRequests, i);
                            break;
                        #endregion
                        /* ================= SUMMARY SHARED TABLE =====================*/

                        default:
                            break;
                    }
                });
            }
        }

        #region Calculators

        #region SUMMARY TABLE 1

        private static void GetCompleted(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                               //where pr.record_status == Constant.RecordStatus.Completed
                           where pr.project_status == Constant.ProjectRequest.ProjectStatus.Completed
                           group pr by pr.project_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetOnGoing(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && (pr.project_status != Constant.ProjectRequest.ProjectStatus.Draft && pr.project_status != Constant.ProjectRequest.ProjectStatus.Completed)
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetDelayed(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active && pr.required_date <= DateTime.Today
                           && (pr.project_status != Constant.ProjectRequest.ProjectStatus.Draft && pr.project_status != Constant.ProjectRequest.ProjectStatus.Completed)
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetDrafts(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                           where pr.project_status == Constant.ProjectRequest.ProjectStatus.Draft
                           group pr by pr.project_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetApproved(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active &&
                           (
                                pr.project_status != Constant.ProjectRequest.ProjectStatus.Draft &&
                                pr.project_status != Constant.ProjectRequest.ProjectStatus.NewlyRequested &&
                                pr.project_status != Constant.ProjectRequest.ProjectStatus.ForBudgetApproval &&
                                pr.project_status != Constant.ProjectRequest.ProjectStatus.Completed
                               )
                           //&& pr.project_substatus == Constant.ProjectRequest.ProjectSubStatus.ITB_Preparation
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetForApproval(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && pr.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetRejected(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: How to get rejected
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Rejected
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetRecommeneded(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active && pr.project_substatus == Constant.ProjectRequest.ProjectSubStatus.ForRecommendation
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetIncoming(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            // TODO: How to get incoming
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && pr.project_substatus == Constant.ProjectRequest.ProjectSubStatus.Draft
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        #endregion

        #region SUMMARY TABLE 2

        private static void GetUnderTWG(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem, List<Models.Settings.Workflow> workflows)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                               //join wf in workflows on pr.project_status equals wf.project_status
                           where pr.record_status == Constant.RecordStatus.Active &&
                                (pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Eval ||
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_ForPostQual ||
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_LCB ||
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_PostEval ||
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_PostQual ||
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Ranking
                           //&& wf.access_group.Split(',').ToList().Contains(Common.Enum.AccessGroups.TWG.ToString()
                           )
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetPreBid(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && (pr.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_BudgetApproved ||
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_Hope ||
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_BACSEC ||
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_Posting ||
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_Close_ReOpen
                           )
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetBudgetApproval(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active && pr.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetNewlyRequest(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: Get New request
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active && pr.project_status == Constant.ProjectRequest.ProjectStatus.NewlyRequested
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetPast10Days(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: Get Past 10 Days

            var startDate = DateTime.Now.Date.AddDays(-10);
            var endDate = DateTime.Now.Date;

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active &&
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval &&
                                pr.required_date.Date >= startDate && pr.required_date.Date <= endDate

                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetDays10Below(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: GetDays10Below

            var endDate = DateTime.Now.Date.AddDays(-10);

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active &&
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval &&
                                pr.required_date.Date < endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetNewlySubmitted(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: GetNewlySubmitted
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active &&
                                pr.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval &&
                                pr.updated_date.Date == DateTime.Now.Date
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetPast21Days(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            var endDate = DateTime.Now.Date.AddDays(-21);

            //TODO: Get GetPast21Days
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                            && (
                            pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Ranking
                            || pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Eval
                            )
                            && pr.updated_date.Date <= endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.approved_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetBetween1321(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            var startDate = DateTime.Now.Date.AddDays(-21);
            var endDate = DateTime.Now.Date.AddDays(-13);

            //TODO: Get GetBetween1321
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                            && (
                            pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Ranking
                            || pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Eval
                            )
                            && pr.updated_date.Date >= startDate && pr.updated_date.Date <= endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.approved_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetLess13(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            var startDate = DateTime.Now.Date.AddDays(-13);
            var endDate = DateTime.Now.Date;

            //TODO: Get GetLess13
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && (
                            pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Ranking
                            || pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Eval
                            )
                            && pr.updated_date.Date >= startDate && pr.updated_date.Date <= endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.approved_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetUnderImplementation(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: Get GetUnderImplementation
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetBACResolution(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: Get GetBACResolution
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetAwardByTWG(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: Get GetAwardByTWG
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetPast30Days(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            // TODO: Get GetPast30Days
            var endDate = DateTime.Now.Date.AddDays(-30);

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && pr.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_Hope
                           && pr.updated_date <= endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetBetween1530(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            // TODO: Get GetBetween1530
            var startDate = DateTime.Now.Date.AddDays(-30);
            var endDate = DateTime.Now.Date.AddDays(-15);

            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && pr.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_Hope
                           && pr.updated_date.Date >= startDate && pr.updated_date.Date < endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetLess15(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            // TODO: Get GetLess15
            var startDate = DateTime.Now.Date.AddDays(-15);
            var endDate = DateTime.Now.Date;
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                             && pr.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_Hope
                           && pr.updated_date.Date >= startDate && pr.updated_date.Date < endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        #endregion

        #region SUMMARY TABLE 3

        private static void GetTop1(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {

            var topMon = new DashboardLogic().TopMonitored(1, 0);

            var _result = (from pr in topMon.value
                           where pr.record_status == Constant.RecordStatus.Active
                           group pr by pr.id into final
                           select new { ProjectName = final.First().title, TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.label = _result.ProjectName;
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetTop2(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            var topMon = new DashboardLogic().TopMonitored(1, 1);

            var _result = (from pr in topMon.value
                           where pr.record_status == Constant.RecordStatus.Active
                           group pr by pr.id into final
                           select new { ProjectName = final.First().title, TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.label = _result.ProjectName;
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetTop3(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            var topMon = new DashboardLogic().TopMonitored(1, 2);

            var _result = (from pr in topMon.value
                           where pr.record_status == Constant.RecordStatus.Active
                           group pr by pr.id into final
                           select new { ProjectName = final.First().title, TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.label = _result.ProjectName;
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetPast17Days(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            var endDate = DateTime.Now.Date.AddDays(-17);

            //TODO: Get GetPast17Days
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_ForPostQual
                           && pr.updated_date.Date < endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetBetween1017(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            var startDate = DateTime.Now.Date.AddDays(-17);
            var endDate = DateTime.Now.Date.AddDays(-10);
            //TODO: Get GetBetween1017
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_ForPostQual
                           && pr.updated_date.Date >= startDate && pr.updated_date.Date < endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetLess10(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            var startDate = DateTime.Now.Date.AddDays(-10);
            var endDate = DateTime.Now.Date;
            //TODO: Get GetLess10
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && pr.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_ForPostQual
                           && pr.updated_date.Date >= startDate && pr.updated_date.Date < endDate
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetMissed(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            // Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: Get GetMissed
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Cancelled
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetUnawarded(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //TODO: Get GetUnawarded
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && pr.project_substatus == Constant.ProjectRequest.ProjectSubStatus.CloseForShortlising
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        #endregion

        #region SHARED TABLE

        private static void GetAbove80(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            // TODO: Calculate GetAbove80
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                            && (
                            pr.project_substatus == "PSS-16.63"
                            )
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetBetween5080(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            // TODO: Calculate GetBetween5080
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && (
                            pr.project_substatus == "PSS-16.5"
                            || pr.project_substatus == "PSS-16.6"
                            || pr.project_substatus == "PSS-16.61"
                            || pr.project_substatus == "PSS-16.62"
                           )
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }

        private static void GetLess50(List<Models.ProjectRequest.ProjectRequest> projectRequests, SummaryItem summaryItem)
        {
            //Dictionary<string, string> keyValues = new Dictionary<string, string>();
            // TODO: Calculate GetLess50
            var _result = (from pr in projectRequests
                           where pr.record_status == Constant.RecordStatus.Active
                           && (
                            pr.project_substatus == "PSS-16.1"
                            || pr.project_substatus == "PSS-16.2"
                            || pr.project_substatus == "PSS-16.3"
                            || pr.project_substatus == "PSS-16.4"
                           )
                           group pr by pr.record_status into final
                           select new { TotalAmount = final.Sum(x => x.estimated_budget), TotalCount = final.Count() }).FirstOrDefault();
            if (_result != null)
            {
                summaryItem.value = _result.TotalAmount;
                summaryItem.count = _result.TotalCount;
            }
        }
        #endregion

        #endregion
    }
}