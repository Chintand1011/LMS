namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentRoutes_sequence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentRoutes", "sequence", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentRoutes", "sequence");
        }
    }
}
