namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10225 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExcursionTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.Time(nullable: false, precision: 0 ),
                        From_Id = c.Int(),
                        StartingPlace_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.From_Id)
                .ForeignKey("dbo.StartingPlaces", t => t.StartingPlace_Id)
                .Index(t => t.From_Id)
                .Index(t => t.StartingPlace_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExcursionTimes", "StartingPlace_Id", "dbo.StartingPlaces");
            DropForeignKey("dbo.ExcursionTimes", "From_Id", "dbo.Cities");
            DropIndex("dbo.ExcursionTimes", new[] { "StartingPlace_Id" });
            DropIndex("dbo.ExcursionTimes", new[] { "From_Id" });
            DropTable("dbo.ExcursionTimes");
        }
    }
}
