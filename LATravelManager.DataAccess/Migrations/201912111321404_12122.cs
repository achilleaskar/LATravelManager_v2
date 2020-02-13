namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _12122 : DbMigration
    {
        public override void Up()
        {
        }

        public override void Down()
        {
            DropPrimaryKey("dbo.ExcursionCities");
            AddPrimaryKey("dbo.ExcursionCities", new[] { "City_Id", "Excursion_Id" });
            RenameTable(name: "dbo.ExcursionCities", newName: "CityExcursions");
        }
    }
}