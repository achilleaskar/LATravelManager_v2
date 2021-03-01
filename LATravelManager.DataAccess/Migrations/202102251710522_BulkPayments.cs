namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BulkPayments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BulkPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Comment = c.String(maxLength: 200, unicode: false),
                        PaymentMethod = c.Int(nullable: false),
                        Partner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Partners", t => t.Partner_Id)
                .Index(t => t.Partner_Id);
            
            AddColumn("dbo.Payments", "BulkPayment_Id", c => c.Int());
            CreateIndex("dbo.Payments", "BulkPayment_Id");
            AddForeignKey("dbo.Payments", "BulkPayment_Id", "dbo.BulkPayments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "BulkPayment_Id", "dbo.BulkPayments");
            DropForeignKey("dbo.BulkPayments", "Partner_Id", "dbo.Partners");
            DropIndex("dbo.BulkPayments", new[] { "Partner_Id" });
            DropIndex("dbo.Payments", new[] { "BulkPayment_Id" });
            DropColumn("dbo.Payments", "BulkPayment_Id");
            DropTable("dbo.BulkPayments");
        }
    }
}
