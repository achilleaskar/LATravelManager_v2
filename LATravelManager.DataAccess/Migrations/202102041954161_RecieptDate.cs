namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecieptDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reciepts", "Date", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Reciepts", "Booking_Id", c => c.Int());
            AddColumn("dbo.Reciepts", "Personal_Booking_Id", c => c.Int());
            AddColumn("dbo.Reciepts", "ThirdParty_Booking_Id", c => c.Int());
            CreateIndex("dbo.Reciepts", "Booking_Id");
            CreateIndex("dbo.Reciepts", "Personal_Booking_Id");
            CreateIndex("dbo.Reciepts", "ThirdParty_Booking_Id");
            AddForeignKey("dbo.Reciepts", "Booking_Id", "dbo.Bookings", "Id");
            AddForeignKey("dbo.Reciepts", "Personal_Booking_Id", "dbo.Personal_Booking", "Id");
            AddForeignKey("dbo.Reciepts", "ThirdParty_Booking_Id", "dbo.ThirdParty_Booking", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reciepts", "ThirdParty_Booking_Id", "dbo.ThirdParty_Booking");
            DropForeignKey("dbo.Reciepts", "Personal_Booking_Id", "dbo.Personal_Booking");
            DropForeignKey("dbo.Reciepts", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.Reciepts", new[] { "ThirdParty_Booking_Id" });
            DropIndex("dbo.Reciepts", new[] { "Personal_Booking_Id" });
            DropIndex("dbo.Reciepts", new[] { "Booking_Id" });
            DropColumn("dbo.Reciepts", "ThirdParty_Booking_Id");
            DropColumn("dbo.Reciepts", "Personal_Booking_Id");
            DropColumn("dbo.Reciepts", "Booking_Id");
            DropColumn("dbo.Reciepts", "Date");
        }
    }
}
