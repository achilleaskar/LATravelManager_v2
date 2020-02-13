namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _114 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "SelectedBus_Id", c => c.Int());
            CreateIndex("dbo.Transactions", "SelectedBus_Id");
            AddForeignKey("dbo.Transactions", "SelectedBus_Id", "dbo.Buses", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "SelectedBus_Id", "dbo.Buses");
            DropIndex("dbo.Transactions", new[] { "SelectedBus_Id" });
            DropColumn("dbo.Transactions", "SelectedBus_Id");
        }
    }
}