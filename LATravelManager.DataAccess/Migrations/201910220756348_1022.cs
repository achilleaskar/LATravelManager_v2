namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1022 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "LeaderDriver", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Customers", "LeaderDriver");
        }
    }
}