namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1162 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingInfoPerDays", "IsAllotment", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.BookingInfoPerDays", "IsAllotment");
        }
    }
}