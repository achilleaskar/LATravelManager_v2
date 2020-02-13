namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _115 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Board", c => c.Byte(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Customers", "Board");
        }
    }
}