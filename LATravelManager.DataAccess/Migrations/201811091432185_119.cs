namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _119 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Commision", c => c.Single(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Bookings", "Commision");
        }
    }
}