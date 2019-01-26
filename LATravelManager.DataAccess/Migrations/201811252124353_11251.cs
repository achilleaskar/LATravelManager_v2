namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _11251 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Partners", "Email", c => c.String(maxLength: 30, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Partners", "Email", c => c.String(nullable: false, maxLength: 30, unicode: false));
        }
    }
}