namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _193 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Excursions", "NightStart", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Excursions", "NightStart");
        }
    }
}
