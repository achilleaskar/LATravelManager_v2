namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1211 : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookingInfoPerDays", "Reservation_Id", c => c.Int());
            CreateIndex("dbo.BookingInfoPerDays", "Reservation_Id");
            AddForeignKey("dbo.BookingInfoPerDays", "Reservation_Id", "dbo.Reservations", "Id");
        }
    }
}
