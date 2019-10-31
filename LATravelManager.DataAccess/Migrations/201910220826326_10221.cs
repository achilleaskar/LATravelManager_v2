namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10221 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExcursionTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.Time(nullable: false, precision: 0),
                        StartingPlace_Id = c.Int(),
                        Excursion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StartingPlaces", t => t.StartingPlace_Id)
                .ForeignKey("dbo.Excursions", t => t.Excursion_Id)
                .Index(t => t.StartingPlace_Id)
                .Index(t => t.Excursion_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExcursionTimes", "Excursion_Id", "dbo.Excursions");
            DropForeignKey("dbo.ExcursionTimes", "StartingPlace_Id", "dbo.StartingPlaces");
            DropIndex("dbo.ExcursionTimes", new[] { "Excursion_Id" });
            DropIndex("dbo.ExcursionTimes", new[] { "StartingPlace_Id" });
            DropTable("dbo.ExcursionTimes");
        }
    }
}
