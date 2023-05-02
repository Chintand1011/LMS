using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;
using OPBids.Entities.View.Home;
using OPBids.Entities.View.Shared;

namespace OPBids.Entities.View.DTS
{
    public class DocumentsPayloadVM : BaseVM
    {
        public string menu_id { get; set; }
        public DocumentSearchVM filter { get; set; }
        public DocumentsVM document { get; set; }
        public List<UserAnnouncementVM> userAnnouncements { get; set; }
        public UserAnnouncementVM userAnnouncement { get; set; }
        public List<UserNotificationVM> userNotifications { get; set; }
        public UserNotificationVM userNotification { get; set; }
        public List<RequestBarcodeVM> requestBarcodes { get; set; }
        public RequestBarcodeVM requestBarcode { get; set; }
        public List<DocumentAttachmentVM> documentAttachments { get; set; }
        public DocumentAttachmentVM documentAttachment { get; set; }
        public List<DocumentRoutesVM> documentRoutes { get; set; }
        public DocumentRoutesVM documentRoute { get; set; }
        public List<DocumentLogsVM> documentLogs { get; set; }
        public DocumentLogsVM documentLog { get; set; }
    }
}
