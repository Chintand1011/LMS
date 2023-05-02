namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parsefullnameofaccessuserstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessUsers", "first_name", c => c.String(nullable: false, maxLength: 150));
            AddColumn("dbo.AccessUsers", "mi", c => c.String());
            AddColumn("dbo.AccessUsers", "last_name", c => c.String());
            DropColumn("dbo.AccessUsers", "full_name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccessUsers", "full_name", c => c.String(nullable: false, maxLength: 150));
            DropColumn("dbo.AccessUsers", "last_name");
            DropColumn("dbo.AccessUsers", "mi");
            DropColumn("dbo.AccessUsers", "first_name");
        }
    }
}
