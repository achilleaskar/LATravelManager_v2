namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class disableCompanies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Disabled", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "Disabled");
        }
    }
}
