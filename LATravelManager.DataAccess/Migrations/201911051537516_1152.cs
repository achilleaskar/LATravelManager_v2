namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1152 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "DifferentDates", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Bookings", "Disabled", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Bookings", "IsPartners", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Bookings", "Reciept", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Bookings", "SecondDepart", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Excursions", "FixedDates", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Excursions", "DiscountsExist", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Excursions", "DOBNeeded", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Excursions", "IncludesBus", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Excursions", "IncludesPlane", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Excursions", "IncludesShip", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Excursions", "PassportNeeded", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Buses", "OneWay", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Reservations", "HB", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Reservations", "OnlyStay", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Reservations", "Transfer", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Services", "Allerretour", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Services", "Stop", c => c.Boolean(storeType: "bit"));
            AlterColumn("dbo.Personal_Booking", "Disabled", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Personal_Booking", "Reciept", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Personal_Booking", "IsPartners", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Partners", "Person", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.Payments", "Checked", c => c.Boolean(storeType: "bit"));
            AlterColumn("dbo.Payments", "Outgoing", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.ThirdParty_Booking", "Disabled", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.ThirdParty_Booking", "Reciept", c => c.Boolean(nullable: false, storeType: "bit"));
            AlterColumn("dbo.ExcursionDates", "NightStart", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExcursionDates", "NightStart", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ThirdParty_Booking", "Reciept", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ThirdParty_Booking", "Disabled", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Payments", "Outgoing", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Payments", "Checked", c => c.Boolean());
            AlterColumn("dbo.Partners", "Person", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Personal_Booking", "IsPartners", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Personal_Booking", "Reciept", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Personal_Booking", "Disabled", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Services", "Stop", c => c.Boolean());
            AlterColumn("dbo.Services", "Allerretour", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Reservations", "Transfer", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Reservations", "OnlyStay", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Reservations", "HB", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Buses", "OneWay", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Excursions", "PassportNeeded", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Excursions", "IncludesShip", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Excursions", "IncludesPlane", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Excursions", "IncludesBus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Excursions", "DOBNeeded", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Excursions", "DiscountsExist", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Excursions", "FixedDates", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Bookings", "SecondDepart", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Bookings", "Reciept", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Bookings", "IsPartners", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Bookings", "Disabled", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Bookings", "DifferentDates", c => c.Boolean(nullable: false));
        }
    }
}
