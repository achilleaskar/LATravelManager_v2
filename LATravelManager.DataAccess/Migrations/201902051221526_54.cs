namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _54 : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Room_Id", c => c.Int());
            CreateIndex("dbo.Customers", "Room_Id");
            AddForeignKey("dbo.Customers", "Room_Id", "dbo.Rooms", "Id");
        }
    }
}
