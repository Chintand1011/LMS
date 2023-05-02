using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Web.Helper;

namespace OPBids.Web.Logic.ProjectRequest
{
    public class IncomingLogic : LogicBase
    {
        #region Context
        public IncomingLogic(HttpRequestBase httpContext)
        {
            base._context = httpContext;
            base.ProjectStatus = Constant.ProjectRequest.ProjectStatus.Draft;
        }
        #endregion

        #region View

        public override string HeaderView(string subMenuId)
        {
            return Constant.ProjectRequest.HeaderView.Incoming;
        }

        public override ActionResult ResultView(PayloadVM payload)
        {
            return Search(payload);
        }
       
        #endregion

        #region CRUD


        public override ActionResult Search(PayloadVM payload)
        {
            if (payload.projectSearch == null)
            {
                return PartialView(Constant.ProjectRequest.ResultView.Incoming, new ProjectSearchResultVM()
                {
                    count = 0,
                    page_index = 1,
                    items = new List<ProjectRequestVM>()
                });
            }
            this.InitializePayload(payload);
            Result<ProjectSearchResultVM> _result = new ApiManager<Result<ProjectSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequest, payload);
            base.FormatProjectRequest(_result.value.items);
            return PartialView(Constant.ProjectRequest.ResultView.Incoming, _result.value);
        }

        public override ActionResult Save(PayloadVM payload)
        {
            throw new NotImplementedException();
        }

        public override ActionResult ProjectAttachments(PayloadVM payload)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Utilities

        private void InitializePayload(PayloadVM payload)
        {
            payload.projectSearch = payload.projectSearch == null ? new ProjectSearchVM() : payload.projectSearch;
            payload.projectSearch.record_status = Constant.RecordStatus.Active;
            payload.projectSearch.project_status = Constant.ProjectRequest.ProjectStatus.Draft;
            payload.monitoredProject = new MonitoredProjectVM() { created_by = Convert.ToInt16(base.UserID) };
        }

        #endregion
    }
}