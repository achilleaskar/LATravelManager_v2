namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1163 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rooms", "IsAllotment");
        }

        public override void Down()
        {
            AddColumn("dbo.Rooms", "IsAllotment", c => c.Boolean(nullable: false));
        }
    }
}