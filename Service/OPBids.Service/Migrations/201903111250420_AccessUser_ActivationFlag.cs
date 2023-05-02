namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessUser_ActivationFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessUsers", "activation_flag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccessUsers", "activation_flag");
        }
    }
}
