namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReciepSeries", "Letter", c => c.String(maxLength: 200, fixedLength: true, unicode: false, storeType: "char"));
            AddColumn("dbo.ReciepSeries", "RecieptType", c => c.Int(nullable: false));
            AddColumn("dbo.ReciepSeries", "CurrentNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReciepSeries", "CurrentNumber");
            DropColumn("dbo.ReciepSeries", "RecieptType");
            DropColumn("dbo.ReciepSeries", "Letter");
        }
    }
}
