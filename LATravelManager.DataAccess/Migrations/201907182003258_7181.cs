namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _7181 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "Checked", c => c.Boolean());
        }

        public override void Down()
        {
            AlterColumn("dbo.Payments", "Checked", c => c.Boolean(nullable: false));
        }
    }
}