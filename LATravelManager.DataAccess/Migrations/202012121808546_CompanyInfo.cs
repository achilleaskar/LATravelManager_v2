namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CompanyInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ActivityId = c.Int(),
                    AddressNumber = c.Int(nullable: false),
                    AddressRoad = c.String(maxLength: 100, unicode: false),
                    AddressZipCode = c.String(maxLength: 40, unicode: false),
                    BillAddressNumber = c.Int(nullable: false),
                    BillCityId = c.Int(),
                    BillRoad = c.String(maxLength: 100, unicode: false),
                    BillZipCode = c.String(maxLength: 10, unicode: false),
                    CityId = c.Int(),
                    Code = c.Int(nullable: false),
                    Comment = c.String(maxLength: 200, unicode: false),
                    CompanyName = c.String(maxLength: 120, unicode: false),
                    CountryId = c.Int(),
                    CreationDate = c.DateTime(nullable: false, precision: 0),
                    Email = c.String(maxLength: 50, unicode: false),
                    IsAgent = c.Boolean(nullable: false, storeType: "bit"),
                    LastName = c.String(maxLength: 40, unicode: false),
                    MobilePhone = c.String(maxLength: 18, unicode: false),
                    Name = c.String(maxLength: 120, unicode: false),
                    Phone1 = c.String(maxLength: 18, unicode: false),
                    Phone2 = c.String(maxLength: 18, unicode: false),
                    TaxationNumber = c.String(maxLength: 20, unicode: false),
                    TaxOffice = c.String(maxLength: 40, unicode: false),
                    CreatedDate = c.DateTime(nullable: false, precision: 0),
                    AddressCity_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyActivities", t => t.ActivityId)
                .ForeignKey("dbo.Cities", t => t.AddressCity_Id)
                .ForeignKey("dbo.Cities", t => t.BillCityId)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.ActivityId)
                .Index(t => t.BillCityId)
                .Index(t => t.CountryId)
                .Index(t => t.AddressCity_Id);

            CreateTable(
                "dbo.CompanyActivities",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 200, unicode: false),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Partners", "CompanyInfo_Id", c => c.Int());
            CreateIndex("dbo.Partners", "CompanyInfo_Id");
            AddForeignKey("dbo.Partners", "CompanyInfo_Id", "dbo.Companies", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Partners", "CompanyInfo_Id", "dbo.Companies");
            DropForeignKey("dbo.Companies", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Companies", "BillCityId", "dbo.Cities");
            DropForeignKey("dbo.Companies", "AddressCity_Id", "dbo.Cities");
            DropForeignKey("dbo.Companies", "ActivityId", "dbo.CompanyActivities");
            DropIndex("dbo.Companies", new[] { "AddressCity_Id" });
            DropIndex("dbo.Companies", new[] { "CountryId" });
            DropIndex("dbo.Companies", new[] { "BillCityId" });
            DropIndex("dbo.Companies", new[] { "ActivityId" });
            DropIndex("dbo.Partners", new[] { "CompanyInfo_Id" });
            DropColumn("dbo.Partners", "CompanyInfo_Id");
            DropTable("dbo.CompanyActivities");
            DropTable("dbo.Companies");
        }
    }
}