namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1129 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "SecondDepart", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Bookings", "SecondDepart");
        }
    }
}