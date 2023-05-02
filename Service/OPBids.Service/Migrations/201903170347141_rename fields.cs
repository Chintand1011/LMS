namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SenderRecipientUsers", "first_name", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.SenderRecipientUsers", "last_name", c => c.String(nullable: false, maxLength: 120));
            DropColumn("dbo.SenderRecipientUsers", "FirstName");
            DropColumn("dbo.SenderRecipientUsers", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SenderRecipientUsers", "LastName", c => c.String(nullable: false, maxLength: 120));
            AddColumn("dbo.SenderRecipientUsers", "FirstName", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.SenderRecipientUsers", "last_name");
            DropColumn("dbo.SenderRecipientUsers", "first_name");
        }
    }
}
