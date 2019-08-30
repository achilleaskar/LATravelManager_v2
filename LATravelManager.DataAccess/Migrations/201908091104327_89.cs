namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _89 : DbMigration
    {
        public override void Up()
        {
        }

        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.Reservations", new[] { "Booking_Id" });
            AlterColumn("dbo.Reservations", "Booking_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Reservations", "Booking_Id");
            AddForeignKey("dbo.Reservations", "Booking_Id", "dbo.Bookings", "Id", cascadeDelete: true);
        }
    }
}