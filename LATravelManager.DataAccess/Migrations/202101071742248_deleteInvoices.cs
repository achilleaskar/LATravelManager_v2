namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteInvoices : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.AirTicketsReciepts", "CompanyId", "dbo.Companies");
            //DropForeignKey("dbo.AirTicketsReciepts", "RecieptFileId", "dbo.CustomFiles");
            //DropForeignKey("dbo.CancelationInvoices", "CompanyId", "dbo.Companies");
            //DropForeignKey("dbo.CancelationInvoices", "RecieptFileId", "dbo.CustomFiles");
            //DropForeignKey("dbo.CreditInvoices", "CompanyId", "dbo.Companies");
            //DropForeignKey("dbo.CreditInvoices", "RecieptFileId", "dbo.CustomFiles");
            //DropForeignKey("dbo.FerryTicketsReciepts", "CompanyId", "dbo.Companies");
            //DropForeignKey("dbo.FerryTicketsReciepts", "RecieptFileId", "dbo.CustomFiles");
            //DropForeignKey("dbo.ServiceInvoices", "CompanyId", "dbo.Companies");
            //DropForeignKey("dbo.ServiceInvoices", "RecieptFileId", "dbo.CustomFiles");
            //DropForeignKey("dbo.ServiceReciepts", "CompanyId", "dbo.Companies");
            //DropForeignKey("dbo.ServiceReciepts", "RecieptFileId", "dbo.CustomFiles");
            //DropIndex("dbo.AirTicketsReciepts", new[] { "CompanyId" });
            //DropIndex("dbo.AirTicketsReciepts", new[] { "RecieptFileId" });
            //DropIndex("dbo.CancelationInvoices", new[] { "CompanyId" });
            //DropIndex("dbo.CancelationInvoices", new[] { "RecieptFileId" });
            //DropIndex("dbo.CreditInvoices", new[] { "CompanyId" });
            //DropIndex("dbo.CreditInvoices", new[] { "RecieptFileId" });
            //DropIndex("dbo.FerryTicketsReciepts", new[] { "CompanyId" });
            //DropIndex("dbo.FerryTicketsReciepts", new[] { "RecieptFileId" });
            //DropIndex("dbo.ServiceInvoices", new[] { "CompanyId" });
            //DropIndex("dbo.ServiceInvoices", new[] { "RecieptFileId" });
            //DropIndex("dbo.ServiceReciepts", new[] { "CompanyId" });
            //DropIndex("dbo.ServiceReciepts", new[] { "RecieptFileId" });
            ////DropTable("dbo.AirTicketsReciepts");
            ////DropTable("dbo.CancelationInvoices");
            ////DropTable("dbo.CreditInvoices");
            ////DropTable("dbo.FerryTicketsReciepts");
            ////DropTable("dbo.ServiceInvoices");
            ////DropTable("dbo.ServiceReciepts");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ServiceReciepts", "RecieptFileId");
            CreateIndex("dbo.ServiceReciepts", "CompanyId");
            CreateIndex("dbo.ServiceInvoices", "RecieptFileId");
            CreateIndex("dbo.ServiceInvoices", "CompanyId");
            CreateIndex("dbo.FerryTicketsReciepts", "RecieptFileId");
            CreateIndex("dbo.FerryTicketsReciepts", "CompanyId");
            CreateIndex("dbo.CreditInvoices", "RecieptFileId");
            CreateIndex("dbo.CreditInvoices", "CompanyId");
            CreateIndex("dbo.CancelationInvoices", "RecieptFileId");
            CreateIndex("dbo.CancelationInvoices", "CompanyId");
            CreateIndex("dbo.AirTicketsReciepts", "RecieptFileId");
            CreateIndex("dbo.AirTicketsReciepts", "CompanyId");
            AddForeignKey("dbo.ServiceReciepts", "RecieptFileId", "dbo.CustomFiles", "Id");
            AddForeignKey("dbo.ServiceReciepts", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.ServiceInvoices", "RecieptFileId", "dbo.CustomFiles", "Id");
            AddForeignKey("dbo.ServiceInvoices", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.FerryTicketsReciepts", "RecieptFileId", "dbo.CustomFiles", "Id");
            AddForeignKey("dbo.FerryTicketsReciepts", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.CreditInvoices", "RecieptFileId", "dbo.CustomFiles", "Id");
            AddForeignKey("dbo.CreditInvoices", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.CancelationInvoices", "RecieptFileId", "dbo.CustomFiles", "Id");
            AddForeignKey("dbo.CancelationInvoices", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.AirTicketsReciepts", "RecieptFileId", "dbo.CustomFiles", "Id");
            AddForeignKey("dbo.AirTicketsReciepts", "CompanyId", "dbo.Companies", "Id");
        }
    }
}
