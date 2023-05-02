namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_ProjectRequestBatch2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectRequestBatches", "philgeps_att", c => c.String(maxLength: 150));
            DropColumn("dbo.ProjectRequestBatches", "philgeps_attachment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectRequestBatches", "philgeps_attachment", c => c.String(maxLength: 150));
            DropColumn("dbo.ProjectRequestBatches", "philgeps_att");
        }
    }
}
