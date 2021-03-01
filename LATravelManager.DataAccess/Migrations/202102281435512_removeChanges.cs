namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("ChangeInBookings", "LoginData_Id", "LoginDatas");
            DropIndex("ChangeInBookings", new[] { "LoginData_Id" });
            DropColumn("ChangeInBookings", "LoginData_Id");
        }
        
        public override void Down()
        {
            AddColumn("ChangeInBookings", "LoginData_Id", c => c.Int());
            CreateIndex("ChangeInBookings", "LoginData_Id");
            AddForeignKey("ChangeInBookings", "LoginData_Id", "dbo.LoginDatas", "Id");
        }
    }
}
