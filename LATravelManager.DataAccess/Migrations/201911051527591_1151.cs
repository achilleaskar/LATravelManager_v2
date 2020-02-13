namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1151 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "Comment");
        }

        public override void Down()
        {
            AddColumn("dbo.Customers", "Comment", c => c.String(maxLength: 200, unicode: false));
        }
    }
}