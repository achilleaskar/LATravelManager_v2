namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1933 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "PartnerEmail", c => c.String(maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Bookings", "PartnerEmail");
        }
    }
}