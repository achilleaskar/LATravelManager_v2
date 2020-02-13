namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _10222 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExcursionTimes", "From_Id", c => c.Int());
            AddColumn("dbo.ExcursionTimes", "StartingPlace_Id1", c => c.Int());
            CreateIndex("dbo.ExcursionTimes", "From_Id");
            CreateIndex("dbo.ExcursionTimes", "StartingPlace_Id1");
            AddForeignKey("dbo.ExcursionTimes", "From_Id", "dbo.StartingPlaces", "Id");
            AddForeignKey("dbo.ExcursionTimes", "StartingPlace_Id1", "dbo.StartingPlaces", "Id");
        }

        public override void Down()
        {
            AddColumn("dbo.ExcursionTimes", "Excursion_Id", c => c.Int());
            DropForeignKey("dbo.ExcursionTimes", "StartingPlace_Id1", "dbo.StartingPlaces");
            DropForeignKey("dbo.ExcursionTimes", "From_Id", "dbo.StartingPlaces");
            DropIndex("dbo.ExcursionTimes", new[] { "StartingPlace_Id1" });
            DropIndex("dbo.ExcursionTimes", new[] { "From_Id" });
            DropColumn("dbo.ExcursionTimes", "StartingPlace_Id1");
            DropColumn("dbo.ExcursionTimes", "From_Id");
            CreateIndex("dbo.ExcursionTimes", "Excursion_Id");
            AddForeignKey("dbo.ExcursionTimes", "Excursion_Id", "dbo.Excursions", "Id");
        }
    }
}