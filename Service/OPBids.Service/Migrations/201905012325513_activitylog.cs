namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activitylog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RecordId = c.String(),
                        UserId = c.String(),
                        Module = c.String(),
                        IPAddress = c.String(),
                        UserName = c.String(),
                        FullName = c.String(),
                        Activities = c.String(),
                        Action = c.String(),
                        Type = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ActivityLogs");
        }
    }
}
