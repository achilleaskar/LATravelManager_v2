namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1261 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "FirstHotel", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "FirstHotel");
        }
    }
}
