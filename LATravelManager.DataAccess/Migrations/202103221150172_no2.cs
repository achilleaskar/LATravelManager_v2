namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class no2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BulkPayments", "PartnerId", c => c.Int());
            CreateIndex("dbo.BulkPayments", "PartnerId");
            AddForeignKey("dbo.BulkPayments", "PartnerId", "dbo.Partners", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BulkPayments", "PartnerId", "dbo.Partners");
            DropIndex("dbo.BulkPayments", new[] { "PartnerId" });
            DropColumn("dbo.BulkPayments", "PartnerId");
        }
    }
}
