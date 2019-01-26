namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1121 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "CheckIn", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.Customers", "CheckOut", c => c.DateTime(nullable: false, precision: 0));
        }

        public override void Down()
        {
            AlterColumn("dbo.Customers", "CheckOut", c => c.DateTime(precision: 0));
            AlterColumn("dbo.Customers", "CheckIn", c => c.DateTime(precision: 0));
        }
    }
}