namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _122 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Email", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.Customers", "Surename", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.Hotels", "Email", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Hotels", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Hotels", "Name", c => c.String(nullable: false, maxLength: 25, unicode: false));
            AlterColumn("dbo.Hotels", "Email", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.Customers", "Surename", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Customers", "Email", c => c.String(maxLength: 30, unicode: false));
        }
    }
}