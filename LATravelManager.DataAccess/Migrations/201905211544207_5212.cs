namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5212 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomTypes", "Index", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomTypes", "Index");
        }
    }
}
