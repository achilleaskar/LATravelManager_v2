namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1029 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Note = c.String(maxLength: 200, unicode: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        ModifiedDate = c.DateTime(precision: 0),
                        Hotel_Id = c.Int(),
                        RoomType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id)
                .ForeignKey("dbo.RoomTypes", t => t.RoomType_Id)
                .Index(t => t.Hotel_Id)
                .Index(t => t.RoomType_Id);
            
            CreateTable(
                "dbo.BookingInfoPerDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        IsAllotement = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        ModifiedDate = c.DateTime(precision: 0),
                        Option_Id = c.Int(),
                        Reservation_Id = c.Int(),
                        Room_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RoomOptions", t => t.Option_Id)
                .ForeignKey("dbo.Reservations", t => t.Reservation_Id)
                .ForeignKey("dbo.Rooms", t => t.Room_Id)
                .Index(t => t.Option_Id)
                .Index(t => t.Reservation_Id)
                .Index(t => t.Room_Id);
            
            CreateTable(
                "dbo.RoomOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OptionDate = c.DateTime(nullable: false, precision: 0),
                        OptionNote = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        ModifiedDate = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoomTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200, unicode: false),
                        MinCapacity = c.Int(nullable: false),
                        MaxCapacity = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        ModifiedDate = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Countries", "Continentindex", c => c.Int(nullable: false));
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Countries", "Name", c => c.String(nullable: false, maxLength: 20, unicode: false));
            DropColumn("dbo.Countries", "Continent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Countries", "Continent", c => c.Int(nullable: false));
            DropForeignKey("dbo.Rooms", "RoomType_Id", "dbo.RoomTypes");
            DropForeignKey("dbo.Rooms", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.BookingInfoPerDays", "Room_Id", "dbo.Rooms");
            DropForeignKey("dbo.BookingInfoPerDays", "Reservation_Id", "dbo.Reservations");
            DropForeignKey("dbo.BookingInfoPerDays", "Option_Id", "dbo.RoomOptions");
            DropIndex("dbo.BookingInfoPerDays", new[] { "Room_Id" });
            DropIndex("dbo.BookingInfoPerDays", new[] { "Reservation_Id" });
            DropIndex("dbo.BookingInfoPerDays", new[] { "Option_Id" });
            DropIndex("dbo.Rooms", new[] { "RoomType_Id" });
            DropIndex("dbo.Rooms", new[] { "Hotel_Id" });
            AlterColumn("dbo.Countries", "Name", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false, maxLength: 15, unicode: false));
            DropColumn("dbo.Countries", "Continentindex");
            DropTable("dbo.RoomTypes");
            DropTable("dbo.RoomOptions");
            DropTable("dbo.BookingInfoPerDays");
            DropTable("dbo.Rooms");
        }
    }
}
