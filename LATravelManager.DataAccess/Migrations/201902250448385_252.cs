namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _252 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Airlines",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Checkin = c.Int(nullable: false),
                    Name = c.String(maxLength: 200, unicode: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Personal_Booking",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Comment = c.String(maxLength: 200, unicode: false),
                    Commision = c.Single(nullable: false),
                    IsPartners = c.Boolean(nullable: false),
                    NetPrice = c.Single(nullable: false),
                    CreatedDate = c.DateTime(nullable: false, precision: 0),
                    ModifiedDate = c.DateTime(precision: 0),
                    Partner_Id = c.Int(),
                    User_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Partners", t => t.Partner_Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Partner_Id)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.Services",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CreatedDate = c.DateTime(nullable: false, precision: 0),
                    ModifiedDate = c.DateTime(precision: 0),
                    WaitingTime = c.DateTime(precision: 0),
                    PNR = c.String(maxLength: 200, unicode: false),
                    FlyFrom = c.String(maxLength: 200, unicode: false),
                    FlyTo = c.String(maxLength: 200, unicode: false),
                    Stop = c.Boolean(),
                    Allerretour = c.Boolean(),
                    FlightReturn = c.DateTime(precision: 0),
                    FlightGo = c.DateTime(precision: 0),
                    StopPlace = c.String(maxLength: 200, unicode: false),
                    Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    Personal_Booking_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personal_Booking", t => t.Personal_Booking_Id)
                .Index(t => t.Personal_Booking_Id);

            AddColumn("dbo.Payments", "Personal_Booking_Id", c => c.Int());
            AddColumn("dbo.Customers", "Personal_Booking_Id", c => c.Int());
            AddColumn("dbo.Customers", "PlaneService_Id", c => c.Int());
            CreateIndex("dbo.Payments", "Personal_Booking_Id");
            CreateIndex("dbo.Customers", "Personal_Booking_Id");
            CreateIndex("dbo.Customers", "PlaneService_Id");
            AddForeignKey("dbo.Customers", "Personal_Booking_Id", "dbo.Personal_Booking", "Id");
            AddForeignKey("dbo.Payments", "Personal_Booking_Id", "dbo.Personal_Booking", "Id");
            AddForeignKey("dbo.Customers", "PlaneService_Id", "dbo.Services", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Personal_Booking", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Services", "Personal_Booking_Id", "dbo.Personal_Booking");
            DropForeignKey("dbo.Customers", "PlaneService_Id", "dbo.Services");
            DropForeignKey("dbo.Payments", "Personal_Booking_Id", "dbo.Personal_Booking");
            DropForeignKey("dbo.Personal_Booking", "Partner_Id", "dbo.Partners");
            DropForeignKey("dbo.Customers", "Personal_Booking_Id", "dbo.Personal_Booking");
            DropIndex("dbo.Services", new[] { "Personal_Booking_Id" });
            DropIndex("dbo.Personal_Booking", new[] { "User_Id" });
            DropIndex("dbo.Personal_Booking", new[] { "Partner_Id" });
            DropIndex("dbo.Customers", new[] { "PlaneService_Id" });
            DropIndex("dbo.Customers", new[] { "Personal_Booking_Id" });
            DropIndex("dbo.Payments", new[] { "Personal_Booking_Id" });
            DropColumn("dbo.Customers", "PlaneService_Id");
            DropColumn("dbo.Customers", "Personal_Booking_Id");
            DropColumn("dbo.Payments", "Personal_Booking_Id");
            DropTable("dbo.Services");
            DropTable("dbo.Personal_Booking");
            DropTable("dbo.Airlines");
        }
    }
}