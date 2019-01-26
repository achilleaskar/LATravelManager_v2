namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1263 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "OnlyStay", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reservations", "Transfer", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bookings", "OnlyStay");
            DropColumn("dbo.Bookings", "Transfer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "Transfer", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "OnlyStay", c => c.Boolean(nullable: false));
            DropColumn("dbo.Reservations", "Transfer");
            DropColumn("dbo.Reservations", "OnlyStay");
        }
    }
}
