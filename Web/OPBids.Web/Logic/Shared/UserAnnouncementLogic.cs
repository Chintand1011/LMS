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
using OPBids.Web.Logic.DTS;

namespace OPBids.Web.Logic.Shared
{
    public class UserAnnouncementLogic : DTSLogicBase
    {
        public override ActionResult PartialView(DocumentsPayloadVM param)
        {
            return null;
        }
        public override Result<IEnumerable<T>> Maintain<T>(DocumentsPayloadVM param)
        {
            return ProcessData<T>(param, Constant.ServiceEnpoint.DTS.MaintainUserAnnouncement);
        }
    }
}