namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastPrintTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecieptSeries", "LastPrint", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecieptSeries", "LastPrint");
        }
    }
}
