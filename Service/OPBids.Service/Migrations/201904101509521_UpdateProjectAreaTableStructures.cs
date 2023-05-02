namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProjectAreaTableStructures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectAreasBarangays", "city_id", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectAreasBarangays", "district_id", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectAreasDistricts", "city_id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectAreasDistricts", "city_id");
            DropColumn("dbo.ProjectAreasBarangays", "district_id");
            DropColumn("dbo.ProjectAreasBarangays", "city_id");
        }
    }
}
