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
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.Owin;
using OPBids.Entities.View.DTS;
using OPBids.Entities.Base;

namespace OPBids.Web.Logic.DTS
{
    public abstract class DTSCommonLogicBase : Controller
    {
        public abstract IEnumerable<T> Maintain<T>(DocumentsPayloadVM param);
        protected IEnumerable<T> ProcessData<T>(DocumentsPayloadVM param, string service)
        {
            var apiManager = new ApiManager<Result<IEnumerable<T>>>();
            Result<IEnumerable<T>> _list;
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                service, param);
            return _list.value == null ? new List<T>() : _list.value;
        }
    }
}