using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Common
{
    public class Enum
    {
        public enum AccessGroups
        {
            ADMIN = 1,
            AGM = 2,
            BAC = 3,
            BACSEC = 4,
            ENDUSER = 5,
            BUDGET = 6,
            HOPE = 7,
            TWG = 8,
            DTS_FINANCE = 9,
            DTS_IT = 10,
            SUPPLIER = 11,
            PMO = 12
        }

        public enum ProcurementType
        {
            Consultation = 1,
            Goods = 2,
            Infrastructure = 3
        }

        public enum ProjectStatus
        {
            None = 0,
            Draft = 1,
            [Description("On Going")]
            OnGoing = 2,
            Completed = 3,
            Delayed = 4,
            [Description("For Budget Approval")]
            ForBudgetApproval = 5
        }

        public enum SurveyType
        {

            SummaryEndUser = 100,
            SummaryBudget = 101,
            SummaryTWG = 102,
            SummaryBACSEC = 103,
            SummaryBAC = 104,
            SummarySupplier = 105,
            SummaryHope = 106,


            [Description("Under BAC Procurement")]
            UnderBacProcurement = 1,

            [Description("Under Implementation")]
            UnderImplementation = 2,

            [Description("For Approval")]
            BudgetForApproval = 3,

            [Description("Approved")]
            BudgetApproved = 4,

            [Description("Top 3 Monitored")]
            TopMonitored = 5,

            [Description("For Ranking & Evaluation")]
            ForRankingAndEvaluation = 6,

            [Description("For Post Qualifications")]
            ForPostQualification = 7,

            Awarded = 8,

            [Description("For Approval")]
            HopeForApproval = 9,

            [Description("On Going")]
            BACOnGoing = 10,

            [Description("On Going")]
            SupplierOnGoing = 11,

            [Description("Lost Opportunities")]
            LostOpportunities = 12

        }

        public enum SearchDuration
        {
            None = 0,
            ThisWeek = 1,
            ThisMonth = 2,
            Last60Days = 3
        }
    }
}
