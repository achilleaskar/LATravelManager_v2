namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1222 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bookings", "Remaining");
        }

        public override void Down()
        {
            AddColumn("dbo.Bookings", "Remaining", c => c.Single(nullable: false));
        }
    }
}