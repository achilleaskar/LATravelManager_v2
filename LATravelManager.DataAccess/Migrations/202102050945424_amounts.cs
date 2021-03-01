namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amounts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reciepts", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.RecieptItems", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecieptItems", "Amount");
            DropColumn("dbo.Reciepts", "Total");
        }
    }
}
