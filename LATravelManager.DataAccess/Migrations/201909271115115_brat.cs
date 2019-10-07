namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class brat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partners", "Person", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Partners", "Person");
        }
    }
}
