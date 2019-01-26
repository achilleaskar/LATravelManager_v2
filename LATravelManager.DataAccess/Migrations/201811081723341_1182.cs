namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1182 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BaseLocation_Id", c => c.Int());
            CreateIndex("dbo.Users", "BaseLocation_Id");
            AddForeignKey("dbo.Users", "BaseLocation_Id", "dbo.StartingPlaces", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Users", "BaseLocation_Id", "dbo.StartingPlaces");
            DropIndex("dbo.Users", new[] { "BaseLocation_Id" });
            DropColumn("dbo.Users", "BaseLocation_Id");
        }
    }
}