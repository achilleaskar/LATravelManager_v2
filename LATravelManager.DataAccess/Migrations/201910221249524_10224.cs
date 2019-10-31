namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10224 : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ExcursionTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.Time(nullable: false, precision: 0),
                        From_Id = c.Int(),
                        StartingPlace_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ExcursionTimes", "StartingPlace_Id");
            CreateIndex("dbo.ExcursionTimes", "From_Id");
            AddForeignKey("dbo.ExcursionTimes", "StartingPlace_Id", "dbo.StartingPlaces", "Id");
            AddForeignKey("dbo.ExcursionTimes", "From_Id", "dbo.Cities", "Id");
        }
    }
}
