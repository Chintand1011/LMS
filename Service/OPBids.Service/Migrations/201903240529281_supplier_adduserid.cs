namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supplier_adduserid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "user_id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suppliers", "user_id");
        }
    }
}
