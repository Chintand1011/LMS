namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProjectAreas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectAreas", "city_id", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectAreas", "district_id", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectAreas", "barangay_id", c => c.Int(nullable: false));
            DropColumn("dbo.ProjectAreas", "city");
            DropColumn("dbo.ProjectAreas", "district");
            DropColumn("dbo.ProjectAreas", "barangay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectAreas", "barangay", c => c.String(maxLength: 50));
            AddColumn("dbo.ProjectAreas", "district", c => c.String(maxLength: 50));
            AddColumn("dbo.ProjectAreas", "city", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.ProjectAreas", "barangay_id");
            DropColumn("dbo.ProjectAreas", "district_id");
            DropColumn("dbo.ProjectAreas", "city_id");
        }
    }
}
