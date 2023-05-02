namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_ProjectRequestBatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectRequestBatches", "philgeps_attachment", c => c.String(maxLength: 150));
            AddColumn("dbo.ProjectRequestBatches", "mmda_portal_att", c => c.String(maxLength: 150));
            AddColumn("dbo.ProjectRequestBatches", "conspost_lobby_att", c => c.String(maxLength: 150));
            AddColumn("dbo.ProjectRequestBatches", "conspost_reception_att", c => c.String(maxLength: 150));
            AddColumn("dbo.ProjectRequestBatches", "conspost_command_att", c => c.String(maxLength: 150));
            AddColumn("dbo.ProjectRequestBatches", "newspaper_att", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectRequestBatches", "newspaper_att");
            DropColumn("dbo.ProjectRequestBatches", "conspost_command_att");
            DropColumn("dbo.ProjectRequestBatches", "conspost_reception_att");
            DropColumn("dbo.ProjectRequestBatches", "conspost_lobby_att");
            DropColumn("dbo.ProjectRequestBatches", "mmda_portal_att");
            DropColumn("dbo.ProjectRequestBatches", "philgeps_attachment");
        }
    }
}
