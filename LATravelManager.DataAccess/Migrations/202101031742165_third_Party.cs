namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class third_Party : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThirdParty_Booking", "IsPartners", c => c.Boolean(nullable: false, storeType: "bit"));
            AddColumn("dbo.ThirdParty_Booking", "BuyerPartner_Id", c => c.Int());
            CreateIndex("dbo.ThirdParty_Booking", "BuyerPartner_Id");
            AddForeignKey("dbo.ThirdParty_Booking", "BuyerPartner_Id", "dbo.Partners", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThirdParty_Booking", "BuyerPartner_Id", "dbo.Partners");
            DropIndex("dbo.ThirdParty_Booking", new[] { "BuyerPartner_Id" });
            DropColumn("dbo.ThirdParty_Booking", "BuyerPartner_Id");
            DropColumn("dbo.ThirdParty_Booking", "IsPartners");
        }
    }
}
