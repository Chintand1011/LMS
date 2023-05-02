namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectRequestAdvertisement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectRequestAdvertisements",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        project_request_id = c.Int(nullable: false),
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
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProjectRequestAdvertisements");
        }
    }
}
