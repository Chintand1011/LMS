using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static OPBids.Common.Enum;
namespace OPBids.Web.Helper
{
    public class CssHelper
    {
        public static string GetSurveyTypeIconCls(int value)
        {
            if (Enum.IsDefined(typeof(SurveyType), value))
            {
                SurveyType type = (SurveyType)value;

                switch(type)
                {
                   // case SurveyType.Summary: return "icon-awarded";
                    case SurveyType.Awarded: return "icon-awarded";
                    case SurveyType.BudgetApproved: return "icon-approved";
                    case SurveyType.BudgetForApproval: return "icon-approval";
                    case SurveyType.ForPostQualification: return "icon-checklist";
                    case SurveyType.UnderBacProcurement: return "icon-procurement";
                    case SurveyType.UnderImplementation: return "icon-ongoing";
                    case SurveyType.ForRankingAndEvaluation: return "icon-ranking";
                    case SurveyType.TopMonitored: return "icon-star";
                }
            }
            return string.Empty;
        }

    }
}