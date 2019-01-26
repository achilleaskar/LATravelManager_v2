namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _116 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Checkout", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Bookings", "CheckIn", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Reservations", "ReservationType", c => c.Int(nullable: false));
            AddColumn("dbo.Reservations", "NoNameRoomType_Id", c => c.Int());
            AddColumn("dbo.Rooms", "IsAllotment", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Customers", "CheckIn", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Customers", "CheckOut", c => c.DateTime(precision: 0));
            CreateIndex("dbo.Reservations", "NoNameRoomType_Id");
            AddForeignKey("dbo.Reservations", "NoNameRoomType_Id", "dbo.RoomTypes", "Id");
            DropColumn("dbo.BookingInfoPerDays", "IsAllotement");
        }

        public override void Down()
        {
            AddColumn("dbo.BookingInfoPerDays", "IsAllotement", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Reservations", "NoNameRoomType_Id", "dbo.RoomTypes");
            DropIndex("dbo.Reservations", new[] { "NoNameRoomType_Id" });
            AlterColumn("dbo.Customers", "CheckOut", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Customers", "CheckIn", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.Rooms", "IsAllotment");
            DropColumn("dbo.Reservations", "NoNameRoomType_Id");
            DropColumn("dbo.Reservations", "ReservationType");
            DropColumn("dbo.Bookings", "CheckIn");
            DropColumn("dbo.Bookings", "Checkout");
        }
    }
}