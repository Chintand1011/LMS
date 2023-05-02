using OPBids.Entities.Base;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Suppliers
{
    public class SupplierPayloadVM:BaseVM
    {
        public string menu_id { get; set; }
        public string process { get; set; }
        public SupplierSearchVM filter { get; set; }
        public SuppliersVM supplier { get; set; }
        public List<ProjectRequestAttachmentVM> documentAttachments { get; set; }
        public ProjectRequestAttachmentVM documentAttachment { get; set; }
    }
}
