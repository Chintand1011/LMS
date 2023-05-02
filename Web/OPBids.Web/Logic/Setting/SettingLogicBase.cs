using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OPBids.Common;
using System.Web.Mvc;
using OPBids.Entities.View.Setting;
using OPBids.Entities.Base;
using OPBids.Entities.Common;
using OPBids.Web.Helper;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Web.Http;
using System.Reflection;

namespace OPBids.Web.Logic.Setting
{
    public abstract class SettingLogicBase : Controller
    {
        public abstract string HeaderView(string subMenuId);
        public abstract ActionResult ResultView(SettingVM setting);
        public abstract IEnumerable<T> SearchData<T>(SettingVM setting) where T : BaseVM;
        public abstract IEnumerable<T> SearchSub<T>(SettingVM setting) where T : BaseVM;
        public abstract ActionResult Search(SettingVM setting);
        public abstract string HTMLTable();
        public abstract GridView DownloadExcel();
        public abstract Tuple<string, string> DownloadCSV();
        public abstract ActionResult Save(SettingVM setting);
        public abstract ActionResult StatusUpdate(SettingVM setting);
        protected Result<IEnumerable<T>> ProcessData<T>(SettingVM setting, string service)
        {
            var apiManager = new ApiManager<Result<IEnumerable<T>>>();
            Result<IEnumerable<T>> _list;
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                service, setting);
            _list.value = _list.value == null ? new List<T>() : _list.value;
            return _list;
        }

        protected Tuple<string, string> DownloadCSV<T>(string fileName, string service)
        {
            var list = ProcessData<T>(new SettingVM(), service).value.ToList();
            if (list == null || list.Count == 0)
            {
                return Tuple.Create<string, string>(string.Concat(fileName, ".csv"), "");
            }
            var t = list[0].GetType();
            var sb = new StringBuilder();
            var newLine = Environment.NewLine;
            var o = Activator.CreateInstance(t);
            var props = o.GetType().GetProperties();
            foreach (var pi in props)
            {
                sb.Append(pi.Name.ToUpper() + ",");
            }
            sb.Append(newLine);

            //this acts as datarow
            foreach (T item in list)
            {
                //this acts as datacolumn
                foreach (PropertyInfo pi in props)
                {
                    //this is the row+col intersection (the value)
                    string whatToWrite =
                        Convert.ToString(item.GetType().GetProperty(pi.Name).GetValue(item, null)).ToSafeString().Replace(',', ' ') + ',';

                    sb.Append(whatToWrite);
                }
                sb.Append(newLine);
            }
            return Tuple.Create<string, string>(string.Concat(fileName, ".csv"), sb.ToString());
        }
        protected string HTMLTable<T>(string service)
        {
            var list = ProcessData<T>(new SettingVM(), service).value.ToList();
            if (list == null || list.Count == 0)
            {
                return "";
            }
            var t = list[0].GetType();
            var sb = new StringBuilder();
            var newLine = Environment.NewLine;
            var o = Activator.CreateInstance(t);
            var props = o.GetType().GetProperties();
            sb.Append("<table><thead><tr>");
            foreach (var pi in props)
            {
                sb.Append(string.Concat("<th style='font-weight:bold;color:#ffffff;background-color:#000000;border:1px solid #0000000;'>", pi.Name.ToUpper() + "</th>"));
            }
            sb.Append("</thead><tbody>");

            //this acts as datarow
            foreach (T item in list)
            {
                sb.Append("<tr>");
                //this acts as datacolumn
                foreach (PropertyInfo pi in props)
                {
                    sb.Append(string.Concat("<td style='border:1px solid #0000000;'>", item.GetType().GetProperty(pi.Name).GetValue(item, null).ToSafeString(), "</td>"));
                }
                sb.Append("</tr>");
            }
            sb.Append("</tbody></table>");
            return sb.ToString();
        }
        protected GridView DownloadExcel<T>(string fileName, string service)
        {
            var grid = new GridView();
            grid.ID = string.Concat(fileName, ".xlsx");
            grid.DataSource = ProcessData<T>(new SettingVM(), service).value.ToList();
            grid.DataBind();
            return grid;
        }
    }
}