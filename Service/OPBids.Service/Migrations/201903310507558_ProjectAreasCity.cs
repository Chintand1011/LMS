namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectAreasCity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectAreasCities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        city_name = c.String(nullable: false, maxLength: 50),
                        status = c.String(maxLength: 1),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProjectAreasCities");
        }
    }
}
