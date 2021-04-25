namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class no1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BulkPayments", "Partner_Id", "dbo.Partners");
            DropIndex("dbo.BulkPayments", new[] { "Partner_Id" });
            DropColumn("dbo.BulkPayments", "Partner_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BulkPayments", "Partner_Id", c => c.Int());
            CreateIndex("dbo.BulkPayments", "Partner_Id");
            AddForeignKey("dbo.BulkPayments", "Partner_Id", "dbo.Partners", "Id");
        }
    }
}
