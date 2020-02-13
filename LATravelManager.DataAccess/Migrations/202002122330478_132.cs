namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _132 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExtraServices", "Customer_Id", c => c.Int());
            CreateIndex("dbo.ExtraServices", "Customer_Id");
            AddForeignKey("dbo.ExtraServices", "Customer_Id", "dbo.Customers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtraServices", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.ExtraServices", new[] { "Customer_Id" });
            DropColumn("dbo.ExtraServices", "Customer_Id");
        }
    }
}
