namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniqueKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecieptSeries", "SerieCode", c => c.String(maxLength: 40, unicode: false));
            CreateIndex("dbo.RecieptSeries", "SerieCode", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.RecieptSeries", new[] { "SerieCode" });
            DropColumn("dbo.RecieptSeries", "SerieCode");
        }
    }
}
