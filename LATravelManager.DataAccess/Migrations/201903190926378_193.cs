namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _193 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Partners", "Email");
        }

        public override void Down()
        {
            AddColumn("dbo.Partners", "Email", c => c.String(maxLength: 30, unicode: false));
        }
    }
}