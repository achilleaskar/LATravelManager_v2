namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecieptSeries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reciepts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(),
                        RecieptType = c.Int(nullable: false),
                        RecieptNumber = c.Int(nullable: false),
                        RecieptDescription = c.String(maxLength: 200, unicode: false),
                        RecieptFileId = c.Int(),
                        Canceled = c.Boolean(nullable: false, storeType: "bit"),
                        Printed = c.Boolean(nullable: false, storeType: "bit"),
                        Series_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.CustomFiles", t => t.RecieptFileId)
                .ForeignKey("dbo.ReciepSeries", t => t.Series_Id)
                .Index(t => t.CompanyId)
                .Index(t => t.RecieptFileId)
                .Index(t => t.Series_Id);
            
            CreateTable(
                "dbo.ReciepSeries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateStarted = c.DateTime(nullable: false, precision: 0),
                        DateEnded = c.DateTime(nullable: false, precision: 0),
                        Disabled = c.Boolean(nullable: false, storeType: "bit"),
                        Name = c.String(maxLength: 40, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reciepts", "Series_Id", "dbo.ReciepSeries");
            DropForeignKey("dbo.Reciepts", "RecieptFileId", "dbo.CustomFiles");
            DropForeignKey("dbo.Reciepts", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Reciepts", new[] { "Series_Id" });
            DropIndex("dbo.Reciepts", new[] { "RecieptFileId" });
            DropIndex("dbo.Reciepts", new[] { "CompanyId" });
            DropTable("dbo.ReciepSeries");
            DropTable("dbo.Reciepts");
        }
    }
}
