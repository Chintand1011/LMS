namespace OPBids.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectRequestItemsCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectRequestItems",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        project_id = c.Int(nullable: false),
                        description = c.String(maxLength: 1000),
                        unit = c.String(maxLength: 100),
                        quantity = c.Int(nullable: false),
                        unit_cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        created_by = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        updated_by = c.Int(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProjectRequestItems");
        }
    }
}
