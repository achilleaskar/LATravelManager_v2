namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _718 : DbMigration
    {
        public override void Up()
        {
        }

        public override void Down()
        {
            DropColumn("dbo.Payments", "Checked");
        }
    }
}