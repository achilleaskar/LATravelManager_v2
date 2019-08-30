namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _724 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Excursions", "FixedDates", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Excursions", "FixedDates");
        }
    }
}