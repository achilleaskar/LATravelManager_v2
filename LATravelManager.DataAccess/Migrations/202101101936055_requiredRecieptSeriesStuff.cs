namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredRecieptSeriesStuff : DbMigration
    {
        public override void Up()
        {
           // AlterColumn("dbo.RecieptSerie", "Letter", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RecieptSeries", "Letter", c => c.String(maxLength: 200, fixedLength: true, unicode: false, storeType: "char"));
        }
    }
}
