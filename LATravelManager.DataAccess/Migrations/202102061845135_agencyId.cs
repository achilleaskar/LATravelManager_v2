namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agencyId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecieptSeries", "AgencyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecieptSeries", "AgencyId");
        }
    }
}
