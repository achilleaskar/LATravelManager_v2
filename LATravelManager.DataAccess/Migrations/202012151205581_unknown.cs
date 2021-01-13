namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unknown : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            AddColumn("dbo.ThirdParty_Booking", "Billed", c => c.Boolean(nullable: false, storeType: "bit"));
        }
    }
}
