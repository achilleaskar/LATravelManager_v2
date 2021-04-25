namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userid : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            RenameIndex(table: "Payments", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "Payments", name: "UserId", newName: "User_Id");
        }
    }
}
