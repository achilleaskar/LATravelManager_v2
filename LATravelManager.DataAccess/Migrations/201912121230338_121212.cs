namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _121212 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerOptionals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Note = c.String(maxLength: 200, unicode: false),
                        Customer_Id = c.Int(),
                        OptionalExcursion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.OptionalExcursions", t => t.OptionalExcursion_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.OptionalExcursion_Id);
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OptionalExcursionCustomers",
                c => new
                    {
                        OptionalExcursion_Id = c.Int(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OptionalExcursion_Id, t.Customer_Id });
            
            DropForeignKey("dbo.CustomerOptionals", "OptionalExcursion_Id", "dbo.OptionalExcursions");
            DropForeignKey("dbo.CustomerOptionals", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.CustomerOptionals", new[] { "OptionalExcursion_Id" });
            DropIndex("dbo.CustomerOptionals", new[] { "Customer_Id" });
            DropTable("dbo.CustomerOptionals");
            CreateIndex("dbo.OptionalExcursionCustomers", "Customer_Id");
            CreateIndex("dbo.OptionalExcursionCustomers", "OptionalExcursion_Id");
            AddForeignKey("dbo.OptionalExcursionCustomers", "Customer_Id", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OptionalExcursionCustomers", "OptionalExcursion_Id", "dbo.OptionalExcursions", "Id", cascadeDelete: true);
        }
    }
}
