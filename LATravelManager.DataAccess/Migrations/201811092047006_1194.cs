namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1194 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Excursion_Id", c => c.Int());
            CreateIndex("dbo.Bookings", "Excursion_Id");
            AddForeignKey("dbo.Bookings", "Excursion_Id", "dbo.Excursions", "Id");
        }

        public override void Down()
        {
            AddColumn("dbo.Bookings", "City_Id", c => c.Int());
            DropForeignKey("dbo.Bookings", "Excursion_Id", "dbo.Excursions");
            DropIndex("dbo.Bookings", new[] { "Excursion_Id" });
            DropColumn("dbo.Bookings", "Excursion_Id");
            CreateIndex("dbo.Bookings", "City_Id");
            AddForeignKey("dbo.Bookings", "City_Id", "dbo.Cities", "Id");
        }
    }
}