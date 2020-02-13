namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _111 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExcursionDates", "NightStart", c => c.Boolean(nullable: false));
            DropColumn("dbo.Excursions", "NightStart");
            DropColumn("dbo.RoomTypes", "Index");
        }

        public override void Down()
        {
            AddColumn("dbo.RoomTypes", "Index", c => c.Int(nullable: false));
            AddColumn("dbo.Excursions", "NightStart", c => c.Boolean(nullable: false));
            DropColumn("dbo.ExcursionDates", "NightStart");
        }
    }
}