namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _12112 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OptionalExcursions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200, unicode: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        ModifiedDate = c.DateTime(precision: 0),
                        Excursion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Excursions", t => t.Excursion_Id)
                .Index(t => t.Excursion_Id);
            
            AddColumn("dbo.Customers", "OptionalExcursion_Id", c => c.Int());
            CreateIndex("dbo.Customers", "OptionalExcursion_Id");
            AddForeignKey("dbo.Customers", "OptionalExcursion_Id", "dbo.OptionalExcursions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptionalExcursions", "Excursion_Id", "dbo.Excursions");
            DropForeignKey("dbo.Customers", "OptionalExcursion_Id", "dbo.OptionalExcursions");
            DropIndex("dbo.OptionalExcursions", new[] { "Excursion_Id" });
            DropIndex("dbo.Customers", new[] { "OptionalExcursion_Id" });
            DropColumn("dbo.Customers", "OptionalExcursion_Id");
            DropTable("dbo.OptionalExcursions");
        }
    }
}
