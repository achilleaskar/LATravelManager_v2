namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1114 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bookings", "Temp");
        }

        public override void Down()
        {
            AddColumn("dbo.Bookings", "Temp", c => c.Boolean(nullable: false));
        }
    }
}