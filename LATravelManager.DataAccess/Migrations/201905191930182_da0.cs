namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class da0 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "NetPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Bookings", "Commision", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Bookings", "NetPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Payments", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Customers", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Personal_Booking", "Commision", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Personal_Booking", "NetPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Personal_Booking", "NetPrice", c => c.Single(nullable: false));
            AlterColumn("dbo.Personal_Booking", "Commision", c => c.Single(nullable: false));
            AlterColumn("dbo.Customers", "Price", c => c.Single(nullable: false));
            AlterColumn("dbo.Payments", "Amount", c => c.Single(nullable: false));
            AlterColumn("dbo.Bookings", "NetPrice", c => c.Single(nullable: false));
            AlterColumn("dbo.Bookings", "Commision", c => c.Single(nullable: false));
            DropColumn("dbo.Services", "NetPrice");
        }
    }
}
