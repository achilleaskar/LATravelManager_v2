namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class da1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceCustomers",
                c => new
                {
                    Service_Id = c.Int(nullable: false),
                    Customer_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Service_Id, t.Customer_Id })
                .ForeignKey("dbo.Services", t => t.Service_Id, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Service_Id)
                .Index(t => t.Customer_Id);
        }

        public override void Down()
        {
            AddColumn("dbo.Customers", "PlaneService_Id", c => c.Int());
            DropForeignKey("dbo.ServiceCustomers", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.ServiceCustomers", "Service_Id", "dbo.Services");
            DropIndex("dbo.ServiceCustomers", new[] { "Customer_Id" });
            DropIndex("dbo.ServiceCustomers", new[] { "Service_Id" });
            DropTable("dbo.ServiceCustomers");
            CreateIndex("dbo.Customers", "PlaneService_Id");
            AddForeignKey("dbo.Customers", "PlaneService_Id", "dbo.Services", "Id");
        }
    }
}