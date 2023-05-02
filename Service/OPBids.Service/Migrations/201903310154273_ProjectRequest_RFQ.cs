namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectRequest_RFQ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectRequests", "rfq_deadline", c => c.DateTime());
            AddColumn("dbo.ProjectRequests", "rfq_place", c => c.String(maxLength: 200));
            AddColumn("dbo.ProjectRequests", "rfq_requestor", c => c.String(maxLength: 50));
            AddColumn("dbo.ProjectRequests", "rfq_requestor_dept", c => c.String(maxLength: 50));
            AddColumn("dbo.ProjectRequests", "rfq_request_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectRequests", "rfq_request_date");
            DropColumn("dbo.ProjectRequests", "rfq_requestor_dept");
            DropColumn("dbo.ProjectRequests", "rfq_requestor");
            DropColumn("dbo.ProjectRequests", "rfq_place");
            DropColumn("dbo.ProjectRequests", "rfq_deadline");
        }
    }
}
