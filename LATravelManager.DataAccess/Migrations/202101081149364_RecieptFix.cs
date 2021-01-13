namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecieptFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReciepSeries", "DateEnded", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReciepSeries", "DateEnded", c => c.DateTime(nullable: false, precision: 0));
        }
    }
}
