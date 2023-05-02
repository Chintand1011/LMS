using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Entities.View.Shared;
using OPBids.Web.Logic;
using OPBids.Web.Logic.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static OPBids.Common.Constant;

namespace OPBids.Web.Helper
{
    public static class UserInfoStore
    {
        public static List<OPBids.Entities.View.Shared.Menu> Menus(Microsoft.Owin.IOwinContext cntxt)
        {
            var menuItems = new AccessGroupTypeLogic(cntxt);
            var deptcode = AuthHelper.GetClaims(cntxt, Constant.Auth.Claims.DeptCode).ToSafeString();
            var rslts = menuItems.GetAccessGroupMenu<AccessGroupTypeVM>(new Entities.View.Setting.SettingVM()
            {
                id = AuthHelper.GetClaims(cntxt, Constant.Auth.Claims.GroupId).ToSafeInt()
            }).value.Where(a => a.sys_id == 0 || a.sys_id == UserInfoStore.GetCurrentProduct(cntxt)).OrderBy(a => a.seq_no).ToList();
            var menuLst = new List<OPBids.Entities.View.Shared.Menu>();
            menuLst.AddRange(rslts.Where(a => (a.status.ToUpper() == RecordStatus.Header || a.access_type_id == a.parent_id) &&
            (UserInfoStore.IsMobileBrowser(cntxt.Request) == true && a.disp_menu_to_mobile == true ||
            UserInfoStore.IsMobileBrowser(cntxt.Request) == false)).ToList().Select(a =>
            new Entities.View.Shared.Menu()
            {
                ControllerAction = a.controller,
                CssClass = a.css_class,
                Icon = a.icon,
                ID = a.access_type_id.ToSafeString(),
                Code = a.code,
                Name = a.name,
                SubMenus = null,
                AddOrEdit = a.add_edit_data ?? false,
                Delete = a.delete_data ?? false,
                RecordSection = a.record_section ?? false,
            }));
            menuLst.ForEach(a =>
           {
               a.SubMenus = rslts.Where(b => (b.access_type_id != b.parent_id && b.parent_id == a.ID.ToSafeInt() &&
               b.status.ToUpper() == RecordStatus.Active) && (UserInfoStore.IsMobileBrowser(cntxt.Request) == true &&
               b.disp_menu_to_mobile == true || UserInfoStore.IsMobileBrowser(cntxt.Request) == false)).Select(c =>
               new SubMenu()
               {
                   ID = c.access_type_id.ToSafeString(),
                   Code = c.code,
                   ControllerAction = c.controller,
                   CssClass = c.css_class,
                   Icon = c.icon,
                   Name = c.name,
                   AddOrEdit = c.add_edit_data ?? false,
                   Delete = c.delete_data ?? false,
                   RecordSection = c.record_section ?? false,
               }).ToList();
           });
            return menuLst;
        }
        public static int GetCurrentProduct(Microsoft.Owin.IOwinContext cntxt)
        {
            return AuthHelper.GetClaims(cntxt, Constant.Auth.Claims.CurrentProduct).ToSafeInt();
        }
        public static int GetUserId(Microsoft.Owin.IOwinContext cntxt)
        {
            return AuthHelper.GetClaims(cntxt, Constant.Auth.Claims.UserId).ToSafeInt();
        }
        public static bool GetPfmsAccess(Microsoft.Owin.IOwinContext cntxt)
        {
            return AuthHelper.GetClaims(cntxt, Constant.Auth.Claims.PfmsAccess).ToSafeBool();
        }
        public static bool GetDtsAccess(Microsoft.Owin.IOwinContext cntxt)
        {
            return AuthHelper.GetClaims(cntxt, Constant.Auth.Claims.DtsAccess).ToSafeBool();
        }
        public static IOrderedEnumerable<DocumentCategoryVM> GetDocumentCategory(Microsoft.Owin.IOwinContext cntxt)
        {
            return new ControllerLogic.Setting(cntxt).Logic(Constant.Menu.DocumentCategory).SearchData<DocumentCategoryVM>(new SettingVM() { page_index = -1 }).OrderBy(a => a.document_category_name);
        }
        public static IOrderedEnumerable<DocumentTypeVM> GetDocumentType(Microsoft.Owin.IOwinContext cntxt)
        {
            return new ControllerLogic.Setting(cntxt).Logic(Constant.Menu.DocumentType).SearchData<DocumentTypeVM>(new SettingVM() { page_index = -1 }).OrderBy(a => a.document_type_code);
        }
        public static IOrderedEnumerable<SenderRecipientUserVM> GetSenderRecipient(Microsoft.Owin.IOwinContext cntxt)
        {
            return new ControllerLogic.Setting(cntxt).Logic(Constant.Menu.SenderRecipient).SearchData<SenderRecipientUserVM>(new SettingVM() {page_index = -1 }).OrderBy(a => a.first_name);
        }
        public static IOrderedEnumerable<DepartmentsVM> GetDepartment(Microsoft.Owin.IOwinContext cntxt)
        {
            return new ControllerLogic.Setting(cntxt).Logic(Constant.Menu.Departments).SearchData<DepartmentsVM>(new SettingVM() { page_index = -1 }).OrderBy(a => a.dept_code);
        }
        public static IEnumerable<DeliveryVM> GetDeliveryType(Microsoft.Owin.IOwinContext cntxt)
        {
            return new ControllerLogic.Setting(cntxt).Logic(Constant.Menu.Delivery).SearchData<DeliveryVM>(new SettingVM() { page_index = -1 });
        }
        public static IOrderedEnumerable<DocumentSecurityLevelVM> GetDocumentSecurity(Microsoft.Owin.IOwinContext cntxt)
        {
            return new ControllerLogic.Setting(cntxt).Logic(Constant.Menu.DocumentSecurityLevel).SearchData<DocumentSecurityLevelVM>(new SettingVM() { page_index = -1 }).OrderBy(a => a.code);
        }
        public static bool IsMobileBrowser(Microsoft.Owin.IOwinRequest request)
        {
            var b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var userAgent = request.Headers["User-Agent"];
            if ((b.IsMatch(userAgent) || v.IsMatch(userAgent.Substring(0, 4))))
            {
                return true;
            }

            return false;
        }
    }
}