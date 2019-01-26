namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1143 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Comment", c => c.String(maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Customers", "Comment");
        }
    }
}