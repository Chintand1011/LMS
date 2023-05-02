﻿using System;
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
using OPBids.Entities.View.Suppliers;

namespace OPBids.Web.Logic.Supplier
{
    public abstract class SupplierLogicBase : Controller
    {
        public abstract ActionResult PartialView(SupplierPayloadVM payload);
        public abstract Result<IEnumerable<T>> Maintain<T>(SupplierPayloadVM payload) where T : BaseVM;
        protected Result<IEnumerable<T>> ProcessData<T>(SupplierPayloadVM payload, string service)
        {
            var apiManager = new ApiManager<Result<IEnumerable<T>>>();
            Result<IEnumerable<T>> _list;
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                service, payload);
            _list.value = (_list.value == null ? new List<T>() : _list.value);
            return _list;
        }
    }
}