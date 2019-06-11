namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _521 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rooms", "EndDate");
            DropColumn("dbo.Rooms", "StartDate");
        }

        public override void Down()
        {
            AddColumn("dbo.Rooms", "StartDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Rooms", "EndDate", c => c.DateTime(nullable: false, precision: 0));
        }
    }
}