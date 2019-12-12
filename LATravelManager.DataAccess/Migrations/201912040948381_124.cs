namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _124 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "GroupBooking", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "VoucherSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "RoomingListIncluded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "RoomingListIncluded");
            DropColumn("dbo.Bookings", "VoucherSent");
            DropColumn("dbo.Bookings", "GroupBooking");
        }
    }
}
