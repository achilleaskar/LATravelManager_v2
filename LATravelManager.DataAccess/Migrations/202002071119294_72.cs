namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _72 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "ProformaSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "RecieptSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Personal_Booking", "VoucherSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Personal_Booking", "ProformaSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Personal_Booking", "RecieptSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.ThirdParty_Booking", "VoucherSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.ThirdParty_Booking", "ProformaSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.ThirdParty_Booking", "RecieptSent", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.ThirdParty_Booking", "RecieptSent");
            DropColumn("dbo.ThirdParty_Booking", "ProformaSent");
            DropColumn("dbo.ThirdParty_Booking", "VoucherSent");
            DropColumn("dbo.Personal_Booking", "RecieptSent");
            DropColumn("dbo.Personal_Booking", "ProformaSent");
            DropColumn("dbo.Personal_Booking", "VoucherSent");
            DropColumn("dbo.Bookings", "RecieptSent");
            DropColumn("dbo.Bookings", "ProformaSent");
        }
    }
}