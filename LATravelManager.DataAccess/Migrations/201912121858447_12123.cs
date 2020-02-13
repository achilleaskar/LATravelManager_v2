namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _12123 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerOptionals", "Leader_Id", c => c.Int());
            AddColumn("dbo.OptionalExcursions", "Cost", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomerOptionals", "Leader_Id");
            AddForeignKey("dbo.CustomerOptionals", "Leader_Id", "dbo.Leaders", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.CustomerOptionals", "Leader_Id", "dbo.Leaders");
            DropIndex("dbo.CustomerOptionals", new[] { "Leader_Id" });
            DropColumn("dbo.OptionalExcursions", "Cost");
            DropColumn("dbo.CustomerOptionals", "Leader_Id");
        }
    }
}