namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1214 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerOptionals", "PaymentType", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.CustomerOptionals", "PaymentType");
        }
    }
}