namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class senderrecipient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SenderRecipientUsers", "FirstName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.SenderRecipientUsers", "MI", c => c.String(maxLength: 2));
            AddColumn("dbo.SenderRecipientUsers", "LastName", c => c.String(nullable: false, maxLength: 120));
            DropColumn("dbo.SenderRecipientUsers", "full_name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SenderRecipientUsers", "full_name", c => c.String(nullable: false, maxLength: 150));
            DropColumn("dbo.SenderRecipientUsers", "LastName");
            DropColumn("dbo.SenderRecipientUsers", "MI");
            DropColumn("dbo.SenderRecipientUsers", "FirstName");
        }
    }
}
