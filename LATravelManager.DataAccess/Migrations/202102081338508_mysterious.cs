namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mysterious : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.Reciepts", name: "Personal_Booking_Id", newName: "Personal_BookingId");
            //RenameColumn(table: "dbo.Reciepts", name: "Booking_Id", newName: "BookingId");
            //RenameColumn(table: "dbo.Reciepts", name: "ThirdParty_Booking_Id", newName: "ThirdParty_BookingId");
            //RenameIndex(table: "dbo.Reciepts", name: "IX_Booking_Id", newName: "IX_BookingId");
            //RenameIndex(table: "dbo.Reciepts", name: "IX_Personal_Booking_Id", newName: "IX_Personal_BookingId");
            //RenameIndex(table: "dbo.Reciepts", name: "IX_ThirdParty_Booking_Id", newName: "IX_ThirdParty_BookingId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Reciepts", name: "IX_ThirdParty_BookingId", newName: "IX_ThirdParty_Booking_Id");
            RenameIndex(table: "dbo.Reciepts", name: "IX_Personal_BookingId", newName: "IX_Personal_Booking_Id");
            RenameIndex(table: "dbo.Reciepts", name: "IX_BookingId", newName: "IX_Booking_Id");
            RenameColumn(table: "dbo.Reciepts", name: "ThirdParty_BookingId", newName: "ThirdParty_Booking_Id");
            RenameColumn(table: "dbo.Reciepts", name: "BookingId", newName: "Booking_Id");
            RenameColumn(table: "dbo.Reciepts", name: "Personal_BookingId", newName: "Personal_Booking_Id");
        }
    }
}
