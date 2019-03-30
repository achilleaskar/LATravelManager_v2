namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _101 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingInfoPerDays", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Cities", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Countries", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.ExcursionDates", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.ExcursionCategories", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Payments", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.HotelCategories", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.RoomTypes", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Leaders", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.StartingPlaces", "ModifiedDate", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StartingPlaces", "ModifiedDate");
            DropColumn("dbo.Leaders", "ModifiedDate");
            DropColumn("dbo.RoomTypes", "ModifiedDate");
            DropColumn("dbo.HotelCategories", "ModifiedDate");
            DropColumn("dbo.Payments", "ModifiedDate");
            DropColumn("dbo.ExcursionCategories", "ModifiedDate");
            DropColumn("dbo.ExcursionDates", "ModifiedDate");
            DropColumn("dbo.Countries", "ModifiedDate");
            DropColumn("dbo.Cities", "ModifiedDate");
            DropColumn("dbo.BookingInfoPerDays", "ModifiedDate");
        }
    }
}
