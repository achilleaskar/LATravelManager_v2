namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1213 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerOptionals", "Cost", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.CustomerOptionals", "Cost");
        }
    }
}