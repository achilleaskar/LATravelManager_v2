namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _121804274 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StartingPlaces", "Details", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.StartingPlaces", "ReturnTime", c => c.String(maxLength: 6, unicode: false));
            AddColumn("dbo.StartingPlaces", "StartTime", c => c.String(maxLength: 6, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StartingPlaces", "StartTime");
            DropColumn("dbo.StartingPlaces", "ReturnTime");
            DropColumn("dbo.StartingPlaces", "Details");
        }
    }
}
