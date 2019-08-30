namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _719 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Disabled", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Reservations", "Disabled");
        }
    }
}