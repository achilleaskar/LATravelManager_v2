namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1221 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "User_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Bookings", "User_Id");
            AddForeignKey("dbo.Bookings", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "User_Id", "dbo.Users");
            DropIndex("dbo.Bookings", new[] { "User_Id" });
            AlterColumn("dbo.Bookings", "User_Id", c => c.Int());
            CreateIndex("dbo.Bookings", "User_Id");
            AddForeignKey("dbo.Bookings", "User_Id", "dbo.Users", "Id");
        }
    }
}