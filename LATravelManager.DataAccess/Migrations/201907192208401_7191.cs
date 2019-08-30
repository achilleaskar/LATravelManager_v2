namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _7191 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "DisableDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Bookings", "DisabledBy_Id", c => c.Int());
            AddColumn("dbo.Personal_Booking", "Disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Personal_Booking", "DisableDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.Personal_Booking", "DisabledBy_Id", c => c.Int());
            CreateIndex("dbo.Bookings", "DisabledBy_Id");
            CreateIndex("dbo.Personal_Booking", "DisabledBy_Id");
            AddForeignKey("dbo.Bookings", "DisabledBy_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Personal_Booking", "DisabledBy_Id", "dbo.Users", "Id");
            DropColumn("dbo.Reservations", "Disabled");
        }

        public override void Down()
        {
            AddColumn("dbo.Reservations", "Disabled", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Personal_Booking", "DisabledBy_Id", "dbo.Users");
            DropForeignKey("dbo.Bookings", "DisabledBy_Id", "dbo.Users");
            DropIndex("dbo.Personal_Booking", new[] { "DisabledBy_Id" });
            DropIndex("dbo.Bookings", new[] { "DisabledBy_Id" });
            DropColumn("dbo.Personal_Booking", "DisabledBy_Id");
            DropColumn("dbo.Personal_Booking", "DisableDate");
            DropColumn("dbo.Personal_Booking", "Disabled");
            DropColumn("dbo.Bookings", "DisabledBy_Id");
            DropColumn("dbo.Bookings", "DisableDate");
            DropColumn("dbo.Bookings", "Disabled");
        }
    }
}