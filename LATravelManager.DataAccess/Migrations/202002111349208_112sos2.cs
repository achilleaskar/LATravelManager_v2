namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _112sos2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExtraServices", "Amount", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.ExtraServices", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}