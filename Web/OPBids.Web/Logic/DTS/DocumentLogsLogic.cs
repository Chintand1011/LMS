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
using OPBids.Web.Logic.DTS;
using Microsoft.Owin;

namespace OPBids.Web.Logic.Setting
{
    public class DocumentLogsLogic : DTSCommonLogicBase
    {
        IOwinContext _context;
        public DocumentLogsLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override IEnumerable<T> Maintain<T>(DocumentsPayloadVM param)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            param.documentLog.updated_by = user_id;
            param.documentLog.receipient_id = user_id;
            if (param.documentLog.id == 0)
            {
                param.documentLog.created_by = user_id;
            }
            if (param.documentLogs != null)
            {
                param.documentLogs.ForEach(a =>
                {
                    a.receipient_id = user_id;
                    a.updated_by = user_id;
                    if (a.id == 0)
                    {
                        a.created_by = user_id;
                    }
                });
            }
            Result<IEnumerable<T>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<T>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.DTS.MaintainDocumentLogs, param);
            return _list.value == null ? new List<T>() : _list.value;
        }
    }

}