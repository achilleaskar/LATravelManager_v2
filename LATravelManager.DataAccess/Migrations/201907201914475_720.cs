namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _720 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "CancelReason", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Personal_Booking", "CancelReason", c => c.String(maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Personal_Booking", "CancelReason");
            DropColumn("dbo.Bookings", "CancelReason");
        }
    }
}