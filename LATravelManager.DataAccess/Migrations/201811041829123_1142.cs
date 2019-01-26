namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1142 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Amount = c.Single(nullable: false),
                    Comment = c.String(maxLength: 200, unicode: false),
                    Date = c.DateTime(nullable: false, precision: 0),
                    PaymentMethod = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false, precision: 0),
                    ModifiedDate = c.DateTime(precision: 0),
                    User_Id = c.Int(),
                    Booking_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Bookings", t => t.Booking_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Booking_Id);

            AddColumn("dbo.Bookings", "Comment", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Bookings", "Remaining", c => c.Single(nullable: false));
        }

        public override void Down()
        {
            DropForeignKey("dbo.Payments", "Booking_Id", "dbo.Bookings");
            DropForeignKey("dbo.Payments", "User_Id", "dbo.Users");
            DropIndex("dbo.Payments", new[] { "Booking_Id" });
            DropIndex("dbo.Payments", new[] { "User_Id" });
            DropColumn("dbo.Bookings", "Remaining");
            DropColumn("dbo.Bookings", "Comment");
            DropTable("dbo.Payments");
        }
    }
}