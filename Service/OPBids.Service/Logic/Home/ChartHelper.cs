using OPBids.Common;
using OPBids.Entities.View.Home;
using OPBids.Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPBids.Service.Logic.Home
{
    public static class ChartHelper
    {
        #region PIE CHART
        public static ChartGroup GetPieChart(DatabaseContext db, int dashboard_id)
        {
            //TODO: Create Constants for Dashboard ID
            switch (dashboard_id)
            {
                case 2:
                    // AGM
                    return PieByCategory(db, dashboard_id);

                case 5:
                    // USER
                    return PieByCategory(db, dashboard_id);

                case 6:
                    // Budget
                    return PieByCategory(db, dashboard_id);

                case 4:
                    // BACSEC
                    return PieByCategory(db, dashboard_id);

                case 7:
                    // HOPE
                    return PieByCategory(db, dashboard_id);

                case 11:
                    // Supplier
                    return PieByCategory(db, dashboard_id);

                case 3:
                    // BAC
                    return PieByCategory(db, dashboard_id);

                case 8:
                    // TWG
                    return PieByEndUser(db, dashboard_id);

                default:
                    return PieByCategory(db, dashboard_id);
            }
        }

        //TODO
        private static ChartGroup PieByCategory(DatabaseContext db, int dashboard_id)
        {


            var projCat = (from pr in db.ProjectRequests
                           join cat in db.ProjectCategory on pr.category equals cat.id.ToString()
                           where pr.record_status == Constant.RecordStatus.Active && pr.updated_date.Year == DateTime.Today.Year
                           group pr by new { id = pr.category, cat.proj_cat } into grp
                           select new { id = grp.Key.id, proj_cat = grp.Key.proj_cat, count = grp.Count(), amount = grp.Sum(x => x.estimated_budget) })
                           .OrderByDescending(r => r.amount).ToList();

            ChartGroup _result = new ChartGroup()
            {
                title = "Project Density <span style=\"font-size:16px;\">by</span> Category",
                columnDescription = "Category",
                data = new List<ChartData>()
            };

            for (int indx = 0; indx < projCat.Count && indx < 5; indx++)
            {
                _result.data.Add(
                     new ChartData()
                     {
                         name = projCat[indx].proj_cat,
                         count = projCat[indx].count,
                         amount = projCat[indx].amount
                     }
                    );
            }

            int otherCount = 0;
            decimal otherAmout = 0;
            for (int indx = 5; indx < projCat.Count; indx++)
            {
                otherCount += projCat[indx].count;
                otherAmout += projCat[indx].amount;
            }

            if (otherCount > 0)
            {
                _result.data.Add(
                     new ChartData()
                     {
                         name = "Others",
                         count = otherCount,
                         amount = otherAmout
                     }
                    );
            }

            return _result;

        }

        //TODO
        private static ChartGroup PieByArea(DatabaseContext db, int dashboard_id)
        {
            return new ChartGroup()
            {
                title = "Project Density <span style=\"font-size:16px;\">by</span> Area",
                columnDescription = "Area",
                data = new List<ChartData>()
                {
                    new ChartData()
                    {
                        name = "Cubao",
                        count = 1,
                        amount = 940250
                    },
                    new ChartData()
                    {
                        name = "Muntinlupa",
                        count = 2,
                        amount = 759258
                    },
                    new ChartData()
                    {
                        name = "Taguig",
                        count = 3,
                        amount = 699079
                    },
                    new ChartData()
                    {
                        name = "Cavite",
                        count = 2,
                        amount = 606400
                    },
                    new ChartData()
                    {
                        name = "Zamboanga",
                        count = 2,
                        amount = 410000
                    },
                    new ChartData()
                    {
                        name = "Others",
                        count = 11,
                        amount = 6177866
                    }
                }
            };
        }

        //TODO
        private static ChartGroup PieByEndUser(DatabaseContext db, int dashboard_id)
        {

            var projCat = (from pr in db.ProjectRequests
                           where pr.record_status == Constant.RecordStatus.Active && pr.updated_date.Year == DateTime.Today.Year
                           group pr by pr.grantee into grp
                           select new { name = grp.Key, count = grp.Count(), amount = grp.Sum(x => x.estimated_budget) })
                           .OrderByDescending(r => r.amount).ToList();

            ChartGroup _result = new ChartGroup()
            {
                title = "Project Density <span style=\"font-size:16px;\">by</span> End User",
                columnDescription = "End User",
                data = new List<ChartData>()
            };

            for (int indx = 0; indx < projCat.Count && indx < 5; indx++)
            {
                _result.data.Add(
                     new ChartData()
                     {
                         name = projCat[indx].name,
                         count = projCat[indx].count,
                         amount = projCat[indx].amount
                     }
                    );
            }

            int otherCount = 0;
            decimal otherAmout = 0;
            for (int indx = 5; indx < projCat.Count; indx++)
            {
                otherCount += projCat[indx].count;
                otherAmout += projCat[indx].amount;
            }

            if (otherCount > 0)
            {
                _result.data.Add(
                     new ChartData()
                     {
                         name = "Others",
                         count = otherCount,
                         amount = otherAmout
                     }
                    );
            }
            return _result;
            //var projCat = (from pr in db.ProjectRequests
            //               join cat in db.ProjectCategory on pr.category equals cat.id.ToString()
            //               where pr.record_status == Constant.RecordStatus.Active && pr.updated_date.Year == DateTime.Today.Year
            //               group pr by new { id = pr.category, cat.proj_cat } into grp
            //               select new { id = grp.Key.id, proj_cat = grp.Key.proj_cat, count = grp.Count(), amount = grp.Sum(x => x.estimated_budget) })
            //               .OrderByDescending(r => r.amount).ToList();

            //ChartGroup _result = new ChartGroup()
            //{
            //    title = "Project Density by Category",
            //    columnDescription = "Category",
            //    data = new List<ChartData>()
            //};

            //for (int indx = 0; indx < projCat.Count && indx < 5; indx++)
            //{
            //    _result.data.Add(
            //         new ChartData()
            //         {
            //             name = projCat[indx].proj_cat,
            //             count = projCat[indx].count,
            //             amount = projCat[indx].amount
            //         }
            //        );
            //}

            //int otherCount = 0;
            //decimal otherAmout = 0;
            //for (int indx = 5; indx < projCat.Count; indx++)
            //{
            //    otherCount += projCat[indx].count;
            //    otherAmout += projCat[indx].amount;
            //}

            //if (otherCount > 0)
            //{
            //    _result.data.Add(
            //         new ChartData()
            //         {
            //             name = "Others",
            //             count = otherCount,
            //             amount = otherAmout
            //         }
            //        );
            //}


            //return new ChartGroup()
            //{
            //    title = "Project Density by End User",
            //    columnDescription = "End User",
            //    data = new List<ChartData>()
            //    {
            //        new ChartData()
            //        {
            //            name = "Office of the Chairman",
            //            count = 1,
            //            amount = 940250
            //        },
            //        new ChartData()
            //        {
            //            name = "Office of the GM",
            //            count = 2,
            //            amount = 759258
            //        },
            //        new ChartData()
            //        {
            //            name = "Office of the DC",
            //            count = 3,
            //            amount = 699079
            //        },
            //        new ChartData()
            //        {
            //            name = "FC & MS",
            //            count = 2,
            //            amount = 606400
            //        },
            //        new ChartData()
            //        {
            //            name = "Pasig River FS",
            //            count = 2,
            //            amount = 410000
            //        },
            //        new ChartData()
            //        {
            //            name = "Public",
            //            count = 11,
            //            amount = 6177866
            //        }
            //    }
            //};
        }
        #endregion

        #region BAR-LINE CHART

        public static ChartGroup GetBarLineChart(DatabaseContext db, int dashboard_id)
        {
            var chartData = new List<ChartData>();

            var currYear = DateTime.Today.Year;//2018

            var newRequest = (from list in db.ProjectRequests
                              where list.record_status == Constant.RecordStatus.Active
                                    && list.updated_date.Year == currYear
                                    && list.project_status == Constant.ProjectRequest.ProjectStatus.NewlyRequested
                              group list by list.project_status into grp
                              select new { count = grp.Count(), amount = grp.Sum(g => g.estimated_budget) }).ToList();
            chartData.Add(
                new ChartData()
                {
                    name = "Newly Requested",
                    count = newRequest.Sum(q => q.count),
                    amount = newRequest.Sum(q => q.amount)
                }
                );

            var forBudget = (from list in db.ProjectRequests
                             where list.record_status == Constant.RecordStatus.Active
                                && list.updated_date.Year == currYear
                                && list.project_status == Constant.ProjectRequest.ProjectStatus.ForBudgetApproval
                             group list by list.project_status into grp
                             select new { count = grp.Count(), amount = grp.Sum(g => g.estimated_budget) }).ToList();

            chartData.Add(
                new ChartData()
                {
                    name = "For Budget",
                    count = forBudget.Sum(q => q.count),
                    amount = forBudget.Sum(q => q.amount)
                }
                );



            var preBid = (from list in db.ProjectRequests
                          where list.record_status == Constant.RecordStatus.Active && list.updated_date.Year == currYear &&
                              (
                              list.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_BudgetApproved
                              || list.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_Hope
                              || list.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_BACSEC
                              || list.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_Posting
                              || list.project_status == Constant.ProjectRequest.ProjectStatus.PreBidding_Close_ReOpen
                              )
                          group list by list.project_status into grp
                          select new { count = grp.Count(), amount = grp.Sum(g => g.estimated_budget) }).ToList();

            chartData.Add(
                new ChartData()
                {
                    name = "Pre-Bid",
                    count = preBid.Sum(q => q.count),
                    amount = preBid.Sum(q => q.amount)
                }
                );


            var underTWG = (from list in db.ProjectRequests
                            where list.record_status == Constant.RecordStatus.Active && list.updated_date.Year == currYear &&
                                (
                                list.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Ranking
                                || list.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_LCB
                                || list.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_Eval
                                || list.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_PostEval
                                || list.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_ForPostQual
                                || list.project_status == Constant.ProjectRequest.ProjectStatus.UnderTWGQualification_PostQual
                                )
                            group list by list.project_status into grp
                            select new { count = grp.Count(), amount = grp.Sum(g => g.estimated_budget) }).ToList();

            chartData.Add(
                new ChartData()
                {
                    name = "Under TWG",
                    count = underTWG.Sum(q => q.count),
                    amount = underTWG.Sum(q => q.amount)
                }
                );


            var underImplementation = (from list in db.ProjectRequests
                                       where list.record_status == Constant.RecordStatus.Active
                                        && list.updated_date.Year == currYear
                                        && list.project_status == Constant.ProjectRequest.ProjectStatus.ProjectInstallation
                                       group list by list.project_status into grp
                                       select new { count = grp.Count(), amount = grp.Sum(g => g.estimated_budget) }).ToList();

            chartData.Add(
                new ChartData()
                {
                    name = "Under Implementation",
                    count = underImplementation.Sum(q => q.count),
                    amount = underImplementation.Sum(q => q.amount)
                }
                );


            var completed = (from list in db.ProjectRequests
                             where list.record_status == Constant.RecordStatus.Active
                             && list.updated_date.Year == currYear
                             && list.project_status == Constant.ProjectRequest.ProjectStatus.Completed
                             group list by list.project_status into grp
                             select new { count = grp.Count(), amount = grp.Sum(g => g.estimated_budget) }).ToList();
            chartData.Add(
                new ChartData()
                {
                    name = "Completed",
                    count = completed.Sum(q => q.count),
                    amount = completed.Sum(q => q.amount)
                }
                );


            //Constant.ProjectRequest.ProjectStatus.ForBudgetApproval
            return new ChartGroup()
            {
                title = "Project Status",
                columnDescription = "",
                data = chartData
            };
        }

        #endregion
    }
}