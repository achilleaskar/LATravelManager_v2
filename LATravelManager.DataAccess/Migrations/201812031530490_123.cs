namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _123 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservations", "Booking_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Bookings", "Excursion_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Excursions", "Name", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.Excursions", "ExcursionType_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.ExcursionDates", "Name", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.ExcursionCategories", "Name", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.Partners", "Name", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Payments", "Comment", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Payments", "User_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "BaseLocation_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.StartingPlaces", "Name", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Customers", "StartingPlace", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.HotelCategories", "Name", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AlterColumn("dbo.RoomTypes", "Name", c => c.String(maxLength: 20, unicode: false));
            AddForeignKey("dbo.Reservations", "Booking_Id", "dbo.Bookings", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Bookings", "Excursion_Id", "dbo.Excursions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Excursions", "ExcursionType_Id", "dbo.ExcursionCategories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Payments", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Users", "BaseLocation_Id", "dbo.StartingPlaces", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "BaseLocation_Id", "dbo.StartingPlaces");
            DropForeignKey("dbo.Payments", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Excursions", "ExcursionType_Id", "dbo.ExcursionCategories");
            DropForeignKey("dbo.Bookings", "Excursion_Id", "dbo.Excursions");
            DropForeignKey("dbo.Reservations", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.Users", new[] { "BaseLocation_Id" });
            DropIndex("dbo.Payments", new[] { "User_Id" });
            DropIndex("dbo.Excursions", new[] { "ExcursionType_Id" });
            DropIndex("dbo.Bookings", new[] { "Excursion_Id" });
            DropIndex("dbo.Reservations", new[] { "Booking_Id" });
            AlterColumn("dbo.RoomTypes", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.HotelCategories", "Name", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.Customers", "StartingPlace", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.StartingPlaces", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Users", "BaseLocation_Id", c => c.Int());
            AlterColumn("dbo.Payments", "User_Id", c => c.Int());
            AlterColumn("dbo.Payments", "Comment", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Partners", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.ExcursionCategories", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.ExcursionDates", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Excursions", "ExcursionType_Id", c => c.Int());
            AlterColumn("dbo.Excursions", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Bookings", "Excursion_Id", c => c.Int());
            AlterColumn("dbo.Reservations", "Booking_Id", c => c.Int());
            DropColumn("dbo.BookingInfoPerDays", "MinimunStay");
            CreateIndex("dbo.Users", "BaseLocation_Id");
            CreateIndex("dbo.Payments", "User_Id");
            CreateIndex("dbo.Excursions", "ExcursionType_Id");
            CreateIndex("dbo.Bookings", "Excursion_Id");
            CreateIndex("dbo.Reservations", "Booking_Id");
            AddForeignKey("dbo.Users", "BaseLocation_Id", "dbo.StartingPlaces", "Id");
            AddForeignKey("dbo.Payments", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Excursions", "ExcursionType_Id", "dbo.ExcursionCategories", "Id");
            AddForeignKey("dbo.Bookings", "Excursion_Id", "dbo.Excursions", "Id");
            AddForeignKey("dbo.Reservations", "Booking_Id", "dbo.Bookings", "Id");
        }
    }
}
