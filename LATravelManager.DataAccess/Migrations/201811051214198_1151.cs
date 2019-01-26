namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1151 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Tel", c => c.String(nullable: false, maxLength: 18, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Customers", "Tel", c => c.String(maxLength: 200, unicode: false));
        }
    }
}