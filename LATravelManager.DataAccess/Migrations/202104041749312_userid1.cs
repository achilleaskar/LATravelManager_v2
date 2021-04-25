namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userid1 : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Payments", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Payments", name: "User_Id", newName: "UserId");
        }
    }
}
