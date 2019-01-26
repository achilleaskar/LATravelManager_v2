namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _12180400 : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "BaseLocation_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "BaseLocation");
            CreateIndex("dbo.Users", "BaseLocation_Id");
            AddForeignKey("dbo.Users", "BaseLocation_Id", "dbo.StartingPlaces", "Id", cascadeDelete: true);
        }
    }
}
