namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _114 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Excursions", "IncludesPlane", c => c.Boolean(nullable: false));
            DropColumn("dbo.Excursions", "IncludePlane");
        }

        public override void Down()
        {
            AddColumn("dbo.Excursions", "IncludePlane", c => c.Boolean(nullable: false));
            DropColumn("dbo.Excursions", "IncludesPlane");
        }
    }
}