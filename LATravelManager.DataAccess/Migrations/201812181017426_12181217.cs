namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _12181217 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservations", "FirstHotel", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Customers", "Comment", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.OptionalExcursions", "Name", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.HotelCategories", "Name", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.Options", "Note", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Buses", "Name", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Buses", "StartingPlace", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Buses", "Tel", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Leaders", "Tel", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Leaders", "Name", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Leaders", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Leaders", "Tel", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Buses", "Tel", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Buses", "StartingPlace", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Buses", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Options", "Note", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.HotelCategories", "Name", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AlterColumn("dbo.OptionalExcursions", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Customers", "Comment", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.Reservations", "FirstHotel", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
