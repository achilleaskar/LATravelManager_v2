namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1262 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "DifferentDates", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "DifferentDates");
        }
    }
}
