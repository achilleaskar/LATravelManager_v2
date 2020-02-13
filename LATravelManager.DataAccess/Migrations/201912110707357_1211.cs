namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1211 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bookings", "PartnerEmail");
            DropColumn("dbo.Bookings", "ModifiedDate");
            DropColumn("dbo.Users", "ModifiedDate");
            DropColumn("dbo.Excursions", "ModifiedDate");
            DropColumn("dbo.Buses", "ModifiedDate");
            DropColumn("dbo.Customers", "ModifiedDate");
            DropColumn("dbo.OptionalExcursions", "ModifiedDate");
            DropColumn("dbo.Reservations", "ModifiedDate");
            DropColumn("dbo.Hotels", "ModifiedDate");
            DropColumn("dbo.Rooms", "ModifiedDate");
            DropColumn("dbo.Options", "ModifiedDate");
            DropColumn("dbo.Services", "ModifiedDate");
            DropColumn("dbo.Personal_Booking", "ModifiedDate");
            DropColumn("dbo.Partners", "ModifiedDate");
            DropColumn("dbo.ThirdParty_Booking", "ModifiedDate");
            DropColumn("dbo.Vehicles", "ModifiedDate");
            DropColumn("dbo.Transactions", "ModifiedDate");
        }

        public override void Down()
        {
            AddColumn("dbo.Transactions", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Vehicles", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.ThirdParty_Booking", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Partners", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Personal_Booking", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Services", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Options", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Rooms", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Hotels", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Reservations", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.OptionalExcursions", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Customers", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Buses", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Excursions", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Users", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Bookings", "ModifiedDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Bookings", "PartnerEmail", c => c.String(maxLength: 200, unicode: false));
        }
    }
}