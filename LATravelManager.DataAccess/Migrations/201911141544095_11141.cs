namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _11141 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "SeatNum", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Customers", "SeatNum");
        }
    }
}