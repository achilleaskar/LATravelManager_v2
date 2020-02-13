namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _10201 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Leaders", "Name", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Leaders", "Tel", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Leaders", "Tel", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Leaders", "Name", c => c.String(maxLength: 20, unicode: false));
        }
    }
}