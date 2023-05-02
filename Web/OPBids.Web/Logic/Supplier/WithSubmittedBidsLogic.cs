using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Web.Helper;
using OPBids.Entities.View.Suppliers;
using OPBids.Entities.View.Supplier;

namespace OPBids.Web.Logic.Supplier
{
    public class WithSubmittedBidsLogic : SupplierLogicBase
    {
        public override ActionResult PartialView(SupplierPayloadVM param)
        {
            return PartialView(Constant.Supplier.PartialView.WithSubmittedBids, new List<SuppliersVM>());
        }
        public override Result<IEnumerable<T>> Maintain<T>(SupplierPayloadVM param)
        {
            return ProcessData<T>(param, Constant.ServiceEnpoint.SupplierRequest.MaintainWithSubmittedBids);
        }
    }
}