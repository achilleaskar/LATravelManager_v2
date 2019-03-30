namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _103 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Excursions", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Partners", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Users", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Reservations", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Customers", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.OptionalExcursions", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Hotels", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Rooms", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Options", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Buses", "ModifiedDate", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Buses", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Options", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Rooms", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Hotels", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.OptionalExcursions", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Customers", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Reservations", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Users", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Partners", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Excursions", "ModifiedDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Bookings", "ModifiedDate", c => c.DateTime(precision: 0));
        }
    }
}
