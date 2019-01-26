namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1113 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Valid", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Bookings", "Valid");
        }
    }
}