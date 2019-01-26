namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1141 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Partners",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 200, unicode: false),
                    CreatedDate = c.DateTime(nullable: false, precision: 0),
                    ModifiedDate = c.DateTime(precision: 0),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Bookings", "IsPartners", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "NetPrice", c => c.Single(nullable: false));
            AddColumn("dbo.Bookings", "Partner_Id", c => c.Int());
            CreateIndex("dbo.Bookings", "Partner_Id");
            AddForeignKey("dbo.Bookings", "Partner_Id", "dbo.Partners", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "Partner_Id", "dbo.Partners");
            DropIndex("dbo.Bookings", new[] { "Partner_Id" });
            DropColumn("dbo.Bookings", "Partner_Id");
            DropColumn("dbo.Bookings", "NetPrice");
            DropColumn("dbo.Bookings", "IsPartners");
            DropTable("dbo.Partners");
        }
    }
}