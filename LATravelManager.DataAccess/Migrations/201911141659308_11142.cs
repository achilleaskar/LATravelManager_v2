namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11142 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "SeatNumRet", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "SeatNumRet");
        }
    }
}
