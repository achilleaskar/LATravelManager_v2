namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _81 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "Outgoing", c => c.Boolean(nullable: false));
            AddColumn("dbo.ThirdParty_Booking", "City", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ThirdParty_Booking", "City");
            DropColumn("dbo.Payments", "Outgoing");
        }
    }
}
