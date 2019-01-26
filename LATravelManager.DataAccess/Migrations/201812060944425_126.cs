namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _126 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "OnlyStay", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "Transfer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Transfer");
            DropColumn("dbo.Bookings", "OnlyStay");
        }
    }
}
