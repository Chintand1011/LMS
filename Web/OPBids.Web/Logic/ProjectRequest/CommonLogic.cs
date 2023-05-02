using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.Setting;
using OPBids.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPBids.Web.Logic.ProjectRequest
{
    public class CommonLogic : LogicBase
    {
        #region Context
        public CommonLogic(HttpRequestBase httpContext)
        {
            this._context = httpContext;
        }
        #endregion

        public ActionResult AddAttachment(ProjectRequestAttachmentVM attachment)
        {

            Result<int> _result;

            ApiManager<Result<int>> apiManager = new ApiManager<Result<int>>();

            if (this._context.IsAuthenticated)
            {
                attachment.created_by= Convert.ToInt16(base.UserID);
                attachment.updated_by = Convert.ToInt16(base.UserID);
                _result = apiManager.Invoke(
                                   ConfigManager.BaseServiceURL,
                                   Constant.ServiceEnpoint.ProjectRequest.CreateProjectRequestAttachment, attachment);

                if (_result.status.code == Constant.Status.Success)
                {
                    return new JsonResult { Data = _result.value };
                }
            }
            
            return new EmptyResult();
            
            
        }

        public override string HeaderView(string subMenuId)
        {
            throw new NotImplementedException();
        }

        public override ActionResult ProjectAttachments(PayloadVM payload)
        {
            
            Result<IEnumerable<ProjectRequestAttachmentVM>> _result;

            ApiManager<Result<IEnumerable<ProjectRequestAttachmentVM>>> apiManager = new ApiManager<Result<IEnumerable<ProjectRequestAttachmentVM>>>();

            
            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestAttachments, payload);

            if (_result.status.code == Constant.Status.Success)
            {
                return new JsonResult { Data = _result.value };
            }
            return new EmptyResult();

        }

        public override ActionResult ResultView(PayloadVM payload)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Save(PayloadVM payload)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Search(PayloadVM payload)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<ProjectGranteeVM> GetProjectGranteeFilter()
        {
            var apiManager = new ApiManager<Result<IEnumerable<ProjectGranteeVM>>>();
            var _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.ProjectRequest.GetProjectGranteesFilter, new PayloadVM());

            if (_result.status.code == Constant.Status.Success)
            {
                return _result.value;
            }
            return new List<ProjectGranteeVM>();
        }
        public IEnumerable<ProjectGranteeVM> GetProjectGranteeFilter(PayloadVM payload)
        {
            Result<IEnumerable<ProjectGranteeVM>> _result;

            ApiManager<Result<IEnumerable<ProjectGranteeVM>>> apiManager = new ApiManager<Result<IEnumerable<ProjectGranteeVM>>>();


            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.ProjectRequest.GetProjectGranteesFilter, payload);

            if (_result.status.code == Constant.Status.Success)
            {
                return _result.value;
            }
            return new List<ProjectGranteeVM>();
        }
        public static IEnumerable<ProjectCategoryVM> GetProjectCategoryFilter()
        {
            var apiManager = new ApiManager<Result<IEnumerable<ProjectCategoryVM>>>();
            var _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.ProjectRequest.GetProjectCategoriesFilter, new PayloadVM());
            if (_result.status.code == Constant.Status.Success)
            {
                return _result.value;
            }
            return new List<ProjectCategoryVM>();
        }
        public IEnumerable<ProjectCategoryVM> GetProjectCategoryFilter(PayloadVM payload)
        {
            Result<IEnumerable<ProjectCategoryVM>> _result;

            ApiManager<Result<IEnumerable<ProjectCategoryVM>>> apiManager = new ApiManager<Result<IEnumerable<ProjectCategoryVM>>>();


            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.ProjectRequest.GetProjectCategoriesFilter, payload);

            if (_result.status.code == Constant.Status.Success)
            {
                return _result.value;
            }
            return new List<ProjectCategoryVM>();
        }

        public ActionResult ProjectItems(PayloadVM payload)
        {   
            Result<IEnumerable<ProjectRequestItemVM>> _result;

            ApiManager<Result<IEnumerable<ProjectRequestItemVM>>> apiManager = new ApiManager<Result<IEnumerable<ProjectRequestItemVM>>>();

            _result = apiManager.Invoke(
                                   ConfigManager.BaseServiceURL,
                                   Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestItems, payload);

            if (_result.status.code == Constant.Status.Success)
            {
                return new JsonResult { Data = _result.value };
            }
            return new EmptyResult();         
        }

        public ActionResult ProjectInfo(PayloadVM payload)
        {
            Result<IEnumerable<ProjectRequestVM>> _result;

            ApiManager<Result<IEnumerable<ProjectRequestVM>>> apiManager = new ApiManager<Result<IEnumerable<ProjectRequestVM>>>();

            _result = apiManager.Invoke(
                                   ConfigManager.BaseServiceURL,
                                   Constant.ServiceEnpoint.ProjectRequest.GetProjectInfo, payload);

            if (_result.status.code == Constant.Status.Success)
            {
                return new JsonResult { Data = _result.value };
            }
            return new EmptyResult();
        }
    }
}