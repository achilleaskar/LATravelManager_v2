namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _11131 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Temp", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bookings", "Valid");
        }

        public override void Down()
        {
            AddColumn("dbo.Bookings", "Valid", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bookings", "Temp");
        }
    }
}