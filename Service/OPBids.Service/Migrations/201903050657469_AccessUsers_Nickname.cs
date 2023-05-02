namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessUsers_Nickname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessUsers", "nickname", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccessUsers", "nickname");
        }
    }
}
