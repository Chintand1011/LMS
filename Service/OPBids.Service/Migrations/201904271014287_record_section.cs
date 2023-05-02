namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class record_section : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessGroupTypes", "record_section", c => c.Boolean());
            AddColumn("dbo.AccessTypes", "record_section", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccessTypes", "record_section");
            DropColumn("dbo.AccessGroupTypes", "record_section");
        }
    }
}
