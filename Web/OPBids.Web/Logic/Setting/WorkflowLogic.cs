using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Web.Helper;
using System.Web.UI.WebControls;

namespace OPBids.Web.Logic.Setting
{
    public class WorkflowLogic : SettingLogicBase
    {
        private List<SelectListItem> ActorList;
        private List<SelectListItem> TypeList;

        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.Workflow;
        }

        public override ActionResult ResultView(SettingVM setting)
        {
            // Get all View Data
            SetViewData();

            switch (setting.txn)
            {
                case Constant.TransactionType.Search:
                    return Search(setting);
                case Constant.TransactionType.Save:
                    return Save(setting);
                case Constant.TransactionType.StatusUpdate:
                    return StatusUpdate(setting);
                default:
                    //default temp val until permission is set
                    return Search(setting);
                    //return null;
            }            
        }
        public override GridView DownloadExcel()
        {
            return DownloadExcel<WorkflowVM>("Workflow", Constant.ServiceEnpoint.Settings.GetWorkflow);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<WorkflowVM>("Workflow", Constant.ServiceEnpoint.Settings.GetWorkflow);
        }
        public override string HTMLTable()
        {
            return HTMLTable<WorkflowVM>(Constant.ServiceEnpoint.Settings.GetWorkflow);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetWorkflow);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            IEnumerable<WorkflowVM> _list = SearchData<WorkflowVM>(setting);
            _list = UpdateDisplayName(_list);
            return PartialView(Constant.Setting.ResultView.Workflow, _list);            
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
        public override ActionResult Save(SettingVM setting)
        {
            var _workflow = setting.workflow;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateWorkflow;

            if (_workflow.id == 0)
            {
                _workflow.created_by = 1;
                _workflow.updated_by = 1;
            }
            else
            {
                _workflow.updated_by = 1;
                curUrl = Constant.ServiceEnpoint.Settings.UpdateWorkflow;
            }
            Result<IEnumerable<WorkflowVM>> _result;
            var apiManager = new ApiManager<Result<IEnumerable<WorkflowVM>>>();
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL,
                curUrl, _workflow);

            var _list = UpdateDisplayName(_result.value);
            return PartialView(Constant.Setting.ResultView.Workflow, _list);
        }
        
        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<WorkflowVM>> _result;
            var apiManager = new ApiManager<Result<IEnumerable<WorkflowVM>>>();
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateWorkflow, setting);

            var _list = UpdateDisplayName(_result.value);
            return PartialView(Constant.Setting.ResultView.AccessTypes, _list);
        }

        private List<SelectListItem> GetAccessGroupList() {
            List<SelectListItem> actorList = new List<SelectListItem>();
            actorList.Add(new SelectListItem { Value = "", Text = Constant.PleaseSelect });

            var _list = ProcessData<AccessGroupVM>(new SettingVM(), Constant.ServiceEnpoint.Settings.GetAccessGroup);
            if (_list != null && _list.value.Count() > 0) {
                _list.value.ToList().ForEach(x => {
                    actorList.Add(new SelectListItem() { Value = x.id.ToString(), Text = x.group_code });
                });
            }
            return actorList;
        }
        private void SetViewData() {

            // Set Procurement Type
            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Value = "1", Text = "Consultation" });
            typeList.Add(new SelectListItem() { Value = "2", Text = "Goods" });
            typeList.Add(new SelectListItem() { Value = "3", Text = "Infrastructure" });
            this.TypeList = typeList;

            // Set Actor
            List <SelectListItem> actorList = new List<SelectListItem>();
            actorList.Add(new SelectListItem { Value = "", Text = Constant.PleaseSelect });

            var _list = ProcessData<AccessGroupVM>(new SettingVM(), Constant.ServiceEnpoint.Settings.GetAccessGroup);
            if (_list != null && _list.value.Count() > 0)
            {
                _list.value.ToList().ForEach(x => {
                    actorList.Add(new SelectListItem() { Value = x.id.ToString(), Text = x.group_code });
                });
            }
            this.ActorList = actorList;


            // Set View Data
            ViewData["procureType"] = this.TypeList;
            ViewData["actorList"] = this.ActorList;
        }

        private IEnumerable<WorkflowVM> UpdateDisplayName(IEnumerable<WorkflowVM> _list) {
            if (_list != null && _list.Count() > 0)
            {
                _list.ToList().ForEach(x =>
                {
                    // Set Actor Name
                    var actor = this.ActorList.Where(a => a.Value == x.actor.ToString());
                    if (actor != null && actor.Count() > 0)
                    {
                        x.actor_name = actor.First().Text;
                    }

                    // Set Procurement Name
                    var type = this.TypeList.Where(t => t.Value == x.type.ToString());
                    if (type != null && type.Count() > 0) {
                        x.type_name = type.First().Text;
                    }                    
                });
            }
            else {
                _list = new List<WorkflowVM>();
            }
            return _list;
        }
    }
}