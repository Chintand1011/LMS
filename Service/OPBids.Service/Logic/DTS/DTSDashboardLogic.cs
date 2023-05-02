using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.DTS;
using OPBids.Service.Data;
using OPBids.Service.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;

namespace OPBids.Service.Logic.DTS
{
    public class DTSDashboardLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<DocumentsVM>> MaintainData(DocumentsPayload param)
        {
            Result<IEnumerable<DocumentsVM>> rslts;
            switch (param.document.process.ToSafeString().ToUpper())
            {
                case Constant.TransactionType.Search:
                case Constant.TransactionType.Get:
                case Constant.TransactionType.Track:
                    rslts = GetData(param);
                    break;
                case Constant.TransactionType.Create:
                    rslts = Create(param);
                    break;
                case Constant.TransactionType.Update:
                    rslts = Update(param);
                    break;
                case Constant.TransactionType.StatusUpdate:
                    rslts = UpdateStatus(param);
                    break;
                case Constant.TransactionType.Promote:
                    rslts = Promote(param);
                    break;
                case Constant.TransactionType.Top5AgingDocuments:
                    rslts = Top5AgingDocuments(param);
                    break;
                default:
                    rslts = GetData(param);
                    break;
            }
            return rslts;
        }

        public Result<IEnumerable<DocumentsVM>> GetData(DocumentsPayload payload)
        {
            var _result = new Result<IEnumerable<DocumentsVM>>();

            switch (payload.document.process)
            {
                case Constant.TransactionType.Track:
                    _result.value = (from d in db.OnHandDocuments
                                     join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                     equals new { t1 = s.id, t2 = s.status } into s1
                                     from s2 in s1.DefaultIfEmpty()
                                     join s3 in db.SenderRecipientUser on new { t1 = s2.user_id, t2 = s2.is_system_user }
                                     equals new { t1 = s3.id, t2 = false } into s4
                                     from s5 in s4.DefaultIfEmpty()
                                     join a1 in db.AccessUser on new { t1 = s2.user_id, t2 = Constant.RecordStatus.Active, t3 = s2.is_system_user }
                                     equals new { t1 = a1.id, t2 = a1.status, t3 = true } into a2
                                     from a3 in a2.DefaultIfEmpty()
                                     join r in db.SenderRecipient on new { t1 = d.receipient_id, t2 = Constant.RecordStatus.Active }
                                     equals new { t1 = r.id, t2 = r.status } into r1
                                     from r2 in r1.DefaultIfEmpty()
                                     join r3 in db.SenderRecipientUser on new { t1 = r2.user_id, t2 = r2.is_system_user }
                                     equals new { t1 = r3.id, t2 = false } into r4
                                     from r5 in r4.DefaultIfEmpty()
                                     join ra1 in db.AccessUser on new { t1 = r2.user_id, t2 = Constant.RecordStatus.Active, t3 = r2.is_system_user }
                                     equals new { t1 = ra1.id, t2 = ra1.status, t3 = true } into ra2
                                     from ra3 in ra2.DefaultIfEmpty()
                                     join cat in db.DocumentCategory on new { t1 = d.category_id, t2 = Constant.RecordStatus.Active } equals new { t1 = cat.id, t2 = cat.status } into c1
                                     from c2 in c1.DefaultIfEmpty()
                                     join dt in db.Delivery on new { t1 = d.delivery_type_id, t2 = Constant.RecordStatus.Active } equals new { t1 = dt.id, t2 = dt.status } into dt1
                                     from dt2 in dt1.DefaultIfEmpty()
                                     join ds in db.DocumentSecurityLevel on d.document_security_level_id equals ds.id into ds1
                                     from ds2 in ds1.DefaultIfEmpty()
                                     join dtype in db.DocumentType on d.document_type_id equals dtype.id into dtype1
                                     from dtype2 in dtype1.DefaultIfEmpty()
                                     where
                                     (d.id == payload.filter.id || d.document_code == payload.filter.barcode_no ||
                                     db.DocumentAttachment.Any(c => c.barcode_no == payload.filter.barcode_no && c.batch_id == d.id)) &&
                                     (d.status == Constant.RecordStatus.Active &&
                                     (((ds2.id == 1 || ds2.id == 2 || ds2.id == 3 || ds2.id == 4 || ds2.id == 5 || ds2.id == 6) && (
                                     (s2.user_id == payload.filter.created_by && db.DocumentRoutes.Any(b => b.batch_id == d.id &&
                                     b.current_receiver == false)) || db.DocumentRoutes.Any(a => a.batch_id == d.id &&
                                     a.receipient_id == payload.filter.created_by && a.current_receiver == false)
                                     )) || ((ds2.id == 1 || ds2.id == 2 || ds2.id == 4) && db.DocumentRoutes.Any(a => a.batch_id == d.id &&
                                     a.department_id == payload.filter.department_id && a.current_receiver == false)) ||
                                     ((ds2.id == 1 || ds2.id == 2 || ds2.id == 3) && db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                     b.id == payload.filter.created_by) && s2.user_id == a.id) && db.DocumentRoutes.Any(b => b.batch_id == d.id &&
                                     b.current_receiver == false && b.is_sender == true)) ||
                                     d.status == Constant.RecordStatus.MarkForArchive && payload.filter.record_section == true)
                                     )
                                     select new DocumentsVM()
                                     {
                                         doc_stage = "O",
                                         sender_department_id = a3.dept_id,
                                         id = d.id,
                                         status = d.status,
                                         delivery_type_id = dt2 == null ? 0 : dt2.id,
                                         category_code = c2 == null ? "" : c2.document_category_code,
                                         category_id = c2 == null ? 0 : c2.id,
                                         category_name = c2.document_category_name,
                                         created_by = d.created_by == 0 ? s2.user_id : d.created_by,
                                         created_date = d.created_date.ToString(),
                                         delivery_type_name = dt2.delivery_code,
                                         document_code = d.document_code,
                                         document_security_level = ds2.description,
                                         document_security_level_id = d.document_security_level_id,
                                         document_type_id = d.document_type_id,
                                         document_type_name = dtype2.document_type_code,
                                         etd_to_recipient = d.etd_to_recipient.ToString(),
                                         receipient_id = d.receipient_id,
                                         receipient_name = (r2.is_system_user ? string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)),
                                         sender_id = d.sender_id,
                                         sender_name = (s2.is_system_user ? string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)),
                                         updated_by = d.updated_by,
                                         updated_date = d.updated_date.ToString(),
                                         track_status = d.status == Constant.RecordStatus.MarkForArchive ? "D" : "R"
                                     }).ToList();

                    if (_result.value.Count() <= 0)
                    {
                        _result.value = (from d in db.ReceivedDocuments
                                         join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = s.id, t2 = s.status } into s1
                                         from s2 in s1.DefaultIfEmpty()
                                         join s3 in db.SenderRecipientUser on new { t1 = s2.user_id, t2 = s2.is_system_user }
                                         equals new { t1 = s3.id, t2 = false } into s4
                                         from s5 in s4.DefaultIfEmpty()
                                         join a1 in db.AccessUser on new { t1 = s2.user_id, t2 = Constant.RecordStatus.Active, t3 = s2.is_system_user }
                                         equals new { t1 = a1.id, t2 = a1.status, t3 = true } into a2
                                         from a3 in a2.DefaultIfEmpty()
                                         join r in db.SenderRecipient on new { t1 = d.receipient_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = r.id, t2 = r.status } into r1
                                         from r2 in r1.DefaultIfEmpty()
                                         join r3 in db.SenderRecipientUser on new { t1 = r2.user_id, t2 = r2.is_system_user }
                                         equals new { t1 = r3.id, t2 = false } into r4
                                         from r5 in s4.DefaultIfEmpty()
                                         join ra1 in db.AccessUser on new { t1 = r2.user_id, t2 = Constant.RecordStatus.Active, t3 = r2.is_system_user }
                                         equals new { t1 = ra1.id, t2 = ra1.status, t3 = true } into ra2
                                         from ra3 in ra2.DefaultIfEmpty()
                                         join cat in db.DocumentCategory on new { t1 = d.category_id, t2 = Constant.RecordStatus.Active } equals new { t1 = cat.id, t2 = cat.status } into c1
                                         from c2 in c1.DefaultIfEmpty()
                                         join dt in db.Delivery on new { t1 = d.delivery_type_id, t2 = Constant.RecordStatus.Active } equals new { t1 = dt.id, t2 = dt.status } into dt1
                                         from dt2 in dt1.DefaultIfEmpty()
                                         join ds in db.DocumentSecurityLevel on d.document_security_level_id equals ds.id into ds1
                                         from ds2 in ds1.DefaultIfEmpty()
                                         join dtype in db.DocumentType on d.document_type_id equals dtype.id into dtype1
                                         from dtype2 in dtype1.DefaultIfEmpty()
                                         where
                                         (d.batch_id == payload.filter.id || d.document_code == payload.filter.barcode_no ||
                                         db.DocumentAttachment.Any(c => c.barcode_no == payload.filter.barcode_no && c.batch_id == d.batch_id)) &&
                                         (d.status == Constant.RecordStatus.Active &&
                                         (
                                         ((ds2.id == 1 || ds2.id == 2 || ds2.id == 3 || ds2.id == 4 || ds2.id == 5 || ds2.id == 6) && (
                                         (s2.user_id == payload.filter.created_by && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                         b.current_receiver == false && b.is_sender == true)) || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                         b.current_receiver == false && b.is_sender == false && b.receipient_id == payload.filter.created_by))) ||

                                         ((ds2.id == 1 || ds2.id == 2 || ds2.id == 3) &&
                                         (db.AccessUser.Any(a => a.id == s2.user_id && db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                         b.id == payload.filter.created_by)) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == payload.filter.department_id &&
                                         b.current_receiver == false && b.is_sender == true))) ||

                                         ((ds2.id == 1 || ds2.id == 2 || ds2.id == 4) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.current_receiver == false && b.is_sender == false &&
                                         db.AccessUser.Any(a => a.id == payload.filter.created_by && a.dept_id == b.department_id))) ||

                                         d.status == Constant.RecordStatus.MarkForArchive && payload.filter.record_section == true))
                                         select new DocumentsVM()
                                         {
                                             doc_stage = "R",
                                             sender_department_id = a3.dept_id,
                                             id = d.batch_id,
                                             status = d.status,
                                             delivery_type_id = dt2 == null ? 0 : dt2.id,
                                             category_code = c2 == null ? "" : c2.document_category_code,
                                             category_id = c2 == null ? 0 : c2.id,
                                             category_name = c2.document_category_name,
                                             created_by = d.created_by == 0 ? s2.user_id : d.created_by,
                                             created_date = d.created_date.ToString(),
                                             delivery_type_name = dt2.delivery_code,
                                             document_code = d.document_code,
                                             document_security_level = ds2.description,
                                             document_security_level_id = d.document_security_level_id,
                                             document_type_id = d.document_type_id,
                                             document_type_name = dtype2.document_type_code,
                                             etd_to_recipient = d.etd_to_recipient.ToString(),
                                             receipient_id = d.receipient_id,
                                             receipient_name = (r2.is_system_user ? string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)),
                                             sender_id = d.sender_id,
                                             sender_name = (s2.is_system_user ? string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)),
                                             updated_by = d.updated_by,
                                             updated_date = d.updated_date.ToString(),
                                             track_status = d.status == Constant.RecordStatus.MarkForArchive ? "D" : "R"
                                         }).ToList();
                    }
                    if (_result.value.Count() <= 0)
                    {
                        _result.value = (from d in db.OnHandDocuments
                                         join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = s.id, t2 = s.status } into s1
                                         from s2 in s1.DefaultIfEmpty()
                                         join s3 in db.SenderRecipientUser on new { t1 = s2.user_id, t2 = s2.is_system_user }
                                         equals new { t1 = s3.id, t2 = false } into s4
                                         from s5 in s4.DefaultIfEmpty()
                                         join a1 in db.AccessUser on new { t1 = s2.user_id, t2 = Constant.RecordStatus.Active, t3 = s2.is_system_user }
                                         equals new { t1 = a1.id, t2 = a1.status, t3 = true } into a2
                                         from a3 in a2.DefaultIfEmpty()
                                         join r in db.SenderRecipient on new { t1 = d.receipient_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = r.id, t2 = r.status } into r1
                                         from r2 in r1.DefaultIfEmpty()
                                         join r3 in db.SenderRecipientUser on new { t1 = r2.user_id, t2 = r2.is_system_user }
                                         equals new { t1 = r3.id, t2 = false } into r4
                                         from r5 in r4.DefaultIfEmpty()
                                         join ra1 in db.AccessUser on new { t1 = r2.user_id, t2 = Constant.RecordStatus.Active, t3 = r2.is_system_user }
                                         equals new { t1 = ra1.id, t2 = ra1.status, t3 = true } into ra2
                                         from ra3 in ra2.DefaultIfEmpty()
                                         join cat in db.DocumentCategory on new { t1 = d.category_id, t2 = Constant.RecordStatus.Active } equals new { t1 = cat.id, t2 = cat.status } into c1
                                         from c2 in c1.DefaultIfEmpty()
                                         join dt in db.Delivery on new { t1 = d.delivery_type_id, t2 = Constant.RecordStatus.Active } equals new { t1 = dt.id, t2 = dt.status } into dt1
                                         from dt2 in dt1.DefaultIfEmpty()
                                         join ds in db.DocumentSecurityLevel on d.document_security_level_id equals ds.id into ds1
                                         from ds2 in ds1.DefaultIfEmpty()
                                         join dtype in db.DocumentType on d.document_type_id equals dtype.id into dtype1
                                         from dtype2 in dtype1.DefaultIfEmpty()
                                         where
                                         (d.id == payload.filter.id || d.document_code == payload.filter.barcode_no ||
                                         db.DocumentAttachment.Any(c => c.barcode_no == payload.filter.barcode_no && c.batch_id == d.id)) &&
                                         (d.status == Constant.RecordStatus.Active &&
                                         (ds2.id == 1 || ((ds2.id == 1 || ds2.id == 2 || ds2.id == 3 || ds2.id == 4 || ds2.id == 5 || ds2.id == 6) &&
                                         (s2.user_id == payload.filter.created_by)) || ((ds2.id == 1 || ds2.id == 2 || ds2.id == 3) &&
                                         (db.AccessUser.Any(a => a.id == s2.user_id && db.AccessUser.Any(b => b.dept_id == a.dept_id && b.id == payload.filter.created_by))))) ||
                                         d.status == Constant.RecordStatus.MarkForArchive && payload.filter.record_section == true)
                                         select new DocumentsVM()
                                         {
                                             doc_stage = "O",
                                             sender_department_id = a3.dept_id,
                                             id = d.id,
                                             status = d.status,
                                             delivery_type_id = dt2 == null ? 0 : dt2.id,
                                             category_code = c2 == null ? "" : c2.document_category_code,
                                             category_id = c2 == null ? 0 : c2.id,
                                             category_name = c2.document_category_name,
                                             created_by = d.created_by == 0 ? s2.user_id : d.created_by,
                                             created_date = d.created_date.ToString(),
                                             delivery_type_name = dt2.delivery_code,
                                             document_code = d.document_code,
                                             document_security_level = ds2.description,
                                             document_security_level_id = d.document_security_level_id,
                                             document_type_id = d.document_type_id,
                                             document_type_name = dtype2.document_type_code,
                                             etd_to_recipient = d.etd_to_recipient.ToString(),
                                             receipient_id = d.receipient_id,
                                             receipient_name = (r2.is_system_user ? string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)),
                                             sender_id = d.sender_id,
                                             sender_name = (s2.is_system_user ? string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)),
                                             updated_by = d.updated_by,
                                             updated_date = d.updated_date.ToString(),
                                             track_status = d.status == Constant.RecordStatus.MarkForArchive ? "D" : "V"
                                         }).ToList();
                    }
                    if (_result.value.Count() <= 0)
                    {
                        _result.value = (from d in db.ReceivedDocuments
                                         join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = s.id, t2 = s.status } into s1
                                         from s2 in s1.DefaultIfEmpty()
                                         join s3 in db.SenderRecipientUser on new { t1 = s2.user_id, t2 = s2.is_system_user }
                                         equals new { t1 = s3.id, t2 = false } into s4
                                         from s5 in s4.DefaultIfEmpty()
                                         join a1 in db.AccessUser on new { t1 = s2.user_id, t2 = Constant.RecordStatus.Active, t3 = s2.is_system_user }
                                         equals new { t1 = a1.id, t2 = a1.status, t3 = true } into a2
                                         from a3 in a2.DefaultIfEmpty()
                                         join r in db.SenderRecipient on new { t1 = d.receipient_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = r.id, t2 = r.status } into r1
                                         from r2 in r1.DefaultIfEmpty()
                                         join r3 in db.SenderRecipientUser on new { t1 = r2.user_id, t2 = r2.is_system_user }
                                         equals new { t1 = r3.id, t2 = false } into r4
                                         from r5 in s4.DefaultIfEmpty()
                                         join ra1 in db.AccessUser on new { t1 = r2.user_id, t2 = Constant.RecordStatus.Active, t3 = r2.is_system_user }
                                         equals new { t1 = ra1.id, t2 = ra1.status, t3 = true } into ra2
                                         from ra3 in ra2.DefaultIfEmpty()
                                         join cat in db.DocumentCategory on new { t1 = d.category_id, t2 = Constant.RecordStatus.Active } equals new { t1 = cat.id, t2 = cat.status } into c1
                                         from c2 in c1.DefaultIfEmpty()
                                         join dt in db.Delivery on new { t1 = d.delivery_type_id, t2 = Constant.RecordStatus.Active } equals new { t1 = dt.id, t2 = dt.status } into dt1
                                         from dt2 in dt1.DefaultIfEmpty()
                                         join ds in db.DocumentSecurityLevel on d.document_security_level_id equals ds.id into ds1
                                         from ds2 in ds1.DefaultIfEmpty()
                                         join dtype in db.DocumentType on d.document_type_id equals dtype.id into dtype1
                                         from dtype2 in dtype1.DefaultIfEmpty()
                                         where
                                         (d.batch_id == payload.filter.id || d.document_code == payload.filter.barcode_no ||
                                         db.DocumentAttachment.Any(c => c.barcode_no == payload.filter.barcode_no && c.batch_id == d.batch_id)) &&
                                         (d.status == Constant.RecordStatus.Active &&
                                         (ds2.id == 1 || ((ds2.id == 1 || ds2.id == 2 || ds2.id == 3 || ds2.id == 4 || ds2.id == 5 || ds2.id == 6) && (
                                         (s2.user_id == payload.filter.created_by && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                         b.current_receiver == true && b.is_sender == true && b.department_id == payload.filter.department_id)) ||
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                         b.current_receiver == true && b.is_sender == false && b.receipient_id == payload.filter.created_by))) ||

                                         ((ds2.id == 1 || ds2.id == 2 || ds2.id == 3) &&
                                         (db.AccessUser.Any(a => a.id == s2.user_id && db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                         b.id == payload.filter.created_by)) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == payload.filter.department_id &&
                                         b.current_receiver == true && b.is_sender == true))) ||

                                         (((ds2.id == 1 || ds2.id == 2 || ds2.id == 4) && db.DocumentRoutes.Any(a => a.batch_id == d.batch_id &&
                                         a.department_id == payload.filter.department_id && a.current_receiver == true))) ||
                                         (d.status == Constant.RecordStatus.MarkForArchive && payload.filter.record_section == true)
                                         ))
                                         select new DocumentsVM()
                                         {
                                             doc_stage = "R",
                                             sender_department_id = a3.dept_id,
                                             id = d.batch_id,
                                             status = d.status,
                                             delivery_type_id = dt2 == null ? 0 : dt2.id,
                                             category_code = c2 == null ? "" : c2.document_category_code,
                                             category_id = c2 == null ? 0 : c2.id,
                                             category_name = c2.document_category_name,
                                             created_by = d.created_by == 0 ? s2.user_id : d.created_by,
                                             created_date = d.created_date.ToString(),
                                             delivery_type_name = dt2.delivery_code,
                                             document_code = d.document_code,
                                             document_security_level = ds2.description,
                                             document_security_level_id = d.document_security_level_id,
                                             document_type_id = d.document_type_id,
                                             document_type_name = dtype2.document_type_code,
                                             etd_to_recipient = d.etd_to_recipient.ToString(),
                                             receipient_id = d.receipient_id,
                                             receipient_name = (r2.is_system_user ? string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)),
                                             sender_id = d.sender_id,
                                             sender_name = (s2.is_system_user ? string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)),
                                             updated_by = d.updated_by,
                                             updated_date = d.updated_date.ToString(),
                                             track_status = d.status == Constant.RecordStatus.MarkForArchive ? "D" : "V"
                                         }).ToList();
                    }
                    if (_result.value.Count() <= 0)
                    {
                        _result.value = (from d in db.FinalizedDocuments
                                         join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = s.id, t2 = s.status } into s1
                                         from s2 in s1.DefaultIfEmpty()
                                         join s3 in db.SenderRecipientUser on new { t1 = s2.user_id, t2 = s2.is_system_user }
                                         equals new { t1 = s3.id, t2 = false } into s4
                                         from s5 in s4.DefaultIfEmpty()
                                         join a1 in db.AccessUser on new { t1 = s2.user_id, t2 = Constant.RecordStatus.Active, t3 = s2.is_system_user }
                                         equals new { t1 = a1.id, t2 = a1.status, t3 = true } into a2
                                         from a3 in a2.DefaultIfEmpty()
                                         join r in db.SenderRecipient on new { t1 = d.receipient_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = r.id, t2 = r.status } into r1
                                         from r2 in r1.DefaultIfEmpty()
                                         join r3 in db.SenderRecipientUser on new { t1 = r2.user_id, t2 = r2.is_system_user }
                                         equals new { t1 = r3.id, t2 = false } into r4
                                         from r5 in s4.DefaultIfEmpty()
                                         join ra1 in db.AccessUser on new { t1 = r2.user_id, t2 = Constant.RecordStatus.Active, t3 = r2.is_system_user }
                                         equals new { t1 = ra1.id, t2 = ra1.status, t3 = true } into ra2
                                         from ra3 in ra2.DefaultIfEmpty()
                                         join cat in db.DocumentCategory on new { t1 = d.category_id, t2 = Constant.RecordStatus.Active } equals new { t1 = cat.id, t2 = cat.status } into c1
                                         from c2 in c1.DefaultIfEmpty()
                                         join dt in db.Delivery on new { t1 = d.delivery_type_id, t2 = Constant.RecordStatus.Active } equals new { t1 = dt.id, t2 = dt.status } into dt1
                                         from dt2 in dt1.DefaultIfEmpty()
                                         join ds in db.DocumentSecurityLevel on d.document_security_level_id equals ds.id into ds1
                                         from ds2 in ds1.DefaultIfEmpty()
                                         join dtype in db.DocumentType on d.document_type_id equals dtype.id into dtype1
                                         from dtype2 in dtype1.DefaultIfEmpty()
                                         where
                                         (d.batch_id == payload.filter.id || d.document_code == payload.filter.barcode_no ||
                                         db.DocumentAttachment.Any(c => c.barcode_no == payload.filter.barcode_no && c.batch_id == d.batch_id)) &&
                                         ((d.status == Constant.RecordStatus.Active && (s2.user_id == payload.filter.created_by && db.DocumentRoutes.Any(a => a.sequence != null && a.batch_id == d.batch_id))) ||
                                         ds2.id == 1 ||
                                         // document security level 1
                                         (ds2.id == 1 && db.DocumentRoutes.Any(a => a.batch_id == d.batch_id && a.receipient_id == payload.filter.created_by && a.sequence == null) == false) ||
                                         // document security level 2
                                         (ds2.id == 2 && ((db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id && b.id == payload.filter.created_by) && s2.user_id == a.id)) ||
                                         (db.AccessUser.Any(a => a.id == payload.filter.created_by && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == a.dept_id) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == a.dept_id && b.sequence != null) == true)))) ||
                                         // document security level 3
                                         (ds2.id == 3 && (db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id && b.id == payload.filter.created_by) && s2.user_id == a.id)) ||
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by && b.sequence != null)) ||
                                         // document security level 4
                                         (ds2.id == 4 && (db.AccessUser.Any(a => a.id == payload.filter.created_by && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == a.dept_id)) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by && b.sequence != null))) ||
                                         /*
                                         // document security level 5
                                         (ds2.id == 5 && (db.AccessUser.Any(a => a.id == payload.filter.created_by && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == a.dept_id) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by && b.sequence != null)) ||
                                         */
                                         // document security level 6
                                         (ds2.id == 6 && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by && b.sequence != null)) ||
                                         //Special case for RECORDSECTION
                                         d.status == Constant.RecordStatus.MarkForArchive && payload.filter.record_section == true)
                                         select new DocumentsVM()
                                         {
                                             doc_stage = "F",
                                             sender_department_id = a3.dept_id,
                                             id = d.batch_id,
                                             status = d.status,
                                             delivery_type_id = dt2 == null ? 0 : dt2.id,
                                             category_code = c2 == null ? "" : c2.document_category_code,
                                             category_id = c2 == null ? 0 : c2.id,
                                             category_name = c2.document_category_name,
                                             created_by = d.created_by == 0 ? s2.user_id : d.created_by,
                                             created_date = d.created_date.ToString(),
                                             delivery_type_name = dt2.delivery_code,
                                             document_code = d.document_code,
                                             document_security_level = ds2.description,
                                             document_security_level_id = d.document_security_level_id,
                                             document_type_id = d.document_type_id,
                                             document_type_name = dtype2.document_type_code,
                                             etd_to_recipient = d.etd_to_recipient.ToString(),
                                             receipient_id = d.receipient_id,
                                             receipient_name = (r2.is_system_user ? string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)),
                                             sender_id = d.sender_id,
                                             sender_name = (s2.is_system_user ? string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)),
                                             updated_by = d.updated_by,
                                             updated_date = d.updated_date.ToString(),
                                             track_status = (d.status == Constant.RecordStatus.MarkForArchive ? "D" : "V")
                                         }).ToList();
                    }
                    if (_result.value.Count() <= 0)
                    {
                        _result.value = (from d in db.ArchivedDocuments
                                         join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = s.id, t2 = s.status } into s1
                                         from s2 in s1.DefaultIfEmpty()
                                         join s3 in db.SenderRecipientUser on new { t1 = s2.user_id, t2 = s2.is_system_user }
                                         equals new { t1 = s3.id, t2 = false } into s4
                                         from s5 in s4.DefaultIfEmpty()
                                         join a1 in db.AccessUser on new { t1 = s2.user_id, t2 = Constant.RecordStatus.Active, t3 = s2.is_system_user }
                                         equals new { t1 = a1.id, t2 = a1.status, t3 = true } into a2
                                         from a3 in a2.DefaultIfEmpty()
                                         join r in db.SenderRecipient on new { t1 = d.receipient_id, t2 = Constant.RecordStatus.Active }
                                         equals new { t1 = r.id, t2 = r.status } into r1
                                         from r2 in r1.DefaultIfEmpty()
                                         join r3 in db.SenderRecipientUser on new { t1 = r2.user_id, t2 = r2.is_system_user }
                                         equals new { t1 = r3.id, t2 = false } into r4
                                         from r5 in s4.DefaultIfEmpty()
                                         join ra1 in db.AccessUser on new { t1 = r2.user_id, t2 = Constant.RecordStatus.Active, t3 = r2.is_system_user }
                                         equals new { t1 = ra1.id, t2 = ra1.status, t3 = true } into ra2
                                         from ra3 in ra2.DefaultIfEmpty()
                                         join cat in db.DocumentCategory on new { t1 = d.category_id, t2 = Constant.RecordStatus.Active } equals new { t1 = cat.id, t2 = cat.status } into c1
                                         from c2 in c1.DefaultIfEmpty()
                                         join dt in db.Delivery on new { t1 = d.delivery_type_id, t2 = Constant.RecordStatus.Active } equals new { t1 = dt.id, t2 = dt.status } into dt1
                                         from dt2 in dt1.DefaultIfEmpty()
                                         join ds in db.DocumentSecurityLevel on d.document_security_level_id equals ds.id into ds1
                                         from ds2 in ds1.DefaultIfEmpty()
                                         join dtype in db.DocumentType on d.document_type_id equals dtype.id into dtype1
                                         from dtype2 in dtype1.DefaultIfEmpty()
                                         where
                                         (d.batch_id == payload.filter.id || d.document_code == payload.filter.barcode_no ||
                                         db.DocumentAttachment.Any(c => c.barcode_no == payload.filter.barcode_no && c.batch_id == d.batch_id)) &&
                                         ((d.status == Constant.RecordStatus.Active && (s2.user_id == payload.filter.created_by && db.DocumentRoutes.Any(a => a.sequence != null && a.batch_id == d.batch_id))) ||
                                         ds2.id == 1 ||
                                         // document security level 1
                                         (ds2.id == 1 && db.DocumentRoutes.Any(a => a.batch_id == d.batch_id && a.receipient_id == payload.filter.created_by && a.sequence == null) == false) ||
                                         // document security level 2
                                         (ds2.id == 2 && ((db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id && b.id == payload.filter.created_by) && s2.user_id == a.id)) ||
                                         (db.AccessUser.Any(a => a.id == payload.filter.created_by && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == a.dept_id) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == a.dept_id && b.sequence != null) == true)))) ||
                                         // document security level 3
                                         (ds2.id == 3 && (db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id && b.id == payload.filter.created_by) && s2.user_id == a.id)) ||
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by && b.sequence != null)) ||
                                         // document security level 4
                                         (ds2.id == 4 && (db.AccessUser.Any(a => a.id == payload.filter.created_by && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == a.dept_id)) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by && b.sequence != null))) ||
                                         /*
                                         // document security level 5
                                         (ds2.id == 5 && (db.AccessUser.Any(a => a.id == payload.filter.created_by && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.department_id == a.dept_id) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by && b.sequence != null)) ||
                                         */
                                         // document security level 6
                                         (ds2.id == 6 && db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by) &&
                                         db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.receipient_id == payload.filter.created_by && b.sequence != null)) ||
                                         //Special case for RECORDSECTION
                                         payload.filter.record_section == true)
                                         select new DocumentsVM()
                                         {
                                             doc_stage = "A",
                                             sender_department_id = a3.dept_id,
                                             id = d.batch_id,
                                             status = d.status,
                                             delivery_type_id = dt2 == null ? 0 : dt2.id,
                                             category_code = c2 == null ? "" : c2.document_category_code,
                                             category_id = c2 == null ? 0 : c2.id,
                                             category_name = c2.document_category_name,
                                             created_by = d.created_by == 0 ? s2.user_id : d.created_by,
                                             created_date = d.created_date.ToString(),
                                             delivery_type_name = dt2.delivery_code,
                                             document_code = d.document_code,
                                             document_security_level = ds2.description,
                                             document_security_level_id = d.document_security_level_id,
                                             document_type_id = d.document_type_id,
                                             document_type_name = dtype2.document_type_code,
                                             etd_to_recipient = d.etd_to_recipient.ToString(),
                                             receipient_id = d.receipient_id,
                                             receipient_name = (r2.is_system_user ? string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)),
                                             sender_id = d.sender_id,
                                             sender_name = (s2.is_system_user ? string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)),
                                             updated_by = d.updated_by,
                                             updated_date = d.updated_date.ToString(),
                                             track_status = "V"
                                         }).ToList();
                    }
                    break;
                default:
                    var firstDate = new DateTime(DateTime.Today.Year, 1, 1);
                    var lastDate = new DateTime(DateTime.Today.Year, 12, 31).AddDays(1);
                    var days5 = DateTime.Now.AddDays(-5);
                    var days5_a = DateTime.Now.AddDays(-4);
                    var weeks2 = DateTime.Now.AddDays(-14);
                    var weeks2_a = DateTime.Now.AddDays(-13);
                    var month1 = DateTime.Now.AddMonths(-1);
                    var month2 = DateTime.Now.AddMonths(-1).AddDays(1);
                    var docCatLst = new List<DocumentsVM>();
                    docCatLst.AddRange(db.OnHandDocuments.Where(a => a.status == Constant.RecordStatus.Active && db.AccessUser.ToList().
                                      Any(b => db.SenderRecipient.Any(sr => sr.id == a.sender_id && sr.user_id == b.id && sr.status == Constant.RecordStatus.Active) &&
                                      b.dept_id == payload.filter.department_id)).GroupBy(a => new { a.category_id }).
                        Select(b => new { key = b.Key, count = b.Count() }).Select(c => new DocumentsVM() { category_id = c.key.category_id, statistics = c.count }));
                    docCatLst.AddRange(db.ReceivedDocuments.Where(a => a.status == Constant.RecordStatus.Active && db.AccessUser.ToList().
                                      Any(b => db.SenderRecipient.Any(sr => sr.id == a.sender_id && sr.user_id == b.id && sr.status == Constant.RecordStatus.Active) &&
                                      b.dept_id == payload.filter.department_id &&
                             db.DocumentRoutes.Any(c => c.batch_id == a.id && b.dept_id == c.department_id && c.sequence != null))).GroupBy(a => new { a.category_id }).
                        Select(b => new { key = b.Key, count = b.Count() }).Select(c => new DocumentsVM() { category_id = c.key.category_id, statistics = c.count }));
                    docCatLst.AddRange(db.FinalizedDocuments.Where(a => (a.status == Constant.RecordStatus.Active || a.status == Constant.RecordStatus.MarkForArchive) &&
                    db.AccessUser.ToList().Any(b => db.SenderRecipient.Any(sr => sr.id == a.sender_id && sr.user_id == b.id && sr.status == Constant.RecordStatus.Active) &&
                    b.dept_id == payload.filter.department_id)).GroupBy(a => new { a.category_id }).
                        Select(b => new { key = b.Key, count = b.Count() }).Select(c => new DocumentsVM() { category_id = c.key.category_id, statistics = c.count }));
                    docCatLst.AddRange(db.ArchivedDocuments.Where(a => a.status == Constant.RecordStatus.Active && db.AccessUser.ToList().
                                      Any(b => db.SenderRecipient.Any(sr => sr.id == a.sender_id && sr.user_id == b.id && sr.status == Constant.RecordStatus.Active) &&
                                      b.dept_id == payload.filter.department_id)).GroupBy(a => new { a.category_id }).
                        Select(b => new { key = b.Key, count = b.Count() }).Select(c => new DocumentsVM() { category_id = c.key.category_id, statistics = c.count }));
                    docCatLst = docCatLst.GroupBy(a => new { a.category_id }).Select(b => new { key = b.Key, count = b.Sum(c => c.statistics) }).
                        Select(c => new DocumentsVM() { category_id = c.key.category_id, statistics = c.count }).ToList();

                    var tempValue = new List<DocumentsVM>()
                    {
                    new DocumentsVM() {
                        category_id= 1, id = 1,
                        process = "Summary", category_name = "On-Hand Documents",
                        statistics = (from d in db.OnHandDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.status == Constant.RecordStatus.Active && (d.created_date > firstDate && d.created_date <= lastDate) &&
                                      (((new int[] {4, 5, 6 }).Contains(d.document_security_level_id) && s2.user_id == payload.filter.created_by) ||
                                      ((new int[] {1, 2, 3 }).Contains(d.document_security_level_id) &&
                                      db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by) && s2.user_id == a.id))))
                                      select d).Count() + (from d in db.ReceivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.status == Constant.RecordStatus.Active && (d.created_date > firstDate && d.created_date <= lastDate) &&
                                      (((new int[] {4, 5, 6 }).Contains(d.document_security_level_id) && s2.user_id == payload.filter.created_by) ||
                                      ((new int[] {1, 2, 3 }).Contains(d.document_security_level_id) &&
                                      db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by) && s2.user_id == a.id))))
                                      select d).Count() + (from d in db.FinalizedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.status == Constant.RecordStatus.Active && (d.created_date > firstDate && d.created_date <= lastDate) &&
                                      (((new int[] {4, 5, 6 }).Contains(d.document_security_level_id) && s2.user_id == payload.filter.created_by) ||
                                      ((new int[] {1, 2, 3 }).Contains(d.document_security_level_id) &&
                                      db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by) && s2.user_id == a.id))))
                                      select d).Count() + (from d in db.ArchivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.status == Constant.RecordStatus.Active && (d.created_date > firstDate && d.created_date <= lastDate) &&
                                      (((new int[] {4, 5, 6 }).Contains(d.document_security_level_id) && s2.user_id == payload.filter.created_by) ||
                                      ((new int[] {1, 2, 3 }).Contains(d.document_security_level_id) &&
                                      db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by) && s2.user_id == a.id))))
                                      select d).Count()
                    },
                    new DocumentsVM() {
                        category_id = 1, id = 2,
                        process = "Summary", category_name = "Received Documents",
                        statistics = (from d in db.ReceivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where d.status == Constant.RecordStatus.Active && (d.updated_date > firstDate && d.updated_date <= lastDate) &&
                                      (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.sequence != null && ((b.department_id == payload.filter.department_id &&
                                      (d.document_security_level_id == 1 || d.document_security_level_id == 2 || d.document_security_level_id == 4)) ||
                                      ((d.document_security_level_id == 3 || d.document_security_level_id == 5 || d.document_security_level_id == 6) && b.receipient_id == payload.filter.created_by))))
                                      select d).Count() + (from d in db.FinalizedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where d.status == Constant.RecordStatus.Active && (d.updated_date > firstDate && d.updated_date <= lastDate) &&
                                      (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.sequence != null && ((b.department_id == payload.filter.department_id &&
                                      (d.document_security_level_id == 1 || d.document_security_level_id == 2 || d.document_security_level_id == 4)) ||
                                      ((d.document_security_level_id == 3 || d.document_security_level_id == 5 || d.document_security_level_id == 6) && b.receipient_id == payload.filter.created_by))))
                                      select d).Count() + (from d in db.ArchivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where d.status == Constant.RecordStatus.Active && (d.updated_date > firstDate && d.updated_date <= lastDate) &&
                                      (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.sequence != null && ((b.department_id == payload.filter.department_id &&
                                      (d.document_security_level_id == 1 || d.document_security_level_id == 2 || d.document_security_level_id == 4)) ||
                                      ((d.document_security_level_id == 3 || d.document_security_level_id == 5 || d.document_security_level_id == 6) && b.receipient_id == payload.filter.created_by))))
                                      select d).Count()
                    },
                    new DocumentsVM() {
                        category_id = 1, id = 3,
                        process = "Summary", category_name = "Finalized Documents",
                        statistics = (from d in db.FinalizedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.status == Constant.RecordStatus.Active || d.status == Constant.RecordStatus.MarkForArchive) &&
                                      (d.date_finalized > firstDate && d.date_finalized <= lastDate) && d.dept_finalized == payload.filter.department_id
                                      select d).Count() + (from d in db.ArchivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.status == Constant.RecordStatus.Active || d.status == Constant.RecordStatus.MarkForArchive) &&
                                      d.dept_finalized == payload.filter.department_id
                                      select d).Count()
                    },
                    new DocumentsVM() {
                        category_id = 1, id = 4,
                        process = "Summary", category_name = "Archived",
                        statistics = (from d in db.ArchivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.status == Constant.RecordStatus.Active || d.status == Constant.RecordStatus.MarkForArchive) &&
                                      (d.date_archived > firstDate && d.date_archived <= lastDate) &&
                                      (d.dept_archived == payload.filter.department_id || s2.user_id == payload.filter.created_by || payload.filter.record_section == true ||
                                      ((d.document_security_level_id == 1 || d.document_security_level_id == 2) &&
                                      (db.AccessUser.Any(a => a.id == s2.user_id && db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by)) || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                      b.department_id == payload.filter.department_id))) ||
                                      ((d.document_security_level_id == 3) && (db.AccessUser.Any(a => a.id == s2.user_id && db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by)) || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                      b.receipient_id == payload.filter.created_by))) ||
                                      ((d.document_security_level_id == 4) && (s2.user_id == payload.filter.created_by || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                      b.department_id == payload.filter.department_id))) ||
                                      ((d.document_security_level_id == 5 || d.document_security_level_id == 6) && ((s2.user_id == payload.filter.created_by) ||
                                      db.DocumentRoutes.Any(c => c.batch_id == d.batch_id && c.receipient_id == payload.filter.created_by))))
                                      select d).Count()
                    },
                    new DocumentsVM() {
                        category_id = 2, id = 1,
                        process = "Under Department", category_name = "On-Hand Documents",
                        statistics = (from d in db.OnHandDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.status == Constant.RecordStatus.Active &&
                                      (((new int[] {4, 5, 6 }).Contains(d.document_security_level_id) && s2.user_id == payload.filter.created_by) ||
                                      ((new int[] {1, 2, 3 }).Contains(d.document_security_level_id) &&
                                      db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by) && s2.user_id == a.id))))
                                      select d).Count()
                    },
                    new DocumentsVM() {
                        category_id = 2, id = 2,
                        process = "Under Department", category_name = "Received Documents",
                        statistics = (from d in db.ReceivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where d.status == Constant.RecordStatus.Active &&
                                      (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.current_receiver == true && ((b.department_id == payload.filter.department_id &&
                                      ((new int[] {1, 2, 4 }).Contains(d.document_security_level_id))) || (((new int[] {3, 5, 6 }).Contains(d.document_security_level_id)) &&
                                      b.receipient_id == payload.filter.created_by))))
                                      select d).Count()
                    },
                    new DocumentsVM() {
                        category_id = 2, id = 3,
                        process = "Under Department", category_name = "Finalized Documents",
                        statistics = (from d in db.FinalizedDocuments
                                     join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                     equals new { t1 = s.id, t2 = s.status } into s1
                                     from s2 in s1.DefaultIfEmpty()
                                     where (d.status == Constant.RecordStatus.Active || d.status == Constant.RecordStatus.MarkForArchive) &&
                                     (s2.user_id == payload.filter.created_by ||
                                     ((d.document_security_level_id == 1  || d.document_security_level_id == 2) &&
                                     (db.AccessUser.Any(a => a.id == s2.user_id && db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                     b.id == payload.filter.created_by)) || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                     b.department_id == payload.filter.department_id))) ||

                                     ((d.document_security_level_id == 3) && (db.AccessUser.Any(a => a.id == s2.user_id && db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                     b.id == payload.filter.created_by)) || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                     b.receipient_id == payload.filter.created_by))) ||

                                     ((d.document_security_level_id == 4) && (s2.user_id == payload.filter.created_by || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                     b.department_id == payload.filter.department_id))) ||

                                     ((d.document_security_level_id == 5 || d.document_security_level_id == 6) && ((s2.user_id == payload.filter.created_by) ||
                                     db.DocumentRoutes.Any(c => c.batch_id == d.batch_id && c.receipient_id == payload.filter.created_by)))) &&
                                     d.dept_finalized == payload.filter.department_id
                                     select d).Count()
                    },
                    new DocumentsVM() {
                        category_id = 2, id = 4,
                        process = "Under Department", category_name = "Archived",
                        statistics = (from d in db.ArchivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.status == Constant.RecordStatus.Active || d.status == Constant.RecordStatus.MarkForArchive) &&
                                      (d.dept_archived == payload.filter.department_id || s2.user_id == payload.filter.created_by || payload.filter.record_section == true ||
                                      ((d.document_security_level_id == 1  || d.document_security_level_id == 2) &&
                                      (db.AccessUser.Any(a => a.id == s2.user_id && db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by)) || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                      b.department_id == payload.filter.department_id))) ||

                                      ((d.document_security_level_id == 3) && (db.AccessUser.Any(a => a.id == s2.user_id && db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by)) || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                      b.receipient_id == payload.filter.created_by))) ||

                                      ((d.document_security_level_id == 4) && (s2.user_id == payload.filter.created_by || db.DocumentRoutes.Any(b => b.batch_id == d.batch_id &&
                                      b.department_id == payload.filter.department_id))) ||

                                      ((d.document_security_level_id == 5 || d.document_security_level_id == 6) && ((s2.user_id == payload.filter.created_by) ||
                                      db.DocumentRoutes.Any(c => c.batch_id == d.batch_id && c.receipient_id == payload.filter.created_by))))

                                      select d).Count()
                    },
                    new DocumentsVM() {
                        category_id = 3, id = 1,
                        process = "Document Aging", category_name = "More than 1 Month",
                        statistics = (from d in db.OnHandDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where d.created_date<= month2 && (d.status == Constant.RecordStatus.Active &&
                                      (((new int[] {4, 5, 6 }).Contains(d.document_security_level_id) && s2.user_id == payload.filter.created_by) ||
                                      ((new int[] {1, 2, 3 }).Contains(d.document_security_level_id) &&
                                      db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by) && s2.user_id == a.id))))
                                      select d).Count(),
                        statistics1 = (from d in db.ReceivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where d.created_date<= month2 && d.status == Constant.RecordStatus.Active &&
                                      (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.current_receiver == true && ((b.department_id == payload.filter.department_id &&
                                      ((new int[] {1, 2, 4 }).Contains(d.document_security_level_id))) || (((new int[] {3, 5, 6 }).Contains(d.document_security_level_id)) &&
                                      b.receipient_id == payload.filter.created_by))))
                                      select d).Count(),
                    },
                    new DocumentsVM() {
                        category_id = 3, id = 2,
                        process = "Document Aging", category_name = "2 Weeks to 1 Month",
                        statistics = (from d in db.OnHandDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.created_date>= month1 && d.created_date <= weeks2_a) && (d.status == Constant.RecordStatus.Active &&
                                      (((new int[] {4, 5, 6 }).Contains(d.document_security_level_id) && s2.user_id == payload.filter.created_by) ||
                                      ((new int[] {1, 2, 3 }).Contains(d.document_security_level_id) &&
                                      db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by) && s2.user_id == a.id))))
                                      select d).Count(),
                        statistics1 = (from d in db.ReceivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.created_date>= month1 && d.created_date <= weeks2_a) && d.status == Constant.RecordStatus.Active &&
                                      (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.current_receiver == true && ((b.department_id == payload.filter.department_id &&
                                      ((new int[] {1, 2, 4 }).Contains(d.document_security_level_id))) || (((new int[] {3, 5, 6 }).Contains(d.document_security_level_id)) &&
                                      b.receipient_id == payload.filter.created_by))))
                                      select d).Count(),
                    },
                    new DocumentsVM() {
                        category_id = 3, id = 3,
                        process = "Document Aging", category_name = "5 Days to 2 Weeks",
                        statistics = (from d in db.OnHandDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.created_date>= weeks2 && d.created_date <= days5_a) && (d.status == Constant.RecordStatus.Active &&
                                      (((new int[] {4, 5, 6 }).Contains(d.document_security_level_id) && s2.user_id == payload.filter.created_by) ||
                                      ((new int[] {1, 2, 3 }).Contains(d.document_security_level_id) &&
                                      db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by) && s2.user_id == a.id))))
                                      select d).Count(),
                        statistics1 = (from d in db.ReceivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where (d.created_date>= weeks2 && d.created_date <= days5_a) && d.status == Constant.RecordStatus.Active &&
                                      (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.current_receiver == true && ((b.department_id == payload.filter.department_id &&
                                      ((new int[] {1, 2, 4 }).Contains(d.document_security_level_id))) || (((new int[] {3, 5, 6 }).Contains(d.document_security_level_id)) &&
                                      b.receipient_id == payload.filter.created_by))))
                                      select d).Count(),
                    },
                    new DocumentsVM() {
                        category_id = 3, id = 4,
                        process = "Document Aging", category_name = "Less than 5 Days",
                        statistics = (from d in db.OnHandDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where d.created_date>= days5 && (d.status == Constant.RecordStatus.Active &&
                                      (((new int[] {4, 5, 6 }).Contains(d.document_security_level_id) && s2.user_id == payload.filter.created_by) ||
                                      ((new int[] {1, 2, 3 }).Contains(d.document_security_level_id) &&
                                      db.AccessUser.Any(a => db.AccessUser.Any(b => b.dept_id == a.dept_id &&
                                      b.id == payload.filter.created_by) && s2.user_id == a.id))))
                                      select d).Count(),
                        statistics1 = (from d in db.ReceivedDocuments
                                      join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                      equals new { t1 = s.id, t2 = s.status } into s1
                                      from s2 in s1.DefaultIfEmpty()
                                      where d.created_date>= days5 && d.status == Constant.RecordStatus.Active &&
                                      (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.current_receiver == true && ((b.department_id == payload.filter.department_id &&
                                      ((new int[] {1, 2, 4 }).Contains(d.document_security_level_id))) || (((new int[] {3, 5, 6 }).Contains(d.document_security_level_id)) &&
                                      b.receipient_id == payload.filter.created_by))))
                                      select d).Count(),
                    },
                };
                    //change take() and skip() for testing when data is less than 5
                    //top 5
                    tempValue = tempValue.Union(
                                    (from p in docCatLst
                                     join dc in db.DocumentCategory on p.category_id equals dc.id
                                     orderby p.statistics descending
                                     select new DocumentsVM()
                                     {
                                         process = "Documents by Category",
                                         category_id = 4,
                                         category_code = dc.document_category_code,
                                         category_name = dc.document_category_name,
                                         statistics = p.statistics
                                     }).Take(5)).ToList();
                    //others       
                    tempValue = tempValue.Union(
                                    from p1 in (from p in docCatLst
                                                join dc in db.DocumentCategory on p.category_id equals dc.id
                                                orderby p.statistics descending
                                                select p).Skip(5)
                                    group p1 by 0 into g
                                    select new DocumentsVM()
                                    {
                                        process = "Documents by Category",
                                        category_id = 4,
                                        category_code = "Others",
                                        category_name = "Others",
                                        statistics = g.Sum(p => p.statistics)
                                    }).ToList();

                    _result.value = tempValue.OrderBy(a => a.category_id).ThenBy(a => a.id);
                    break;
            }
            return _result;
        }
        public Result<IEnumerable<DocumentsVM>> Top5AgingDocuments(DocumentsPayload payload)
        {
            var timeStampNow = DateTime.Now.Ticks;
            var _result = new Result<IEnumerable<DocumentsVM>>();
            _result.value = (from d in db.ReceivedDocuments
                             join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                             equals new { t1 = s.id, t2 = s.status } into s1
                             from s2 in s1.DefaultIfEmpty()
                             join s3 in db.SenderRecipientUser on new { t1 = s2.user_id, t2 = s2.is_system_user }
                             equals new { t1 = s3.id, t2 = false } into s4
                             from s5 in s4.DefaultIfEmpty()
                             join a1 in db.AccessUser on new { t1 = s2.user_id, t2 = Constant.RecordStatus.Active, t3 = s2.is_system_user }
                             equals new { t1 = a1.id, t2 = a1.status, t3 = true } into a2
                             from a3 in a2.DefaultIfEmpty()
                             join a1d in db.Departments on a3.dept_id equals a1d.id into a2d
                             from a3d in a2d.DefaultIfEmpty()
                             join r in db.SenderRecipient on new { t1 = d.receipient_id, t2 = Constant.RecordStatus.Active }
                             equals new { t1 = r.id, t2 = r.status } into r1
                             from r2 in r1.DefaultIfEmpty()
                             join r3 in db.SenderRecipientUser on new { t1 = r2.user_id, t2 = r2.is_system_user }
                             equals new { t1 = r3.id, t2 = false } into r4
                             from r5 in r4.DefaultIfEmpty()
                             join ra1 in db.AccessUser on new { t1 = r2.user_id, t2 = Constant.RecordStatus.Active, t3 = r2.is_system_user }
                             equals new { t1 = ra1.id, t2 = ra1.status, t3 = true } into ra2
                             from ra3 in ra2.DefaultIfEmpty()
                             join cat in db.DocumentCategory on new { t1 = d.category_id, t2 = Constant.RecordStatus.Active } equals new { t1 = cat.id, t2 = cat.status } into c1
                             from c2 in c1.DefaultIfEmpty()
                             join dt in db.Delivery on new { t1 = d.delivery_type_id, t2 = Constant.RecordStatus.Active } equals new { t1 = dt.id, t2 = dt.status } into dt1
                             from dt2 in dt1.DefaultIfEmpty()
                             join ds in db.DocumentSecurityLevel on d.document_security_level_id equals ds.id into ds1
                             from ds2 in ds1.DefaultIfEmpty()
                             join dtype in db.DocumentType on d.document_type_id equals dtype.id into dtype1
                             from dtype2 in dtype1.DefaultIfEmpty()
                             where d.status == Constant.RecordStatus.Active && (payload.filter.id == 0 || d.batch_id == payload.filter.id) &&
                             (payload.filter.receipient_name == null || payload.filter.receipient_name.Replace(" ", "").Contains((r2.is_system_user ?
                             string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)).Replace(" ", ""))) &&
                             (payload.filter.sender_name == null || payload.filter.sender_name.Replace(" ", "").Contains((s2.is_system_user ?
                             string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)).Replace(" ", ""))) &&
                             (payload.filter.category_name == null || payload.filter.category_name.Contains(c2.document_category_code)) &&
                             (payload.filter.document_code == null || d.document_code.ToLower().Contains(payload.filter.document_code.ToLower())) &&
                             (payload.filter.document_type_name == null || payload.filter.document_type_name.Contains(dtype2.document_type_code)) &&
                             (payload.filter.etd_from == null || d.etd_to_recipient >= payload.filter.etd_from) &&
                             (payload.filter.etd_to == null || d.etd_to_recipient <= payload.filter.etd_to) &&
                             (payload.filter.date_submitted_from == null || d.created_date >= payload.filter.date_submitted_from) &&
                             (payload.filter.date_submitted_to == null || d.created_date <= payload.filter.date_submitted_to) &&
                             (payload.filter.barcode_no == null || db.DocumentAttachment.Any(c => c.barcode_no == payload.filter.barcode_no && c.batch_id == d.batch_id)) &&
                             (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.current_receiver == true && ((b.department_id == payload.filter.department_id &&
                             (ds2.id == 1 || ds2.id == 2 || ds2.id == 4)) || ((ds2.id == 3 || ds2.id == 5 || ds2.id == 6) && b.receipient_id == payload.filter.created_by))))
                             select new DocumentsVM()
                             {
                                 sender_department_id = a3.dept_id,
                                 id = d.batch_id,
                                 status = d.status,
                                 delivery_type_id = dt2 == null ? 0 : dt2.id,
                                 category_code = c2 == null ? "" : c2.document_category_code,
                                 category_id = c2 == null ? 0 : c2.id,
                                 category_name = c2.document_category_name,
                                 created_by = d.created_by,
                                 created_date = d.created_date.ToString(),
                                 is_edoc = d.is_edoc,
                                 delivery_type_name = dt2.delivery_code,
                                 document_code = d.document_code,
                                 document_security_level = ds2.description,
                                 document_security_level_id = d.document_security_level_id,
                                 document_type_id = d.document_type_id,
                                 document_type_name = dtype2.document_type_code,
                                 etd_to_recipient = d.etd_to_recipient.ToString(),
                                 receipient_id = d.receipient_id,
                                 receipient_name = (r2.is_system_user ? string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)),
                                 sender_id = d.sender_id,
                                 sender_name = (s2.is_system_user ? string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)),
                                 updated_by = d.updated_by,
                                 updated_date = d.updated_date.ToString(),
                                 sort_date = d.created_date,
                                 sender_department_name = a3d.dept_code,
                             }).OrderBy(a => a.sort_date).Take(5).ToList();
            return _result;
        }
        public Result<IEnumerable<DocumentsVM>> Create(DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.created_date = DateTime.Now;
                    param.updated_date = DateTime.Now;
                    var itm = new OnHandDocuments()
                    {
                        etd_to_recipient = param.document.etd_to_recipient.ToDate(),
                        receipient_id = param.document.receipient_id,
                        sender_id = param.document.sender_id,
                        status = Constant.RecordStatus.Active,
                        updated_by = param.document.updated_by,
                        updated_date = param.updated_date,
                        category_id = param.document.category_id,
                        created_by = param.document.created_by,
                        created_date = param.created_date,
                        delivery_type_id = param.document.delivery_type_id,
                        document_code = param.document.document_code,
                        document_security_level_id = param.document.document_security_level_id,
                        document_type_id = param.document.document_type_id
                    };
                    db.OnHandDocuments.Add(itm);
                    db.SaveChanges();
                    param.filter.id = itm.id;
                    return GetData(param);
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
            }
            return _result;
        }

        public Result<IEnumerable<DocumentsVM>> Update([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.updated_date = DateTime.Now;
                    db.OnHandDocuments.AddOrUpdate(new OnHandDocuments()
                    {
                        etd_to_recipient = param.document.etd_to_recipient.ToDate(),
                        receipient_id = param.document.receipient_id,
                        sender_id = param.document.sender_id,
                        status = param.document.status == null ? Constant.RecordStatus.Active : param.document.status,
                        updated_by = param.document.updated_by,
                        updated_date = param.updated_date,
                        category_id = param.document.category_id,
                        delivery_type_id = param.document.delivery_type_id,
                        document_code = param.document.document_code,
                        document_security_level_id = param.document.document_security_level_id,
                        document_type_id = param.document.document_type_id,
                        id = param.document.id
                    });
                    db.SaveChanges();
                    param.filter.id = param.document.id;
                    return GetData(param);
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
            }
            return _result;
        }
        public Result<IEnumerable<DocumentsVM>> UpdateStatus([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.updated_date = DateTime.Now;
                    param.id = 0;
                    var item = db.OnHandDocuments.Find(param.document.id);
                    item.status = param.document.status;
                    db.OnHandDocuments.AddOrUpdate(item);
                    db.SaveChanges();
                    return GetData(param);
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
            }
            return _result;
        }
        public Result<IEnumerable<DocumentsVM>> Promote([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    var itm = db.OnHandDocuments.Find(param.document.id);
                    if (itm != null)
                    {
                        db.ReceivedDocuments.Add(new ReceivedDocuments()
                        {
                            category_id = itm.category_id,
                            created_by = itm.created_by,
                            created_date = itm.created_date,
                            is_edoc = itm.is_edoc,
                            delivery_type_id = itm.delivery_type_id,
                            document_code = itm.document_code,
                            document_security_level_id = itm.document_security_level_id,
                            document_type_id = itm.document_type_id,
                            etd_to_recipient = itm.etd_to_recipient,
                            batch_id = itm.id,
                            receipient_id = itm.receipient_id,
                            sender_id = itm.sender_id,
                            status = itm.status,
                            updated_by = itm.updated_by,
                            updated_date = DateTime.Now,
                        });
                        db.OnHandDocuments.Remove(itm);
                        db.SaveChanges();
                    }
                    db.DocumentRoutes.Where(a => a.batch_id == param.document.id).ToList().ForEach(a =>
                    {
                        a.current_receiver = false;
                        db.DocumentRoutes.AddOrUpdate(a);
                    });
                    db.SaveChanges();
                    var routes = db.DocumentRoutes.Where(a => a.batch_id == param.document.id && (a.receipient_id == param.filter.created_by ||
                    a.department_id == param.filter.department_id)).FirstOrDefault();
                    if (routes != null)
                    {
                        routes.sequence = db.DocumentRoutes.Where(a => a.batch_id == param.document.id).Max(a => a.sequence).ToSafeInt32() + 1;
                        routes.current_receiver = true;
                        db.DocumentRoutes.AddOrUpdate(routes);
                        var docAtt = db.DocumentAttachment.Where(a => a.batch_id == param.document.id);
                        docAtt.ToList().ForEach(a =>
                        {
                            a.status = Constant.RecordStatus.Active;
                            db.DocumentAttachment.AddOrUpdate(a);
                        });
                        db.SaveChanges();
                    }
                    if (db.DocumentRoutes.Any(a => a.batch_id == param.document.id && a.is_sender == true) == false)
                    {
                        var seq = db.DocumentRoutes.Where(a => a.batch_id == param.document.id).Max(a => a.sequence).ToSafeInt32() + 1;
                        var docroute = (from d in db.OnHandDocuments
                                        join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                        equals new { t1 = s.id, t2 = s.status } into s1
                                        from sr in s1.DefaultIfEmpty()
                                        where d.id == param.document.id
                                        select new DocumentRoutesVM()
                                        {
                                            batch_id = param.document.id,
                                            receipient_id = sr.user_id,
                                            department_id = param.filter.department_id,
                                            updated_by = param.filter.created_by
                                        }).FirstOrDefault();
                        if (docroute == null)
                        {
                            docroute = (from d in db.ReceivedDocuments
                                        join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                        equals new { t1 = s.id, t2 = s.status } into s1
                                        from sr in s1.DefaultIfEmpty()
                                        where d.batch_id == param.document.id
                                        select new DocumentRoutesVM()
                                        {
                                            batch_id = param.document.id,
                                            receipient_id = sr.user_id,
                                            department_id = param.filter.department_id,
                                            updated_by = param.filter.created_by
                                        }).FirstOrDefault();
                        }
                        if (docroute == null)
                        {
                            docroute = (from d in db.FinalizedDocuments
                                        join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                        equals new { t1 = s.id, t2 = s.status } into s1
                                        from sr in s1.DefaultIfEmpty()
                                        where d.batch_id == param.document.id
                                        select new DocumentRoutesVM()
                                        {
                                            batch_id = param.document.id,
                                            receipient_id = sr.user_id,
                                            department_id = param.filter.department_id,
                                            updated_by = param.filter.created_by
                                        }).FirstOrDefault();
                        }
                        if (docroute == null)
                        {
                            docroute = (from d in db.ArchivedDocuments
                                        join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                                        equals new { t1 = s.id, t2 = s.status } into s1
                                        from sr in s1.DefaultIfEmpty()
                                        where d.batch_id == param.document.id
                                        select new DocumentRoutesVM()
                                        {
                                            batch_id = param.document.id,
                                            receipient_id = sr.user_id,
                                            department_id = param.filter.department_id,
                                            updated_by = param.filter.created_by
                                        }).FirstOrDefault();
                        }
                        if (docroute != null)
                        {
                            db.DocumentRoutes.AddOrUpdate(new DocumentRoutes()
                            {
                                batch_id = docroute.batch_id,
                                is_sender = true,
                                created_by = param.filter.created_by,
                                created_date = DateTime.Today,
                                current_receiver = true,
                                department_id = docroute.department_id,
                                receipient_id = docroute.receipient_id,
                                sequence = seq,
                                status = Constant.RecordStatus.Active,
                                updated_by = param.filter.created_by,
                                updated_date = DateTime.Today
                            });
                            db.SaveChanges();
                        }
                    }
                    return GetData(param);
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
            }
            return _result;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}