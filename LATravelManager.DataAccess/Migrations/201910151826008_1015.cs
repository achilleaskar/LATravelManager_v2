namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1015 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "User_Id", c => c.Int());
            AddColumn("dbo.Transactions", "GeneralOrDatesExpenseCategory", c => c.Int(nullable: false));
            CreateIndex("dbo.Rooms", "User_Id");
            AddForeignKey("dbo.Rooms", "User_Id", "dbo.Users", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Rooms", "User_Id", "dbo.Users");
            DropIndex("dbo.Rooms", new[] { "User_Id" });
            DropColumn("dbo.Transactions", "GeneralOrDatesExpenseCategory");
            DropColumn("dbo.Rooms", "User_Id");
        }
    }
}