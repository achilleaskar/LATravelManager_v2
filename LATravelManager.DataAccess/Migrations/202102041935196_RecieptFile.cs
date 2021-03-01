namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecieptFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecieptItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 100, unicode: false),
                        Dates = c.String(maxLength: 60, unicode: false),
                        ReservationId = c.Int(nullable: false),
                        Pax = c.Int(nullable: false),
                        Extra = c.String(maxLength: 100, unicode: false),
                        Names = c.String(maxLength: 100, unicode: false),
                        Reciept_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reciepts", t => t.Reciept_Id)
                .Index(t => t.Reciept_Id);
            
            AddColumn("dbo.Reciepts", "FileName", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Reciepts", "Content", c => c.Binary());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecieptItems", "Reciept_Id", "dbo.Reciepts");
            DropIndex("dbo.RecieptItems", new[] { "Reciept_Id" });
            DropColumn("dbo.Reciepts", "Content");
            DropColumn("dbo.Reciepts", "FileName");
            DropTable("dbo.RecieptItems");
            CreateIndex("dbo.Reciepts", "RecieptFileId");
            AddForeignKey("dbo.Reciepts", "RecieptFileId", "dbo.CustomFiles", "Id");
        }
    }
}
