namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricing : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PricingPeriods", "Excursion_Id", "dbo.Excursions");
            DropForeignKey("dbo.HotelPricings", "PricingPeriod_Id", "dbo.PricingPeriods");
            DropForeignKey("dbo.PricingPeriods", "ChilndEtcPrices_Id", "dbo.ChildEtcPrices");
            DropForeignKey("dbo.ChildEtcPrices", "Transfer_Id", "dbo.HotelPricings");
            DropForeignKey("dbo.HotelPricings", "Hotel_Id", "dbo.Hotels");
            DropIndex("dbo.HotelPricings", new[] { "PricingPeriod_Id" });
            DropIndex("dbo.HotelPricings", new[] { "Hotel_Id" });
            DropIndex("dbo.ChildEtcPrices", new[] { "Transfer_Id" });
            DropIndex("dbo.PricingPeriods", new[] { "Excursion_Id" });
            DropIndex("dbo.PricingPeriods", new[] { "ChilndEtcPrices_Id" });
            DropColumn("dbo.ExcursionTimes", "ExtraAmmount");
            DropTable("dbo.HotelPricings");
            DropTable("dbo.ChildEtcPrices");
            DropTable("dbo.PricingPeriods");
        }
    }
}
