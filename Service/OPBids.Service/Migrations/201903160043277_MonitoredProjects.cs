namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MonitoredProjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MonitoredProjects",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        project_request_id = c.Int(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MonitoredProjects");
        }
    }
}
