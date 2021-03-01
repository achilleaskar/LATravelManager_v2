namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unknown1 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Companies", "BillCityId", "dbo.Cities");
            //DropForeignKey("dbo.Companies", "CountryId", "dbo.Countries");
            //DropIndex("dbo.Companies", new[] { "BillCityId" });
            //DropIndex("dbo.Companies", new[] { "CountryId" });
            //AlterColumn("dbo.Companies", "BillCityId", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "BillRoad", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.Companies", "BillZipCode", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AlterColumn("dbo.Companies", "CompanyName", c => c.String(nullable: false, maxLength: 120, unicode: false));
           // AlterColumn("dbo.Companies", "CountryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "TaxationNumber", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Companies", "TaxOffice", c => c.String(nullable: false, maxLength: 40, unicode: false));
            //CreateIndex("dbo.Companies", "BillCityId");
            //CreateIndex("dbo.Companies", "CountryId");
           // AddForeignKey("dbo.Companies", "BillCityId", "dbo.Cities", "Id", cascadeDelete: true);
           // AddForeignKey("dbo.Companies", "CountryId", "dbo.Countries", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Companies", "BillCityId", "dbo.Cities");
            DropIndex("dbo.Companies", new[] { "CountryId" });
            DropIndex("dbo.Companies", new[] { "BillCityId" });
            AlterColumn("dbo.Companies", "TaxOffice", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.Companies", "TaxationNumber", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Companies", "CountryId", c => c.Int());
            AlterColumn("dbo.Companies", "CompanyName", c => c.String(maxLength: 120, unicode: false));
            AlterColumn("dbo.Companies", "BillZipCode", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.Companies", "BillRoad", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Companies", "BillCityId", c => c.Int());
            CreateIndex("dbo.Companies", "CountryId");
            CreateIndex("dbo.Companies", "BillCityId");
            AddForeignKey("dbo.Companies", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.Companies", "BillCityId", "dbo.Cities", "Id");
            RenameTable(name: "dbo.RecieptSeries", newName: "ReciepSeries");
        }
    }
}
