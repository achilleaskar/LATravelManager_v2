namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class epitelousgamw : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Excursions", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Partners", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Users", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Reservations", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Customers", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.OptionalExcursions", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Hotels", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Rooms", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Options", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Buses", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.BookingInfoPerDays", "CreatedDate");
            DropColumn("dbo.Cities", "CreatedDate");
            DropColumn("dbo.Countries", "CreatedDate");
            DropColumn("dbo.ExcursionDates", "CreatedDate");
            DropColumn("dbo.ExcursionCategories", "CreatedDate");
            DropColumn("dbo.Payments", "CreatedDate");
            DropColumn("dbo.HotelCategories", "CreatedDate");
            DropColumn("dbo.RoomTypes", "CreatedDate");
            DropColumn("dbo.Leaders", "CreatedDate");
            DropColumn("dbo.StartingPlaces", "CreatedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StartingPlaces", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Leaders", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.RoomTypes", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.HotelCategories", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Payments", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.ExcursionCategories", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.ExcursionDates", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Countries", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Cities", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.BookingInfoPerDays", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Buses", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Options", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Rooms", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Hotels", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.OptionalExcursions", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Customers", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Reservations", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Users", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Partners", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Excursions", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Bookings", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.Buses", "ModifiedDate");
            DropColumn("dbo.Options", "ModifiedDate");
            DropColumn("dbo.Rooms", "ModifiedDate");
            DropColumn("dbo.Hotels", "ModifiedDate");
            DropColumn("dbo.OptionalExcursions", "ModifiedDate");
            DropColumn("dbo.Customers", "ModifiedDate");
            DropColumn("dbo.Reservations", "ModifiedDate");
            DropColumn("dbo.Users", "ModifiedDate");
            DropColumn("dbo.Partners", "ModifiedDate");
            DropColumn("dbo.Excursions", "ModifiedDate");
            DropColumn("dbo.Bookings", "ModifiedDate");
        }
    }
}
