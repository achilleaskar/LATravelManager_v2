namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _721 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeInBookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Description = c.String(maxLength: 200, unicode: false),
                        Booking_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookings", t => t.Booking_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Booking_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeInBookings", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ChangeInBookings", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.ChangeInBookings", new[] { "User_Id" });
            DropIndex("dbo.ChangeInBookings", new[] { "Booking_Id" });
            DropTable("dbo.ChangeInBookings");
        }
    }
}
