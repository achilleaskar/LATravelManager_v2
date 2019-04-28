namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _442 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "EndDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Rooms", "StartDate", c => c.DateTime(nullable: false, precision: 0));
        }

        public override void Down()
        {
            DropColumn("dbo.Rooms", "StartDate");
            DropColumn("dbo.Rooms", "EndDate");
        }
    }
}