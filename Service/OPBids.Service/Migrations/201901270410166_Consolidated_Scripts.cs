namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Consolidated_Scripts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessGroups",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        group_code = c.String(nullable: false, maxLength: 30),
                        group_description = c.String(nullable: false, maxLength: 100),
                        status = c.String(nullable: false, maxLength: 1),
                        dashboard_id = c.Int(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.group_code, unique: true);
            
            CreateTable(
                "dbo.AccessGroupTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        access_type_id = c.Int(nullable: false),
                        access_group_id = c.Int(nullable: false),
                        view_transact_data = c.Boolean(),
                        add_edit_data = c.Boolean(),
                        delete_data = c.Boolean(),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AccessTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(maxLength: 25),
                        name = c.String(nullable: false, maxLength: 100),
                        description = c.String(nullable: false),
                        view_transact_data = c.Boolean(),
                        add_edit_data = c.Boolean(),
                        delete_data = c.Boolean(),
                        seq_no = c.Int(nullable: false),
                        sys_id = c.Int(nullable: false),
                        parent_id = c.Int(),
                        status = c.String(maxLength: 1),
                        disp_menu_to_mobile = c.Boolean(nullable: false),
                        controller = c.String(maxLength: 100),
                        css_class = c.String(maxLength: 100),
                        icon = c.String(maxLength: 50),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AccessUsers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        salutation = c.String(maxLength: 10),
                        username = c.String(nullable: false, maxLength: 75),
                        email_address = c.String(nullable: false, maxLength: 120),
                        password = c.String(maxLength: 30),
                        activation_code = c.String(maxLength: 50),
                        full_name = c.String(nullable: false, maxLength: 150),
                        vip_access = c.Boolean(nullable: false),
                        pfms_access = c.Boolean(nullable: false),
                        dts_access = c.Boolean(nullable: false),
                        status = c.String(nullable: false, maxLength: 1),
                        group_id = c.Int(nullable: false),
                        dept_id = c.Int(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.email_address, unique: true);
            
            CreateTable(
                "dbo.ArchivedDocuments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        batch_id = c.Int(nullable: false),
                        category_id = c.Int(nullable: false),
                        document_type_id = c.Int(nullable: false),
                        document_code = c.String(maxLength: 20),
                        sender_id = c.Int(nullable: false),
                        receipient_id = c.Int(nullable: false),
                        etd_to_recipient = c.DateTime(nullable: false),
                        delivery_type_id = c.Int(nullable: false),
                        document_security_level_id = c.Int(nullable: false),
                        status = c.String(maxLength: 1),
                        is_disposable = c.Boolean(nullable: false),
                        years_retention = c.Int(nullable: false),
                        is_edoc = c.Boolean(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.BarcodePrintingStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 75),
                        description = c.String(nullable: false, maxLength: 150),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.code, unique: true);
            
            CreateTable(
                "dbo.BarcodeSettings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        barcode_only = c.Boolean(nullable: false),
                        barcode_with_print_date = c.Boolean(nullable: false),
                        qr_only = c.Boolean(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        cat_code = c.String(maxLength: 10),
                        cat_name = c.String(maxLength: 100),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.cat_code, unique: true);
            
            CreateTable(
                "dbo.ContractTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        contract_type = c.String(maxLength: 10),
                        contract_desc = c.String(maxLength: 100),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.DashboardConfigs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        dashboard_id = c.Int(nullable: false),
                        dashboard_desc = c.String(maxLength: 100),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.dashboard_id, unique: true);
            
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        delivery_code = c.String(nullable: false, maxLength: 75),
                        delivery_description = c.String(nullable: false, maxLength: 150),
                        status = c.String(nullable: false, maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.delivery_code, unique: true);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        parent_dept_id = c.Int(nullable: false),
                        dept_code = c.String(nullable: false, maxLength: 120),
                        dept_description = c.String(nullable: false, maxLength: 250),
                        headed_by = c.Int(nullable: false),
                        designation = c.String(nullable: false, maxLength: 100),
                        status = c.String(maxLength: 1),
                        is_internal = c.Boolean(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.dept_code, unique: true);
            
            CreateTable(
                "dbo.DocumentAttachments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        barcode_no = c.String(maxLength: 20),
                        batch_id = c.Int(nullable: false),
                        attachment_name = c.String(maxLength: 150),
                        file_name = c.String(maxLength: 150),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.DocumentCategories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        document_category_code = c.String(nullable: false, maxLength: 75),
                        document_category_name = c.String(nullable: false, maxLength: 150),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.document_category_code, unique: true);
            
            CreateTable(
                "dbo.DocumentLogs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        batch_id = c.Int(nullable: false),
                        log_date = c.DateTime(nullable: false),
                        receipient_id = c.Int(nullable: false),
                        remarks = c.String(maxLength: 1000),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.DocumentRoutes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        batch_id = c.Int(nullable: false),
                        department_id = c.Int(nullable: false),
                        receipient_id = c.Int(nullable: false),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.DocumentSecurityLevels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 75),
                        description = c.String(nullable: false, maxLength: 150),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.code, unique: true);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        document_category_id = c.Int(nullable: false),
                        document_type_code = c.String(nullable: false, maxLength: 75),
                        document_type_description = c.String(nullable: false, maxLength: 150),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.document_type_code, unique: true);
            
            CreateTable(
                "dbo.FinalizedDocuments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        batch_id = c.Int(nullable: false),
                        category_id = c.Int(nullable: false),
                        document_type_id = c.Int(nullable: false),
                        document_code = c.String(maxLength: 20),
                        sender_id = c.Int(nullable: false),
                        receipient_id = c.Int(nullable: false),
                        etd_to_recipient = c.DateTime(nullable: false),
                        delivery_type_id = c.Int(nullable: false),
                        document_security_level_id = c.Int(nullable: false),
                        status = c.String(maxLength: 1),
                        is_edoc = c.Boolean(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.OnHandDocuments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        category_id = c.Int(nullable: false),
                        document_type_id = c.Int(nullable: false),
                        document_code = c.String(maxLength: 20),
                        sender_id = c.Int(nullable: false),
                        receipient_id = c.Int(nullable: false),
                        etd_to_recipient = c.DateTime(nullable: false),
                        delivery_type_id = c.Int(nullable: false),
                        document_security_level_id = c.Int(nullable: false),
                        status = c.String(maxLength: 1),
                        is_edoc = c.Boolean(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProcurementMethods",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proc_code = c.String(nullable: false, maxLength: 15),
                        procurement_description = c.String(nullable: false, maxLength: 100),
                        status = c.String(nullable: false, maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.proc_code, unique: true);
            
            CreateTable(
                "dbo.ProcurementTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proc_type = c.String(maxLength: 10),
                        proc_typedesc = c.String(maxLength: 100),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.proc_type, unique: true);
            
            CreateTable(
                "dbo.ProjectBids",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        project_request_id = c.Int(nullable: false),
                        bid_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bid_bond = c.Int(nullable: false),
                        bid_form = c.String(maxLength: 50),
                        duration = c.Int(nullable: false),
                        variance = c.Int(nullable: false),
                        bid_status = c.String(maxLength: 2),
                        record_status = c.String(maxLength: 1),
                        prepared_by = c.String(maxLength: 200),
                        prepared_date = c.DateTime(),
                        procured_docs = c.Boolean(nullable: false),
                        eval_result = c.String(maxLength: 50),
                        gen_eval = c.String(maxLength: 50),
                        notes = c.String(maxLength: 1000),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectBidChecklists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        project_request_id = c.Int(nullable: false),
                        bid_id = c.Int(nullable: false),
                        stage = c.String(maxLength: 1),
                        checklist_id = c.String(maxLength: 10),
                        type = c.String(maxLength: 5),
                        notes = c.String(maxLength: 500),
                        eligibility = c.Boolean(),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectCategories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proj_cat = c.String(nullable: false, maxLength: 100),
                        proj_desc = c.String(nullable: false, maxLength: 225),
                        status = c.String(nullable: false, maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectClassifications",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        classification = c.String(nullable: false, maxLength: 20),
                        description = c.String(nullable: false, maxLength: 200),
                        status = c.String(nullable: false, maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.classification, unique: true);
            
            CreateTable(
                "dbo.ProjectDocuments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        supplier_id = c.Int(nullable: false),
                        project_id = c.Int(nullable: false),
                        name = c.String(maxLength: 255),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectGrantees",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        grantee_code = c.String(nullable: false, maxLength: 100),
                        grantee_name = c.String(nullable: false, maxLength: 225),
                        status = c.String(nullable: false, maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectProponents",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proponent_name = c.String(nullable: false, maxLength: 250),
                        proponent_designation = c.String(nullable: false, maxLength: 75),
                        dept_id = c.Int(nullable: false),
                        department = c.String(),
                        proponent_emailadd = c.String(nullable: false, maxLength: 75),
                        proponent_contactno = c.String(nullable: false, maxLength: 75),
                        status = c.String(nullable: false, maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectRequestAttachments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        project_id = c.Int(nullable: false),
                        file_name = c.String(),
                        attachment_name = c.String(),
                        barcode_no = c.String(),
                        batch_id = c.Int(nullable: false),
                        status = c.String(),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectRequestBatches",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        applicable_year = c.Int(nullable: false),
                        procurement_method = c.String(maxLength: 20),
                        pre_bid_date = c.DateTime(),
                        pre_bid_place = c.String(maxLength: 200),
                        bid_deadline_date = c.DateTime(),
                        bid_deadline_place = c.String(maxLength: 200),
                        bid_opening_date = c.DateTime(),
                        bid_opening_place = c.String(maxLength: 200),
                        bid_notes = c.String(maxLength: 1000),
                        philgeps_publish_date = c.DateTime(),
                        philgeps_publish_by = c.String(maxLength: 100),
                        mmda_publish_date = c.DateTime(),
                        mmda_publish_by = c.String(maxLength: 100),
                        conspost_date_lobby = c.DateTime(),
                        conspost_date_reception = c.DateTime(),
                        conspost_date_command = c.DateTime(),
                        conspost_by = c.String(maxLength: 100),
                        newspaper_sent_date = c.DateTime(),
                        newspaper_publisher = c.String(maxLength: 200),
                        newspaper_received_by = c.String(maxLength: 200),
                        newspaper_post_date = c.DateTime(),
                        newspaper_post_by = c.String(maxLength: 100),
                        project_status = c.String(maxLength: 10),
                        project_substatus = c.String(maxLength: 10),
                        record_status = c.String(maxLength: 1),
                        sla = c.Int(nullable: false),
                        current_user = c.Int(nullable: false),
                        user_action = c.String(maxLength: 200),
                        notes = c.String(maxLength: 1000),
                        routed_date = c.DateTime(),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectRequestBatchHistories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        batch_id = c.Int(nullable: false),
                        applicable_year = c.Int(nullable: false),
                        procurement_method = c.String(maxLength: 20),
                        pre_bid_date = c.DateTime(),
                        pre_bid_place = c.String(maxLength: 200),
                        bid_deadline_date = c.DateTime(),
                        bid_deadline_place = c.String(maxLength: 200),
                        bid_opening_date = c.DateTime(),
                        bid_opening_place = c.String(maxLength: 200),
                        bid_notes = c.String(maxLength: 1000),
                        philgeps_publish_date = c.DateTime(),
                        philgeps_publish_by = c.String(maxLength: 100),
                        mmda_publish_date = c.DateTime(),
                        mmda_publish_by = c.String(maxLength: 100),
                        conspost_date_lobby = c.DateTime(),
                        conspost_date_reception = c.DateTime(),
                        conspost_date_command = c.DateTime(),
                        conspost_by = c.String(maxLength: 100),
                        newspaper_sent_date = c.DateTime(),
                        newspaper_publisher = c.String(maxLength: 200),
                        newspaper_received_by = c.String(maxLength: 200),
                        newspaper_post_date = c.DateTime(),
                        newspaper_post_by = c.String(maxLength: 100),
                        project_status = c.String(maxLength: 10),
                        project_substatus = c.String(maxLength: 10),
                        record_status = c.String(maxLength: 1),
                        sla = c.Int(nullable: false),
                        current_user = c.Int(nullable: false),
                        user_action = c.String(maxLength: 200),
                        notes = c.String(maxLength: 1000),
                        routed_date = c.DateTime(),
                        change_log = c.String(maxLength: 500),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectRequests",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        title = c.String(maxLength: 100),
                        description = c.String(maxLength: 1000),
                        grantee = c.String(maxLength: 100),
                        estimated_budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        approved_budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        required_date = c.DateTime(nullable: false),
                        category = c.String(maxLength: 5),
                        classification = c.String(maxLength: 5),
                        contract_type = c.String(maxLength: 5),
                        security_level = c.Int(nullable: false),
                        delivery_type = c.Int(nullable: false),
                        earmark = c.String(maxLength: 20),
                        earmark_date = c.DateTime(),
                        source_fund = c.String(maxLength: 5),
                        batch_id = c.Int(nullable: false),
                        project_status = c.String(maxLength: 10),
                        project_substatus = c.String(maxLength: 10),
                        record_status = c.String(maxLength: 1),
                        sla = c.Int(nullable: false),
                        current_user = c.Int(nullable: false),
                        user_action = c.String(maxLength: 200),
                        notes = c.String(maxLength: 1000),
                        routed_date = c.DateTime(),
                        project_duration = c.Int(nullable: false),
                        bid_bond = c.Int(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectRequestHistories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        project_request_id = c.Int(nullable: false),
                        title = c.String(maxLength: 100),
                        description = c.String(maxLength: 1000),
                        grantee = c.String(maxLength: 100),
                        estimated_budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        approved_budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        required_date = c.DateTime(nullable: false),
                        category = c.String(maxLength: 5),
                        classification = c.String(maxLength: 5),
                        contract_type = c.String(maxLength: 5),
                        security_level = c.Int(nullable: false),
                        devivery_type = c.Int(nullable: false),
                        earmark = c.String(maxLength: 20),
                        earmark_date = c.DateTime(),
                        source_fund = c.String(maxLength: 5),
                        batch_id = c.Int(nullable: false),
                        project_status = c.String(maxLength: 10),
                        project_substatus = c.String(maxLength: 10),
                        record_status = c.String(maxLength: 1),
                        sla = c.Int(nullable: false),
                        current_user = c.Int(nullable: false),
                        user_action = c.String(),
                        notes = c.String(maxLength: 1000),
                        routed_date = c.DateTime(),
                        change_log = c.String(maxLength: 500),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proj_statcode = c.String(nullable: false, maxLength: 75),
                        proj_statdescription = c.String(nullable: false, maxLength: 250),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectSubCategories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proj_catid = c.Int(nullable: false),
                        proj_subcat = c.String(nullable: false, maxLength: 100),
                        proj_subcatdesc = c.String(nullable: false, maxLength: 225),
                        status = c.String(nullable: false, maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectSubStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proj_statid = c.Int(nullable: false),
                        proj_substatcode = c.String(nullable: false, maxLength: 75),
                        proj_substatdescription = c.String(nullable: false, maxLength: 250),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ReceivedDocuments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        batch_id = c.Int(nullable: false),
                        category_id = c.Int(nullable: false),
                        document_type_id = c.Int(nullable: false),
                        document_code = c.String(maxLength: 20),
                        sender_id = c.Int(nullable: false),
                        receipient_id = c.Int(nullable: false),
                        etd_to_recipient = c.DateTime(nullable: false),
                        delivery_type_id = c.Int(nullable: false),
                        document_security_level_id = c.Int(nullable: false),
                        status = c.String(maxLength: 1),
                        is_edoc = c.Boolean(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.RequestBarcodes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        requested_quantity = c.Int(nullable: false),
                        printed_quantity = c.Int(nullable: false),
                        status = c.String(maxLength: 1),
                        remarks = c.String(maxLength: 1000),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.SenderRecipients",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        is_system_user = c.Boolean(nullable: false),
                        status = c.String(maxLength: 1),
                        department_id = c.Int(nullable: false),
                        is_sender = c.Boolean(nullable: false),
                        is_recipient = c.Boolean(nullable: false),
                        mobile_no = c.String(maxLength: 11),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.SenderRecipientUsers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        salutation = c.String(maxLength: 10),
                        email_address = c.String(maxLength: 120),
                        full_name = c.String(nullable: false, maxLength: 150),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.email_address, unique: true);
            
            CreateTable(
                "dbo.SourceFunds",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        source_code = c.String(nullable: false, maxLength: 25),
                        source_description = c.String(nullable: false, maxLength: 100),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        contact_person = c.String(nullable: false, maxLength: 100),
                        company_code = c.String(nullable: false, maxLength: 25),
                        comp_name = c.String(nullable: false, maxLength: 100),
                        address = c.String(nullable: false, maxLength: 150),
                        email = c.String(nullable: false, maxLength: 50),
                        contact_no = c.String(nullable: false, maxLength: 25),
                        tin = c.String(nullable: false, maxLength: 50),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.UserAnnouncements",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        message = c.String(),
                        is_read = c.Boolean(nullable: false),
                        is_starred = c.Boolean(nullable: false),
                        is_hidden = c.Boolean(nullable: false),
                        recipient_ids = c.String(),
                        sender_id = c.Int(nullable: false),
                        date_sent = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        message = c.String(),
                        is_read = c.Boolean(nullable: false),
                        is_starred = c.Boolean(nullable: false),
                        is_hidden = c.Boolean(nullable: false),
                        recipient_ids = c.String(),
                        sender_id = c.Int(nullable: false),
                        date_sent = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Workflows",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        type = c.String(nullable: false, maxLength: 5),
                        seq_no = c.Int(nullable: false),
                        project_status = c.String(nullable: false, maxLength: 10),
                        project_substatus = c.String(nullable: false, maxLength: 10),
                        project_status_desc = c.String(nullable: false, maxLength: 500),
                        project_substatus_desc = c.String(nullable: false, maxLength: 500),
                        sla = c.Int(nullable: false),
                        access_group = c.String(nullable: false, maxLength: 10),
                        record_status = c.String(nullable: false, maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SenderRecipientUsers", new[] { "email_address" });
            DropIndex("dbo.ProjectClassifications", new[] { "classification" });
            DropIndex("dbo.ProcurementTypes", new[] { "proc_type" });
            DropIndex("dbo.ProcurementMethods", new[] { "proc_code" });
            DropIndex("dbo.DocumentTypes", new[] { "document_type_code" });
            DropIndex("dbo.DocumentSecurityLevels", new[] { "code" });
            DropIndex("dbo.DocumentCategories", new[] { "document_category_code" });
            DropIndex("dbo.Departments", new[] { "dept_code" });
            DropIndex("dbo.Deliveries", new[] { "delivery_code" });
            DropIndex("dbo.DashboardConfigs", new[] { "dashboard_id" });
            DropIndex("dbo.Categories", new[] { "cat_code" });
            DropIndex("dbo.BarcodePrintingStatus", new[] { "code" });
            DropIndex("dbo.AccessUsers", new[] { "email_address" });
            DropIndex("dbo.AccessGroups", new[] { "group_code" });
            DropTable("dbo.Workflows");
            DropTable("dbo.UserNotifications");
            DropTable("dbo.UserAnnouncements");
            DropTable("dbo.Suppliers");
            DropTable("dbo.SourceFunds");
            DropTable("dbo.SenderRecipientUsers");
            DropTable("dbo.SenderRecipients");
            DropTable("dbo.RequestBarcodes");
            DropTable("dbo.ReceivedDocuments");
            DropTable("dbo.ProjectSubStatus");
            DropTable("dbo.ProjectSubCategories");
            DropTable("dbo.ProjectStatus");
            DropTable("dbo.ProjectRequestHistories");
            DropTable("dbo.ProjectRequests");
            DropTable("dbo.ProjectRequestBatchHistories");
            DropTable("dbo.ProjectRequestBatches");
            DropTable("dbo.ProjectRequestAttachments");
            DropTable("dbo.ProjectProponents");
            DropTable("dbo.ProjectGrantees");
            DropTable("dbo.ProjectDocuments");
            DropTable("dbo.ProjectClassifications");
            DropTable("dbo.ProjectCategories");
            DropTable("dbo.ProjectBidChecklists");
            DropTable("dbo.ProjectBids");
            DropTable("dbo.ProcurementTypes");
            DropTable("dbo.ProcurementMethods");
            DropTable("dbo.OnHandDocuments");
            DropTable("dbo.FinalizedDocuments");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.DocumentSecurityLevels");
            DropTable("dbo.DocumentRoutes");
            DropTable("dbo.DocumentLogs");
            DropTable("dbo.DocumentCategories");
            DropTable("dbo.DocumentAttachments");
            DropTable("dbo.Departments");
            DropTable("dbo.Deliveries");
            DropTable("dbo.DashboardConfigs");
            DropTable("dbo.ContractTypes");
            DropTable("dbo.Categories");
            DropTable("dbo.BarcodeSettings");
            DropTable("dbo.BarcodePrintingStatus");
            DropTable("dbo.ArchivedDocuments");
            DropTable("dbo.AccessUsers");
            DropTable("dbo.AccessTypes");
            DropTable("dbo.AccessGroupTypes");
            DropTable("dbo.AccessGroups");
        }
    }
}
