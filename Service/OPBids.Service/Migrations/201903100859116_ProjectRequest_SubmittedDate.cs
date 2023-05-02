namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectRequest_SubmittedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectRequests", "submitted_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectRequests", "submitted_date");
        }
    }
}
