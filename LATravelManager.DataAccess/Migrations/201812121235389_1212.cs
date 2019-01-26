namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1212 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200, unicode: false),
                        StartingPlace = c.String(maxLength: 200, unicode: false),
                        Tel = c.String(maxLength: 200, unicode: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        ModifiedDate = c.DateTime(precision: 0),
                        Excursion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Excursions", t => t.Excursion_Id)
                .Index(t => t.Excursion_Id);
            
            CreateTable(
                "dbo.Leaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tel = c.String(maxLength: 200, unicode: false),
                        Name = c.String(maxLength: 200, unicode: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        ModifiedDate = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Customers", "Bus_Id", c => c.Int());
            CreateIndex("dbo.Customers", "Bus_Id");
            AddForeignKey("dbo.Customers", "Bus_Id", "dbo.Buses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Buses", "Excursion_Id", "dbo.Excursions");
            DropForeignKey("dbo.Customers", "Bus_Id", "dbo.Buses");
            DropIndex("dbo.Buses", new[] { "Excursion_Id" });
            DropIndex("dbo.Customers", new[] { "Bus_Id" });
            DropColumn("dbo.Customers", "Bus_Id");
            DropTable("dbo.Leaders");
            DropTable("dbo.Buses");
        }
    }
}
