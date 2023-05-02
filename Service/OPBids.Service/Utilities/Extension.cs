using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using OPBids.Common;
using OPBids.Service.Models.Settings;
using OPBids.Entities.View.Setting;
using OPBids.Service.Models.ProjectRequest;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.DTS;

namespace OPBids.Service.Utilities
{
    public static class Extension
    {
        #region Domain

        public static AccessGroupType ToDomain(this AccessGroupTypeVM vm)
        {
            return new AccessGroupType()
            {
                id = vm.id,
                access_group_id = vm.access_group_id ?? 0,
                access_type_id = vm.access_type_id ?? 0,
                add_edit_data = vm.add_edit_data,
                delete_data = vm.delete_data,
                record_section = vm.record_section,
                view_transact_data = vm.view_transact_data,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static DocumentType ToDomain(this DocumentTypeVM vm)
        {
            return new DocumentType()
            {
                id = vm.id,
                document_category_id = vm.document_category_id,
                document_type_code = vm.document_type_code,
                document_type_description = vm.document_type_description,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static DocumentCategory ToDomain(this DocumentCategoryVM vm)
        {
            return new DocumentCategory()
            {
                id = vm.id,
                document_category_code = vm.document_category_code,
                document_category_name = vm.document_category_name,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static Delivery ToDomain(this DeliveryVM vm)
        {
            return new Delivery()
            {
                id = vm.id,
                delivery_code = vm.delivery_code,
                delivery_description = vm.delivery_description,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static Department ToDomain(this DepartmentsVM vm)
        {
            return new Department()
            {
                id = vm.id,
                dept_code = vm.dept_code,
                dept_description = vm.dept_description,
                headed_by = vm.headed_by,
                designation = vm.designation,
                status = vm.status,
                parent_dept_id = vm.parent_dept_id,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static AccessType ToDomain(this AccessTypesVM vm)
        {
            return new AccessType()
            {
                id = vm.id,
                code = vm.code,
                name = vm.name,
                seq_no = vm.seq_no,
                parent_id = vm.parent_id,
                description = vm.description,
                status = vm.status,
                sys_id = vm.sys_id,
                view_transact_data = vm.view_transact_data,
                delete_data = vm.delete_data,
                add_edit_data = vm.add_edit_data,
                record_section = vm.record_section,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static BarcodeSetting ToDomain(this BarcodeSettingVM vm)
        {
            return new BarcodeSetting()
            {
                id = vm.id,
                barcode_only = vm.barcode_only,
                barcode_with_print_date = vm.barcode_with_print_date,
                qr_only = vm.qr_only,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static AccessUser ToDomain(this AccessUsersVM vm)
        {
            return new AccessUser()
            {
                id = vm.id,
                salutation = vm.salutation,
                first_name = vm.first_name,
                mi = vm.mi,
                last_name = vm.last_name,
                group_id = vm.group_id,
                password = vm.password,
                status = vm.status,
                email_address = vm.email_address,
                activation_code = vm.activation_code,
                username = vm.username,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static AccessGroup ToDomain(this AccessGroupVM vm)
        {
            return new AccessGroup()
            {
                id = vm.id,
                group_code = vm.group_code,
                group_description = vm.group_description,
                status = vm.status,
                dashboard_id = vm.dashboard_id,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectRequest ToDomain(this ProjectRequestVM vm)
        {
            DateTime? _earmark_date = null, _routed_date = null, _submitted_date = null, _rfq_deadline = null, _rfq_requested_date = null;

            int _pr = 0;
            int.TryParse(vm.pr_number, out _pr);
            if (vm.earmark_date != null) { _earmark_date = DateTime.ParseExact(vm.earmark_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture); }
            if (vm.routed_date != null) { _routed_date = DateTime.ParseExact(vm.earmark_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture); }
            if (vm.submitted_date != null) { _submitted_date = DateTime.ParseExact(vm.submitted_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture); }

            if (vm.rfq_deadline != null) { _rfq_deadline = DateTime.ParseExact(vm.rfq_deadline, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.rfq_request_date != null) { _rfq_requested_date = DateTime.ParseExact(vm.rfq_request_date, Constant.DateFormat, CultureInfo.InvariantCulture); }

            return new ProjectRequest()
            {
                id = vm.id,
                title = vm.title,
                description = vm.description,
                grantee = vm.grantee,
                estimated_budget = vm.estimated_budget.ToSafeDecimal(),
                approved_budget = vm.approved_budget.ToSafeDecimal(),
                required_date = vm.required_date != null ? DateTime.ParseExact(vm.required_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                category = vm.category,
                classification = vm.classification,
                contract_type = vm.contract_type,
                security_level = Convert.ToInt16(vm.security_level),
                delivery_type = Convert.ToInt16(vm.delivery_type),
                earmark = vm.earmark,
                earmark_date = _earmark_date,
                source_fund = vm.source_fund,
                batch_id = vm.batch_id,
                project_status = vm.project_status,
                project_substatus = vm.project_substatus,
                record_status = vm.record_status,
                sla = vm.sla,
                current_user = vm.current_user,
                user_action = vm.user_action,
                notes = vm.notes,
                routed_date = _routed_date,
                pr_number = _pr,
                submitted_date = _submitted_date,
                created_by = vm.created_by,
                rfq_deadline = _rfq_deadline,
                rfq_place = vm.rfq_place,
                rfq_requestor = vm.rfq_requestor,
                rfq_requestor_dept = vm.rfq_requestor_dept,
                rfq_request_date = _rfq_requested_date,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectRequestBatch ToDomain(this ProjectRequestBatchVM vm)
        {
            DateTime? _pre_bid_date = null, _bid_deadline_date = null, _bid_opening_date = null, _philgeps_publish_date = null, _mmda_publish_date = null, _conspost_date_lobby = null,
                _conspost_date_reception = null, _conspost_date_command = null, _newspaper_sent_date = null, _newspaper_post_date = null;

            if (vm.pre_bid_date != null)
            {
                _pre_bid_date = DateTime.Parse(String.Format("{0} {1}", vm.pre_bid_date, vm.pre_bid_time));
            }
            if (vm.bid_deadline_date != null)
            {
                _bid_deadline_date = DateTime.Parse(String.Format("{0} {1}", vm.bid_deadline_date, vm.bid_deadline_time));
            }
            if (vm.bid_opening_date != null)
            {
                _bid_opening_date = DateTime.Parse(String.Format("{0} {1}", vm.bid_opening_date, vm.bid_opening_time));
            }
            if (vm.philgeps_publish_date != null) { _philgeps_publish_date = DateTime.ParseExact(vm.philgeps_publish_date, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.mmda_publish_date != null) { _mmda_publish_date = DateTime.ParseExact(vm.mmda_publish_date, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.conspost_date_lobby != null) { _conspost_date_lobby = DateTime.ParseExact(vm.conspost_date_lobby, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.conspost_date_reception != null) { _conspost_date_reception = DateTime.ParseExact(vm.conspost_date_reception, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.conspost_date_command != null) { _conspost_date_command = DateTime.ParseExact(vm.conspost_date_command, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.newspaper_sent_date != null) { _newspaper_sent_date = DateTime.ParseExact(vm.newspaper_sent_date, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.newspaper_post_date != null) { _newspaper_post_date = DateTime.ParseExact(vm.newspaper_post_date, Constant.DateFormat, CultureInfo.InvariantCulture); }

            return new ProjectRequestBatch()
            {
                id = vm.id,
                applicable_year = vm.applicable_year,
                procurement_method = vm.procurement_method,
                pre_bid_date = _pre_bid_date,
                pre_bid_place = vm.pre_bid_place,
                bid_deadline_date = _bid_deadline_date,
                bid_deadline_place = vm.bid_deadline_place,
                bid_opening_date = _bid_opening_date,
                bid_opening_place = vm.bid_opening_place,
                bid_notes = vm.bid_notes,
                philgeps_publish_date = _philgeps_publish_date,
                philgeps_publish_by = vm.philgeps_publish_by,
                mmda_publish_date = _mmda_publish_date,
                mmda_publish_by = vm.mmda_publish_by,
                conspost_date_lobby = _conspost_date_lobby,
                conspost_date_reception = _conspost_date_reception,
                conspost_date_command = _conspost_date_command,
                conspost_by = vm.conspost_by,
                newspaper_sent_date = _newspaper_sent_date,
                newspaper_publisher = vm.newspaper_publisher,
                newspaper_received_by = vm.newspaper_received_by,
                newspaper_post_date = _newspaper_post_date,
                newspaper_post_by = vm.newspaper_post_by,
                project_status = vm.project_status,
                project_substatus = vm.project_substatus,
                record_status = vm.record_status,
                sla = vm.sla,
                current_user = vm.current_user,
                user_action = vm.user_action,
                created_by = vm.created_by,
                updated_by = vm.updated_by,
                notes = vm.notes,
                philgeps_att = vm.philgeps_att,
                mmda_portal_att = vm.mmda_portal_att,
                conspost_lobby_att = vm.conspost_lobby_att,
                conspost_reception_att = vm.conspost_reception_att,
                conspost_command_att = vm.conspost_command_att,
                newspaper_att = vm.newspaper_att

            };
        }

        public static Supplier ToDomain(this SupplierVM vm)
        {
            return new Supplier()
            {
                id = vm.id,
                comp_name = vm.comp_name,
                company_code = vm.company_code,
                address = vm.address,
                contact_no = vm.contact_no,
                contact_person = vm.contact_person,
                email = vm.email,
                tin = vm.tin,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static Category ToDomain(this CategoryVM vm)
        {
            return new Category()
            {
                id = vm.id,
                cat_code = vm.cat_code,
                cat_name = vm.cat_name,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static RecordCategory ToDomain(this RecordCategoryVM vm)
        {
            return new RecordCategory()
            {
                id = vm.id,
                classification_id = vm.classification_id,
                category_code = vm.category_code,
                category_desc = vm.category_desc,
                retention_period = vm.retention_period,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static RecordClassification ToDomain(this RecordClassificationVM vm)
        {
            return new RecordClassification()
            {
                id = vm.id,
                classification_code = vm.classification_code,
                classification_desc = vm.classification_desc,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static Workflow ToDomain(this WorkflowVM vm)
        {
            return new Workflow()
            {
                //id = vm.id,
                //type = vm.type,
                //seq_title = vm.seq_title,
                //seq_no = vm.seq_no,
                //seq_description = vm.seq_description,
                //actor = vm.actor,
                //sla = vm.sla,
                //status = vm.status,
                //created_by = vm.created_by,
                //created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DataTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                //updated_by = vm.updated_by,
                //updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DataTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now                 
            };
        }

        public static ProcurementMethod ToDomain(this ProcurementMethodVM vm)
        {
            return new ProcurementMethod()
            {
                id = vm.id,
                proc_code = vm.proc_code,
                procurement_description = vm.procurement_description,
                procurement_mode = vm.procurement_mode,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProcurementType ToDomain(this ProcurementTypeVM vm)
        {
            return new ProcurementType()
            {
                id = vm.id,
                proc_type = vm.proc_type,
                proc_typedesc = vm.proc_typedesc,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ContractType ToDomain(this ContractTypeVM vm)
        {
            return new ContractType()
            {
                id = vm.id,
                contract_type = vm.contract_type,
                contract_desc = vm.contract_desc,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static SourceFunds ToDomain(this SourceFundsVM vm)
        {
            return new SourceFunds()
            {
                id = vm.id,
                source_code = vm.source_code,
                source_description = vm.source_description,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectCategory ToDomain(this ProjectCategoryVM vm)
        {
            return new ProjectCategory()
            {
                id = vm.id,
                proj_cat = vm.proj_cat,
                proj_desc = vm.proj_desc,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectSubCategory ToDomain(this ProjectSubCategoryVM vm)
        {
            return new ProjectSubCategory()
            {
                id = vm.id,
                proj_catid = vm.proj_catid,
                proj_subcat = vm.proj_subcat,
                proj_subcatdesc = vm.proj_subcatdesc,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectStatus ToDomain(this ProjectStatusVM vm)
        {
            return new ProjectStatus()
            {
                id = vm.id,
                proj_statcode = vm.proj_statcode,
                proj_statdescription = vm.proj_statdescription,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectSubStatus ToDomain(this ProjectSubStatusVM vm)
        {
            return new ProjectSubStatus()
            {
                id = vm.id,
                proj_statid = vm.proj_statid,
                proj_substatcode = vm.proj_substatcode,
                proj_substatdescription = vm.proj_substatdescription,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectProponent ToDomain(this ProjectProponentVM vm)
        {
            return new ProjectProponent()
            {
                id = vm.id,
                proponent_name = vm.proponent_name,
                proponent_designation = vm.proponent_designation,
                dept_id = vm.dept_id,
                proponent_emailadd = vm.proponent_emailadd,
                proponent_contactno = vm.proponent_contactno,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectRequestAttachment ToDomain(this ProjectRequestAttachmentVM vm)
        {
            return new ProjectRequestAttachment()
            {
                id = vm.id,
                project_id = vm.project_id,
                barcode_no = vm.barcode_no,
                batch_id = vm.batch_id,
                attachment_name = vm.attachment_name,
                file_name = vm.file_name,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectBid ToDomain(this ProjectBidVM vm)
        {
            DateTime? _prepared_date = null;
            if (vm.prepared_date != null) { _prepared_date = DateTime.ParseExact(vm.prepared_date, Constant.DateFormat, CultureInfo.InvariantCulture); }
            return new ProjectBid()
            {
                id = vm.id,
                project_request_id = vm.project_request_id,
                bid_amount = Convert.ToInt32(vm.bid_amount),
                bid_bond = vm.bid_bond,
                bid_form = vm.bid_form,
                duration = vm.duration,
                variance = vm.variance,
                bid_status = vm.bid_status,
                record_status = vm.record_status,
                prepared_by = vm.prepared_by,
                prepared_date = _prepared_date,
                procured_docs = vm.procured_docs,
                eval_result = vm.eval_result,
                gen_eval = vm.gen_eval,
                notes = vm.notes,
                created_by = vm.created_by,
                updated_by = vm.updated_by
            };
        }

        public static ProjectBidChecklist ToDomain(this ProjectBidChecklistVM vm)
        {
            return new ProjectBidChecklist()
            {
                id = vm.id,
                project_request_id = vm.project_request_id,
                bid_id = vm.bid_id,
                stage = vm.stage,
                checklist_id = vm.checklist_id,
                type = vm.type,
                notes = vm.notes,
                eligibility = vm.eligibility,
                created_by = vm.created_by,
                updated_by = vm.updated_by
            };
        }

        public static ProjectRequestItem ToDomain(this ProjectRequestItemVM vm)
        {
            return new ProjectRequestItem()
            {
                id = vm.id,
                project_id = vm.project_id,
                unit = vm.unit,
                description = vm.description,
                quantity = vm.quantity.ToSafeInt(),
                unit_cost = vm.unit_cost.ToSafeDecimal(),
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectAreas ToDomain(this ProjectAreasVM vm)
        {
            return new ProjectAreas()
            {
                id = vm.id,
                city_id = vm.city_id,
                district_id = vm.district_id,
                barangay_id = vm.barangay_id,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectAreasCity ToDomain(this ProjectAreasCityVM vm)
        {
            return new ProjectAreasCity()
            {
                id = vm.id,
                city_name = vm.city_name,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectAreasDistrict ToDomain(this ProjectAreasDistrictVM vm)
        {
            return new ProjectAreasDistrict()
            {
                id = vm.id,
                city_id = vm.city_id,
                district_name = vm.district_name,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectAreasBarangay ToDomain(this ProjectAreasBarangayVM vm)
        {
            return new ProjectAreasBarangay()
            {
                id = vm.id,
                city_id = vm.city_id,
                district_id = vm.district_id,
                barangay_name = vm.barangay_name,
                status = vm.status,
                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }

        public static ProjectRequestAdvertisement ToDomain(this ProjectRequestAdvertisementVM vm)
        {
            DateTime? _philgeps_publish_date = null, _mmda_publish_date = null, _conspost_date_lobby = null, _conspost_date_reception = null, _conspost_date_command = null;
            DateTime? _newspaper_sent_date = null, _newspaper_post_date = null;

            if (vm.philgeps_publish_date != null) { _philgeps_publish_date = DateTime.ParseExact(vm.philgeps_publish_date, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.mmda_publish_date != null) { _mmda_publish_date = DateTime.ParseExact(vm.mmda_publish_date, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.conspost_date_lobby != null) { _conspost_date_lobby = DateTime.ParseExact(vm.conspost_date_lobby, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.conspost_date_reception != null) { _conspost_date_reception = DateTime.ParseExact(vm.conspost_date_reception, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.conspost_date_command != null) { _conspost_date_command = DateTime.ParseExact(vm.conspost_date_command, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.newspaper_sent_date != null) { _newspaper_sent_date = DateTime.ParseExact(vm.newspaper_sent_date, Constant.DateFormat, CultureInfo.InvariantCulture); }
            if (vm.newspaper_post_date != null) { _newspaper_post_date = DateTime.ParseExact(vm.newspaper_post_date, Constant.DateFormat, CultureInfo.InvariantCulture); }

            return new ProjectRequestAdvertisement()
            {
                id = vm.id,

                project_request_id = vm.project_request_id,
                philgeps_publish_date = _philgeps_publish_date,
                philgeps_publish_by = vm.philgeps_publish_by,

                mmda_publish_date = _mmda_publish_date,
                mmda_publish_by = vm.mmda_publish_by,

                conspost_date_lobby = _conspost_date_lobby,
                conspost_date_reception = _conspost_date_reception,
                conspost_date_command = _conspost_date_command,
                conspost_by = vm.conspost_by,

                newspaper_sent_date = _newspaper_sent_date,
                newspaper_publisher = vm.newspaper_publisher,
                newspaper_received_by = vm.newspaper_received_by,
                newspaper_post_date = _newspaper_post_date,
                newspaper_post_by = vm.newspaper_post_by,

                created_by = vm.created_by,
                created_date = vm.created_date != null ? DateTime.ParseExact(vm.created_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now,
                updated_by = vm.updated_by,
                updated_date = vm.updated_date != null ? DateTime.ParseExact(vm.updated_date, Constant.DateTimeFormat, CultureInfo.InvariantCulture) : DateTime.Now
            };
        }
        #endregion

        #region History

        public static ProjectRequestHistory ToHistory(this ProjectRequest main)
        {
            return new ProjectRequestHistory()
            {
                project_request_id = main.id,
                title = main.title,
                description = main.description,
                grantee = main.grantee,
                estimated_budget = main.estimated_budget,
                approved_budget = main.approved_budget,
                required_date = main.required_date,
                category = main.category,
                classification = main.classification,
                contract_type = main.contract_type,
                security_level = main.security_level,
                devivery_type = main.delivery_type,
                earmark = main.earmark,
                earmark_date = main.earmark_date,
                source_fund = main.source_fund,
                batch_id = main.batch_id,
                project_status = main.project_status,
                project_substatus = main.project_substatus,
                record_status = main.record_status,
                sla = main.sla,
                current_user = main.current_user,
                user_action = main.user_action,
                notes = main.notes,
                routed_date = main.routed_date,
                created_by = main.created_by,
                created_date = main.created_date,
                updated_by = main.updated_by,
                updated_date = main.updated_date
            };
        }

        public static ProjectRequestBatchHistory ToHistory(this ProjectRequestBatch main)
        {
            return new ProjectRequestBatchHistory()
            {
                batch_id = main.id,
                applicable_year = main.applicable_year,
                procurement_method = main.procurement_method,
                pre_bid_date = main.pre_bid_date,
                pre_bid_place = main.pre_bid_place,
                bid_deadline_date = main.bid_deadline_date,
                bid_deadline_place = main.bid_deadline_place,
                bid_opening_date = main.bid_opening_date,
                bid_opening_place = main.bid_opening_place,
                bid_notes = main.bid_notes,
                philgeps_publish_date = main.philgeps_publish_date,
                philgeps_publish_by = main.philgeps_publish_by,
                mmda_publish_date = main.mmda_publish_date,
                mmda_publish_by = main.mmda_publish_by,
                conspost_date_lobby = main.conspost_date_lobby,
                conspost_date_reception = main.conspost_date_reception,
                conspost_date_command = main.conspost_date_command,
                conspost_by = main.conspost_by,
                newspaper_sent_date = main.newspaper_sent_date,
                newspaper_publisher = main.newspaper_publisher,
                newspaper_received_by = main.newspaper_received_by,
                newspaper_post_date = main.newspaper_post_date,
                newspaper_post_by = main.newspaper_post_by,
                project_status = main.project_status,
                project_substatus = main.project_substatus,
                record_status = main.record_status,
                sla = main.sla,
                current_user = main.current_user,
                user_action = main.user_action,
                notes = main.notes,
                routed_date = main.routed_date,

                created_by = main.created_by,
                created_date = main.created_date,
                updated_by = main.updated_by,
                updated_date = main.updated_date
            };
        }

        #endregion

        #region ToViewModel

        public static ProjectRequestVM ToView(this ProjectRequest domain)
        {
            int remaining_days = (domain.required_date.Date - DateTime.Now.Date).Days;

            return new ProjectRequestVM()
            {
                id = domain.id,
                title = domain.title,
                description = domain.description,
                grantee = domain.grantee,
                estimated_budget = domain.estimated_budget.ToString("N", CultureInfo.CurrentCulture),
                approved_budget = domain.approved_budget.ToString("N", CultureInfo.CurrentCulture),
                required_date = domain.required_date.ToString(Constant.DateFormat),
                category = domain.category,
                classification = domain.classification,
                contract_type = domain.contract_type,
                security_level = domain.security_level.ToString(),
                delivery_type = domain.delivery_type.ToString(),
                earmark = domain.earmark,
                earmark_date = domain.earmark_date?.ToString(Constant.DateFormat),
                source_fund = domain.source_fund,
                batch_id = domain.batch_id,
                project_status = domain.project_status,
                project_substatus = domain.project_substatus,
                record_status = domain.record_status,
                sla = domain.sla,
                current_user = domain.current_user,
                user_action = domain.user_action,
                notes = domain.notes,
                pr_number = domain.pr_number.ToString(),
                submitted_date = domain.submitted_date?.ToString(Constant.DateFormat),
                routed_date = domain.routed_date?.ToString(Constant.DateFormat),

                rfq_deadline = domain.rfq_deadline?.ToString(Constant.DateFormat),
                rfq_place = domain.rfq_place,
                rfq_requestor = domain.rfq_requestor,
                rfq_requestor_dept = domain.rfq_requestor_dept,
                rfq_request_date = domain.rfq_request_date?.ToString(Constant.DateFormat),

                created_by = domain.created_by,
                created_date = domain.created_date.ToString(Constant.DateFormat),
                updated_by = domain.updated_by,
                updated_date = domain.updated_date.ToString(Constant.DateFormat),

                start_date = domain.start_date?.ToString(Constant.DateFormat),
                completed_date = domain.completed_date?.ToString(Constant.DateFormat),

                days_to_required_date = remaining_days //< 0 ? 0 : remaining_days

            };
        }

        public static ProjectRequestBatchVM ToView(this ProjectRequestBatch domain, string proc_mode = "")
        {
            return new ProjectRequestBatchVM()
            {
                id = domain.id,
                applicable_year = domain.applicable_year,
                procurement_method = domain.procurement_method,
                pre_bid_date = domain.pre_bid_date?.ToString(Constant.DateFormat),
                pre_bid_time = domain.pre_bid_date?.ToString(Constant.TimeFormat),
                pre_bid_place = domain.pre_bid_place,
                bid_deadline_date = domain.bid_deadline_date?.ToString(Constant.DateFormat),
                bid_deadline_time = domain.bid_deadline_date?.ToString(Constant.TimeFormat),
                bid_deadline_place = domain.bid_deadline_place,
                bid_opening_date = domain.bid_opening_date?.ToString(Constant.DateFormat),
                bid_opening_time = domain.bid_opening_date?.ToString(Constant.TimeFormat),
                bid_opening_place = domain.bid_opening_place,
                bid_notes = domain.bid_notes,
                philgeps_publish_date = domain.philgeps_publish_date?.ToString(Constant.DateFormat),
                philgeps_publish_by = domain.philgeps_publish_by,
                mmda_publish_date = domain.mmda_publish_date?.ToString(Constant.DateFormat),
                mmda_publish_by = domain.mmda_publish_by,
                conspost_date_lobby = domain.conspost_date_lobby?.ToString(Constant.DateTimeFormat),
                conspost_date_reception = domain.conspost_date_reception?.ToString(Constant.DateTimeFormat),
                conspost_date_command = domain.conspost_date_command?.ToString(Constant.DateTimeFormat),
                conspost_by = domain.conspost_by,
                newspaper_sent_date = domain.newspaper_sent_date?.ToString(Constant.DateTimeFormat),
                newspaper_publisher = domain.newspaper_publisher,
                newspaper_received_by = domain.newspaper_received_by,
                newspaper_post_date = domain.newspaper_post_date?.ToString(Constant.DateTimeFormat),
                newspaper_post_by = domain.newspaper_post_by,
                project_status = domain.project_status,
                project_substatus = domain.project_substatus,
                record_status = domain.record_status,
                sla = domain.sla,
                current_user = domain.current_user,
                user_action = domain.user_action,
                notes = domain.notes,
                procurement_mode = proc_mode,
                created_by = domain.created_by,
                created_date = domain.created_date.ToString(Constant.DateFormat),
                updated_by = domain.updated_by,
                updated_date = domain.updated_date.ToString(Constant.DateFormat),
                mmda_portal_att = domain.mmda_portal_att,
                conspost_lobby_att = domain.conspost_lobby_att,
                conspost_command_att = domain.conspost_command_att,
                conspost_reception_att = domain.conspost_reception_att,
                newspaper_att = domain.newspaper_att,
                philgeps_att = domain.philgeps_att
            };
        }

        public static ProjectRequestHistoryVM ToView(this ProjectRequestHistory domain)
        {
            return new ProjectRequestHistoryVM()
            {
                id = domain.id,
                project_request_id = domain.project_request_id,
                notes = domain.notes,
                change_log = domain.change_log,
                user_action = domain.user_action,
                created_by = domain.created_by,
                created_date = domain.created_date.ToString(Constant.DateFormat),
                created_time = domain.created_date.ToString(Constant.TimeFormat),
                updated_by = domain.updated_by,
                updated_date = domain.updated_date.ToString(Constant.DateFormat)
            };
        }

        public static AccessUsersVM ToView(this AccessUser domain, int dashboard_id = 0, int dept_id = 0, string dept_desc = "", string dept_code = "")

        {
            return new AccessUsersVM()
            {
                id = domain.id,
                salutation = domain.salutation,
                first_name = domain.first_name,
                mi = domain.mi,
                last_name = domain.last_name,
                group_id = domain.group_id,
                password = domain.password,
                status = domain.status,
                email_address = domain.email_address,
                activation_code = domain.activation_code,
                username = domain.username,
                created_by = domain.created_by,
                created_date = domain.created_date.ToString(Constant.DateTimeFormat),
                updated_by = domain.updated_by,
                updated_date = domain.updated_date.ToString(Constant.DateTimeFormat),
                dept_id = dept_id,
                dashboard_id = dashboard_id,
                dept_description = dept_desc,
                dept_code = dept_code,
                dts_access = domain.dts_access,
                pfms_access = domain.pfms_access,
                vip_access = domain.vip_access
            };
        }

        public static ProjectBidVM ToView(this ProjectBid domain, string name = "", string address = "", string rep = "")
        {
            return new ProjectBidVM()
            {
                id = domain.id,
                project_request_id = domain.project_request_id,
                bid_amount = String.Format("{0:0.00}", domain.bid_amount),
                bid_bond = domain.bid_bond,
                bid_form = domain.bid_form,
                duration = domain.duration,
                variance = domain.variance,
                bid_status = domain.bid_status,
                record_status = domain.record_status,
                prepared_by = domain.prepared_by,
                prepared_date = domain.prepared_date?.ToString(Constant.DateFormat),
                procured_docs = domain.procured_docs,
                eval_result = domain.eval_result,
                gen_eval = domain.gen_eval,
                notes = domain.notes,
                bidder_name = name,
                bidder_address = address,
                auth_rep = rep,
                created_by = domain.created_by,
                updated_by = domain.updated_by
            };
        }

        public static ProjectRequestAttachmentVM ToView(this ProjectRequestAttachment domain)
        {
            return new ProjectRequestAttachmentVM()
            {
                id = domain.id,
                project_id = domain.project_id,
                attachment_name = domain.attachment_name,
                barcode_no = domain.barcode_no,
                batch_id = domain.batch_id,
                file_name = domain.file_name,
                status = domain.status,
                created_by = domain.created_by,
                created_date = domain.created_date.ToString(Constant.DateFormat),
                updated_by = domain.updated_by,
                updated_date = domain.updated_date.ToString(Constant.DateFormat)
            };

        }

        public static ProjectBidChecklistVM ToView(this ProjectBidChecklist domain)
        {
            return new ProjectBidChecklistVM()
            {
                id = domain.id,
                project_request_id = domain.project_request_id,
                bid_id = domain.bid_id,
                stage = domain.stage,
                checklist_id = domain.checklist_id,
                type = domain.type,
                notes = domain.notes,
                eligibility = domain.eligibility,
                created_by = domain.created_by,
                created_date = domain.created_date.ToString(Constant.DateFormat),
                updated_by = domain.updated_by,
                updated_date = domain.updated_date.ToString(Constant.DateFormat)
            };
        }

        public static ProjectRequestItemVM ToView(this ProjectRequestItem domain)
        {
            return new ProjectRequestItemVM()
            {
                id = domain.id,
                project_id = domain.project_id,
                description = domain.description,
                unit = domain.unit,
                quantity = domain.quantity.ToString(),
                unit_cost = domain.unit_cost.ToString("N2"),
                created_by = domain.created_by,
                created_date = domain.created_date.ToString(Constant.DateFormat),
                updated_by = domain.updated_by,
                updated_date = domain.updated_date.ToString(Constant.DateFormat)
            };

        }

        public static ProjectRequestAdvertisementVM ToView(this ProjectRequestAdvertisement domain)
        {
            return new ProjectRequestAdvertisementVM()
            {
                id = domain.id,

                project_request_id = domain.project_request_id,
                philgeps_publish_date = domain.philgeps_publish_date?.ToString(Constant.DateFormat),
                philgeps_publish_by = domain.philgeps_publish_by,

                mmda_publish_date = domain.mmda_publish_date?.ToString(Constant.DateFormat),
                mmda_publish_by = domain.mmda_publish_by,

                conspost_date_lobby = domain.conspost_date_lobby?.ToString(Constant.DateFormat),
                conspost_date_reception = domain.conspost_date_reception?.ToString(Constant.DateFormat),
                conspost_date_command = domain.conspost_date_command?.ToString(Constant.DateFormat),
                conspost_by = domain.conspost_by,

                newspaper_sent_date = domain.newspaper_sent_date?.ToString(Constant.DateFormat),
                newspaper_publisher = domain.newspaper_publisher,
                newspaper_received_by = domain.newspaper_received_by,
                newspaper_post_date = domain.newspaper_post_date?.ToString(Constant.DateFormat),
                newspaper_post_by = domain.newspaper_post_by,

                created_by = domain.created_by,
                created_date = domain.created_date.ToString(Constant.DateFormat),
                updated_by = domain.updated_by,
                updated_date = domain.updated_date.ToString(Constant.DateFormat)
            };
        }

        #endregion
    }
}
