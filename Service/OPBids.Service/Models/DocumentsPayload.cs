using OPBids.Entities.View.DTS;
using OPBids.Entities.View.Shared;
using System.Collections.Generic;

namespace OPBids.Service.Models
{
    public class DocumentsPayload : Base.BaseModel
    {
        public int page_index { get; set; }
        public DocumentSearchVM filter { get; set; }
        public DocumentsVM document {get;set;}
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