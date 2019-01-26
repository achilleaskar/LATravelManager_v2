namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1264 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "HB", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "HB");
        }
    }
}
