namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1120 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "User_Id", c => c.Int());
            CreateIndex("dbo.Bookings", "User_Id");
            AddForeignKey("dbo.Bookings", "User_Id", "dbo.Users", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "User_Id", "dbo.Users");
            DropIndex("dbo.Bookings", new[] { "User_Id" });
            DropColumn("dbo.Bookings", "User_Id");
        }
    }
}