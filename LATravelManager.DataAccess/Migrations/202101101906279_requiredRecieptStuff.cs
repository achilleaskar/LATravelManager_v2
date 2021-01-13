namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredRecieptStuff : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Reciepts", "Series_Id", "dbo.RecieptSeries");
            //DropIndex("dbo.Reciepts", new[] { "Series_Id" });
            //AlterColumn("dbo.Reciepts", "Series_Id", c => c.Int(nullable: false));
            //CreateIndex("dbo.Reciepts", "Series_Id");
            //AddForeignKey("dbo.Reciepts", "Series_Id", "dbo.RecieptSeries", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reciepts", "Series_Id", "dbo.RecieptSeries");
            DropIndex("dbo.Reciepts", new[] { "Series_Id" });
            AlterColumn("dbo.Reciepts", "Series_Id", c => c.Int());
            CreateIndex("dbo.Reciepts", "Series_Id");
            AddForeignKey("dbo.Reciepts", "Series_Id", "dbo.RecieptSeries", "Id");
        }
    }
}
