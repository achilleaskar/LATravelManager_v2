namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _115 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Comment", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.Customers", "PassportNum", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Customers", "StartingPlace", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.Customers", "Surename", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Customers", "Surename", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.Customers", "StartingPlace", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Customers", "PassportNum", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Customers", "Comment", c => c.String(maxLength: 200, unicode: false));
        }
    }
}