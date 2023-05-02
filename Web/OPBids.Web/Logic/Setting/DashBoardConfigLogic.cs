using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Web.Mvc;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Common;
using OPBids.Web.Helper;
using OPBids.Entities.Base;
using System.Web.UI.WebControls;
using Microsoft.Owin;

namespace OPBids.Web.Logic.Setting
{
    public class DashBoardConfigLogic : SettingLogicBase
    {
        public override string HeaderView(string subMenuId)
        {
            return null;//Constant.Setting.HeaderView.;
        }

        public override ActionResult ResultView(SettingVM setting)
        {
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
            return null;//DownloadExcel<AccessGroupTypeVM>("AccessGroupType", Constant.ServiceEnpoint.Settings.GetAccessGroupType);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return null;
        }
        public override string HTMLTable()
        {
            return null;
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetDashBoardConfig);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.DashboardConfig,
                SearchData<DashboardConfigVM>(setting));
        }


        public override ActionResult Save(SettingVM setting)
        {
            return null;
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            return null;
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
    }
}