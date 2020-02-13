namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _112 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtraServices",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Description = c.String(maxLength: 200, unicode: false),
                    Booking_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookings", t => t.Booking_Id)
                .Index(t => t.Booking_Id);

            AddColumn("dbo.Excursions", "Deactivated", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropForeignKey("dbo.ExtraServices", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.ExtraServices", new[] { "Booking_Id" });
            DropColumn("dbo.Excursions", "Deactivated");
            DropTable("dbo.ExtraServices");
        }
    }
}