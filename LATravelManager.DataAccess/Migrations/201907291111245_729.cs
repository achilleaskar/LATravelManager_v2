namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _729 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ThirdParty_Booking",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CancelReason = c.String(maxLength: 200, unicode: false),
                    Destination = c.String(maxLength: 30, unicode: false),
                    CheckIn = c.DateTime(nullable: false, precision: 0),
                    CheckOut = c.DateTime(nullable: false, precision: 0),
                    Comment = c.String(maxLength: 200, unicode: false),
                    Commision = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Description = c.String(maxLength: 250, unicode: false),
                    Disabled = c.Boolean(nullable: false),
                    DisableDate = c.DateTime(precision: 0),
                    NetPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Reciept = c.Boolean(nullable: false),
                    Stay = c.String(maxLength: 200, unicode: false),
                    CreatedDate = c.DateTime(nullable: false, precision: 0),
                    ModifiedDate = c.DateTime(precision: 0),
                    DisabledBy_Id = c.Int(),
                    File_Id = c.Int(),
                    Partner_Id = c.Int(),
                    User_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DisabledBy_Id)
                .ForeignKey("dbo.CustomFiles", t => t.File_Id)
                .ForeignKey("dbo.Partners", t => t.Partner_Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.DisabledBy_Id)
                .Index(t => t.File_Id)
                .Index(t => t.Partner_Id)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.CustomFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FileName = c.String(maxLength: 255, unicode: false),
                    Content = c.Binary(),
                    FileType = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.ChangeInBookings", "ThirdParty_Booking_Id", c => c.Int());
            AddColumn("dbo.Payments", "ThirdParty_Booking_Id", c => c.Int());
            AddColumn("dbo.Customers", "ThirdParty_Booking_Id", c => c.Int());
            CreateIndex("dbo.ChangeInBookings", "ThirdParty_Booking_Id");
            CreateIndex("dbo.Payments", "ThirdParty_Booking_Id");
            CreateIndex("dbo.Customers", "ThirdParty_Booking_Id");
            AddForeignKey("dbo.ChangeInBookings", "ThirdParty_Booking_Id", "dbo.ThirdParty_Booking", "Id");
            AddForeignKey("dbo.Customers", "ThirdParty_Booking_Id", "dbo.ThirdParty_Booking", "Id");
            AddForeignKey("dbo.Payments", "ThirdParty_Booking_Id", "dbo.ThirdParty_Booking", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.ThirdParty_Booking", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Payments", "ThirdParty_Booking_Id", "dbo.ThirdParty_Booking");
            DropForeignKey("dbo.ThirdParty_Booking", "Partner_Id", "dbo.Partners");
            DropForeignKey("dbo.ThirdParty_Booking", "File_Id", "dbo.CustomFiles");
            DropForeignKey("dbo.ThirdParty_Booking", "DisabledBy_Id", "dbo.Users");
            DropForeignKey("dbo.Customers", "ThirdParty_Booking_Id", "dbo.ThirdParty_Booking");
            DropForeignKey("dbo.ChangeInBookings", "ThirdParty_Booking_Id", "dbo.ThirdParty_Booking");
            DropIndex("dbo.ThirdParty_Booking", new[] { "User_Id" });
            DropIndex("dbo.ThirdParty_Booking", new[] { "Partner_Id" });
            DropIndex("dbo.ThirdParty_Booking", new[] { "File_Id" });
            DropIndex("dbo.ThirdParty_Booking", new[] { "DisabledBy_Id" });
            DropIndex("dbo.Customers", new[] { "ThirdParty_Booking_Id" });
            DropIndex("dbo.Payments", new[] { "ThirdParty_Booking_Id" });
            DropIndex("dbo.ChangeInBookings", new[] { "ThirdParty_Booking_Id" });
            DropColumn("dbo.Customers", "ThirdParty_Booking_Id");
            DropColumn("dbo.Payments", "ThirdParty_Booking_Id");
            DropColumn("dbo.ChangeInBookings", "ThirdParty_Booking_Id");
            DropTable("dbo.CustomFiles");
            DropTable("dbo.ThirdParty_Booking");
        }
    }
}