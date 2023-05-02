namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class access_users_profile_link : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessUsers", "business_email_address", c => c.String(maxLength: 120));
            AddColumn("dbo.AccessUsers", "mobile_no", c => c.String(maxLength: 50));
            AddColumn("dbo.AccessUsers", "contact_no", c => c.String(maxLength: 50));
            AddColumn("dbo.AccessUsers", "profile_link", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccessUsers", "profile_link");
            DropColumn("dbo.AccessUsers", "contact_no");
            DropColumn("dbo.AccessUsers", "mobile_no");
            DropColumn("dbo.AccessUsers", "business_email_address");
        }
    }
}
