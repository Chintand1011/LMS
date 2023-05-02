namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_current_receiver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentRoutes", "current_receiver", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentRoutes", "current_receiver");
        }
    }
}
