namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sk527 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingInfoPerDays", "RoomTypeEnm", c => c.Int(nullable: false));
            DropColumn("dbo.BookingInfoPerDays", "IsAllotment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookingInfoPerDays", "IsAllotment", c => c.Boolean(nullable: false));
            DropColumn("dbo.BookingInfoPerDays", "RoomTypeEnm");
        }
    }
}
