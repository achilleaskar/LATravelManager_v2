namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _52821 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Profit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Personal_Booking", "ExtraProfit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Personal_Booking", "Commision");
            DropColumn("dbo.Personal_Booking", "NetPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Personal_Booking", "NetPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Personal_Booking", "Commision", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Personal_Booking", "ExtraProfit");
            DropColumn("dbo.Services", "Profit");
        }
    }
}
