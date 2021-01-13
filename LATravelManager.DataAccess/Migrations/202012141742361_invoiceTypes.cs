namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoiceTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AirTicketsReciepts",
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.CustomFiles", t => t.RecieptFileId)
                .Index(t => t.CompanyId)
                .Index(t => t.RecieptFileId);
            
            CreateTable(
                "dbo.CancelationInvoices",
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.CustomFiles", t => t.RecieptFileId)
                .Index(t => t.CompanyId)
                .Index(t => t.RecieptFileId);
            
            CreateTable(
                "dbo.CreditInvoices",
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.CustomFiles", t => t.RecieptFileId)
                .Index(t => t.CompanyId)
                .Index(t => t.RecieptFileId);
            
            CreateTable(
                "dbo.FerryTicketsReciepts",
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.CustomFiles", t => t.RecieptFileId)
                .Index(t => t.CompanyId)
                .Index(t => t.RecieptFileId);
            
            CreateTable(
                "dbo.ServiceInvoices",
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.CustomFiles", t => t.RecieptFileId)
                .Index(t => t.CompanyId)
                .Index(t => t.RecieptFileId);
            
            CreateTable(
                "dbo.ServiceReciepts",
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.CustomFiles", t => t.RecieptFileId)
                .Index(t => t.CompanyId)
                .Index(t => t.RecieptFileId);
            
            AddColumn("dbo.Services", "Reciept", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceReciepts", "RecieptFileId", "dbo.CustomFiles");
            DropForeignKey("dbo.ServiceReciepts", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.ServiceInvoices", "RecieptFileId", "dbo.CustomFiles");
            DropForeignKey("dbo.ServiceInvoices", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.FerryTicketsReciepts", "RecieptFileId", "dbo.CustomFiles");
            DropForeignKey("dbo.FerryTicketsReciepts", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CreditInvoices", "RecieptFileId", "dbo.CustomFiles");
            DropForeignKey("dbo.CreditInvoices", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CancelationInvoices", "RecieptFileId", "dbo.CustomFiles");
            DropForeignKey("dbo.CancelationInvoices", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AirTicketsReciepts", "RecieptFileId", "dbo.CustomFiles");
            DropForeignKey("dbo.AirTicketsReciepts", "CompanyId", "dbo.Companies");
            DropIndex("dbo.ServiceReciepts", new[] { "RecieptFileId" });
            DropIndex("dbo.ServiceReciepts", new[] { "CompanyId" });
            DropIndex("dbo.ServiceInvoices", new[] { "RecieptFileId" });
            DropIndex("dbo.ServiceInvoices", new[] { "CompanyId" });
            DropIndex("dbo.FerryTicketsReciepts", new[] { "RecieptFileId" });
            DropIndex("dbo.FerryTicketsReciepts", new[] { "CompanyId" });
            DropIndex("dbo.CreditInvoices", new[] { "RecieptFileId" });
            DropIndex("dbo.CreditInvoices", new[] { "CompanyId" });
            DropIndex("dbo.CancelationInvoices", new[] { "RecieptFileId" });
            DropIndex("dbo.CancelationInvoices", new[] { "CompanyId" });
            DropIndex("dbo.AirTicketsReciepts", new[] { "RecieptFileId" });
            DropIndex("dbo.AirTicketsReciepts", new[] { "CompanyId" });
            DropColumn("dbo.Services", "Reciept");
            DropColumn("dbo.ThirdParty_Booking", "Billed");
            DropTable("dbo.ServiceReciepts");
            DropTable("dbo.ServiceInvoices");
            DropTable("dbo.FerryTicketsReciepts");
            DropTable("dbo.CreditInvoices");
            DropTable("dbo.CancelationInvoices");
            DropTable("dbo.AirTicketsReciepts");
        }
    }
}
