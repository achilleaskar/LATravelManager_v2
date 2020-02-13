namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _10223 : DbMigration
    {
        public override void Up()
        {
        }

        public override void Down()
        {
            AddColumn("dbo.ExcursionTimes", "StartingPlace_Id1", c => c.Int());
            CreateIndex("dbo.ExcursionTimes", "StartingPlace_Id1");
            AddForeignKey("dbo.ExcursionTimes", "StartingPlace_Id1", "dbo.StartingPlaces", "Id");
        }
    }
}