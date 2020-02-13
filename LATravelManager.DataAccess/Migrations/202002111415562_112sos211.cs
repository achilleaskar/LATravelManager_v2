namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _112sos211 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bookings", "NetPrice");
        }

        public override void Down()
        {
            AddColumn("dbo.Bookings", "NetPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}