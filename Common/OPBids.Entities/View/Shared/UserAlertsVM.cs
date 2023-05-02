using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPBids.Entities.View.Shared
{
    public class UserAlertsVM
    {
        public List<UserNotificationVM> Notifications { get; set; }
        public List<UserAnnouncementVM> Announcements { get; set; }
    }
}