namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _212 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Excursions", "Name", c => c.String(nullable: false, maxLength: 30, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Excursions", "Name", c => c.String(maxLength: 30, unicode: false));
        }
    }
}