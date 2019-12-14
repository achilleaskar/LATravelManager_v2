namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1215 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "BusGo_Id", c => c.Int());
            AddColumn("dbo.Customers", "BusReturn_Id", c => c.Int());
            CreateIndex("dbo.Customers", "BusGo_Id");
            CreateIndex("dbo.Customers", "BusReturn_Id");
            AddForeignKey("dbo.Customers", "BusGo_Id", "dbo.Buses", "Id");
            AddForeignKey("dbo.Customers", "BusReturn_Id", "dbo.Buses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "BusReturn_Id", "dbo.Buses");
            DropForeignKey("dbo.Customers", "BusGo_Id", "dbo.Buses");
            DropIndex("dbo.Customers", new[] { "BusReturn_Id" });
            DropIndex("dbo.Customers", new[] { "BusGo_Id" });
            DropColumn("dbo.Customers", "BusReturn_Id");
            DropColumn("dbo.Customers", "BusGo_Id");
        }
    }
}
