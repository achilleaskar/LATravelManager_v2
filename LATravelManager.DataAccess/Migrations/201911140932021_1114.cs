namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1114 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Hotels", "Email", c => c.String(maxLength: 60, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Hotels", "Email", c => c.String(maxLength: 30, unicode: false));
        }
    }
}