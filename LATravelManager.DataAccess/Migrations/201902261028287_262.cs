namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _262 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CityExcursions",
                c => new
                    {
                        City_Id = c.Int(nullable: false),
                        Excursion_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.City_Id, t.Excursion_Id })
                .ForeignKey("dbo.Cities", t => t.City_Id, cascadeDelete: true)
                .ForeignKey("dbo.Excursions", t => t.Excursion_Id, cascadeDelete: true)
                .Index(t => t.City_Id)
                .Index(t => t.Excursion_Id);
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cities", "Excursion_Id", c => c.Int());
            DropForeignKey("dbo.CityExcursions", "Excursion_Id", "dbo.Excursions");
            DropForeignKey("dbo.CityExcursions", "City_Id", "dbo.Cities");
            DropIndex("dbo.CityExcursions", new[] { "Excursion_Id" });
            DropIndex("dbo.CityExcursions", new[] { "City_Id" });
            DropTable("dbo.CityExcursions");
            CreateIndex("dbo.Cities", "Excursion_Id");
            AddForeignKey("dbo.Cities", "Excursion_Id", "dbo.Excursions", "Id");
        }
    }
}
