namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcurementMethod_ProducrementMode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProcurementMethods", "procurement_mode", c => c.String(nullable: false, maxLength: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProcurementMethods", "procurement_mode");
        }
    }
}
