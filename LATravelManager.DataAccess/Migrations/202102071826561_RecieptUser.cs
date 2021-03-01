namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecieptUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reciepts", "User_Id", c => c.Int());
            CreateIndex("dbo.Reciepts", "User_Id");
            AddForeignKey("dbo.Reciepts", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reciepts", "User_Id", "dbo.Users");
            DropIndex("dbo.Reciepts", new[] { "User_Id" });
            DropColumn("dbo.Reciepts", "User_Id");
        }
    }
}
