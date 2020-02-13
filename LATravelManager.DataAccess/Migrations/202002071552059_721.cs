namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _721 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "NotifOk", c => c.Boolean(nullable: false));
            AddColumn("dbo.Options", "Paid", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bookings", "RecieptSent");
            DropColumn("dbo.Personal_Booking", "RecieptSent");
            DropColumn("dbo.ThirdParty_Booking", "RecieptSent");
        }

        public override void Down()
        {
            AddColumn("dbo.ThirdParty_Booking", "RecieptSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Personal_Booking", "RecieptSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "RecieptSent", c => c.Boolean(nullable: false));
            DropColumn("dbo.Options", "Paid");
            DropColumn("dbo.Services", "NotifOk");
        }
    }
}