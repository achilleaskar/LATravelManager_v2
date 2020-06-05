namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PricingPeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false, precision: 0),
                        Parted = c.Boolean(nullable: false, storeType: "bit"),
                        FromB = c.DateTime(nullable: false, precision: 0),
                        ToB = c.DateTime(nullable: false, precision: 0),
                        Name = c.String(maxLength: 200, unicode: false),
                        To = c.DateTime(nullable: false, precision: 0),
                        ChilndEtcPrices_Id = c.Int(),
                        Excursion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChildEtcPrices", t => t.ChilndEtcPrices_Id)
                .ForeignKey("dbo.Excursions", t => t.Excursion_Id)
                .Index(t => t.ChilndEtcPrices_Id)
                .Index(t => t.Excursion_Id);
            
            CreateTable(
                "dbo.ChildEtcPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Under12 = c.Int(nullable: false),
                        Under18 = c.Int(nullable: false),
                        OnlyStayDiscount = c.Int(nullable: false),
                        Single = c.Int(nullable: false),
                        Transfer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotelPricings", t => t.Transfer_Id)
                .Index(t => t.Transfer_Id);
            
            CreateTable(
                "dbo.HotelPricings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        N2 = c.Int(nullable: false),
                        N3 = c.Int(nullable: false),
                        N4 = c.Int(nullable: false),
                        N5 = c.Int(nullable: false),
                        N6 = c.Int(nullable: false),
                        N7 = c.Int(nullable: false),
                        N8 = c.Int(nullable: false),
                        Hotel_Id = c.Int(),
                        PricingPeriod_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id)
                .ForeignKey("dbo.PricingPeriods", t => t.PricingPeriod_Id)
                .Index(t => t.Hotel_Id)
                .Index(t => t.PricingPeriod_Id);
            
            AddColumn("dbo.ExcursionTimes", "ExtraAmmount", c => c.Int(nullable: false));
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
