namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _731 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ThirdParty_Booking", "Destination");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ThirdParty_Booking", "Destination", c => c.String(maxLength: 30, unicode: false));
        }
    }
}
