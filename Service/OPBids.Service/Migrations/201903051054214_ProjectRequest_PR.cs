namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectRequest_PR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectRequests", "pr_number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectRequests", "pr_number");
        }
    }
}
