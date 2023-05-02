namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectRequest_StartDate_CompletedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectRequests", "start_date", c => c.DateTime());
            AddColumn("dbo.ProjectRequests", "completed_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectRequests", "completed_date");
            DropColumn("dbo.ProjectRequests", "start_date");
        }
    }
}
