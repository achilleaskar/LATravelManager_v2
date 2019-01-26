namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1161 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "City_Id", c => c.Int());
            CreateIndex("dbo.Bookings", "City_Id");
            AddForeignKey("dbo.Bookings", "City_Id", "dbo.Cities", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "City_Id", "dbo.Cities");
            DropIndex("dbo.Bookings", new[] { "City_Id" });
            DropColumn("dbo.Bookings", "City_Id");
        }
    }
}