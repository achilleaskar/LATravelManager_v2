namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class afterskiath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Reciept", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Bookings", "Reciept");
        }
    }
}